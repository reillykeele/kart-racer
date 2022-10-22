using Data.Environment;
using Environment.Track;
using Manager;
using UnityEngine;
using Util.Helpers;

namespace Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMovementController : RacerMovementController
    {
        private PlayerInputController _input;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInputController>();
            _input.OnDriftEvent.AddListener(isDrifting => IsDrifting = isDrifting);
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            var isGrounded = IsGrounded(out var hitInfo);

            // Check the surface we are on 
            var trackSurfaceModifier = new TrackSurfaceModifierData();
            if (isGrounded)
            {
                var trackSurface = hitInfo.collider.GetComponent<TrackSurface>();
                if (trackSurface != null)
                    trackSurfaceModifier = trackSurface.TrackSurfaceModifierData;
            }

            // Handle acceleration and deceleration 
            if (IsBoosting)
            {
                // Boosting
                CurrSpeed = RacerMovement.MaxSpeed + RacerMovement.MaxSpeed * CurrBoostPower;
            }

            if (_input.PlayerInput.IsAccelerating == _input.PlayerInput.IsBraking ||
                CurrSpeed >= RacerMovement.MaxSpeed)
            {
                // Coasting
                var newSpeed = CurrSpeed + (CurrSpeed > 0 ? -1 : 1) * RacerMovement.DecelerationSpeed *
                    trackSurfaceModifier.DeccelerationModifier;
                CurrSpeed = CurrSpeed.IsZero() || CurrSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.PlayerInput.IsAccelerating)
            {
                // Acceleration
                CurrSpeed = CurrSpeed >= RacerMovement.MaxSpeed * trackSurfaceModifier.SpeedModifier
                    ? RacerMovement.MaxSpeed * trackSurfaceModifier.SpeedModifier
                    : CurrSpeed + RacerMovement.AccelerationSpeed * trackSurfaceModifier.AccelerationModifier;
            }
            else if (_input.PlayerInput.IsBraking)
            {
                // Braking or reversing
                if (CurrSpeed > 0)
                    CurrSpeed = CurrSpeed <= 0 ? 0 : CurrSpeed - RacerMovement.BrakeSpeed;
                else
                    CurrSpeed = CurrSpeed <= -RacerMovement.MaxReverseSpeed
                        ? -RacerMovement.MaxReverseSpeed
                        : CurrSpeed - RacerMovement.ReverseAccelerationSpeed;
            }

            // Calculate direction
            var forward = transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);

            Vector3 movement;

            // Calculate and apply steering
            float steerDirection;
            var steeringX = _input.PlayerInput.Steering.x;
            if (_isDrifting)
            {
                if (DriftDirection == 0)
                    DriftDirection = steeringX == 0 ? 0 : (int)Mathf.Sign(steeringX);

                steerDirection =
                    DriftDirection * (RacerMovement.TurningSpeed * 1.25f) +
                    steeringX * (RacerMovement.TurningSpeed * 0.75f);

                // Add outward velocity 
                var dir = (forward + transform.right * RacerMovement.OutwardDriftPercentage * -DriftDirection).normalized;
                movement = dir * CurrSpeed;

                // Calculate turbo progress
                ++_driftProgress;
                if (DriftLevel < 3 && _driftProgress >= 225)
                    DriftLevel = 3;
                else if (DriftLevel < 2 && _driftProgress >= 150)
                    DriftLevel = 2;
                else if (DriftLevel < 1 && _driftProgress >= 75)
                    DriftLevel = 1;
            }
            else
            {
                movement = forward * CurrSpeed;

                DriftDirection = 0;
                steerDirection = steeringX * RacerMovement.TurningSpeed;
            }

            // Apply gravity
            var pos = transform.position;
            if (isGrounded)
            {
                pos.y = hitInfo.point.y + 0.5f; // TODO: Change to some "dist to ground" var

                var groundNormal = hitInfo.normal;
                var forwardDirection = forward.normalized;

                // Project the forward & surface normal using the dot product
                // Set the rotation w/ relative forward and up axes
                var rotForward = forwardDirection - groundNormal * Vector3.Dot(forwardDirection, groundNormal);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(rotForward.normalized, groundNormal),
                    8f * Time.fixedDeltaTime);
            }
            else
                movement.y -= RacerMovement.GravitySpeed;

            // Move & rotate player
            if (!CurrSpeed.IsZero())
                transform.Rotate(transform.up, steerDirection * Time.fixedDeltaTime);

            movement *= Time.fixedDeltaTime;

            if (Physics.BoxCast(
                    _collider.bounds.center + movement,
                    _collider.bounds.extents,
                    forward,
                    out var boxHit,
                    transform.rotation,
                    _collider.bounds.extents.z,
                    LayerMask.GetMask("Walls")))
            {
                Debug.Log(boxHit.transform.name);
            }
            else
            {
                transform.position = pos + movement;
            }
        }
    }
}
