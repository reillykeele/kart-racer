using UnityEngine;

namespace KartRacer.Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMovementController : RacerMovementController
    {
        private PlayerInputController _input;

        protected override bool IsAccelerating => _input.PlayerInput.IsAccelerating;
        protected override bool IsBraking => _input.PlayerInput.IsBraking;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInputController>();
            _input.OnDriftEvent.AddListener(isDrifting => IsDrifting = isDrifting);
        }

        // protected override void FixedUpdate()
        // {
        //     if (!GameManager.Instance.IsPlaying()) return;
        //
        //     if (UseAutopilot)
        //     {
        //         Autopilot();
        //         return;
        //     }
        //
        //     base.FixedUpdate();
        //     return;
        //
        //     // // Check the surface we are on 
        //     // var isGrounded = IsGrounded(out var groundedHitInfo, out var trackSurfaceModifier);
        //     //
        //     // // Calculate what our current speed should be
        //     // var velocity = _rb.velocity;
        //     // velocity.y = 0;
        //     // CurrSpeed = velocity.magnitude * Mathf.Sign(CurrSpeed);
        //     // CurrSpeed = CalculateSpeed(CurrSpeed, _input.PlayerInput.IsAccelerating, _input.PlayerInput.IsBraking, trackSurfaceModifier);
        //     //
        //     // // Calculate direction
        //     // Debug.DrawRay(transform.position, 3 * transform.forward, Color.yellow);
        //     //
        //     // // Calculate and apply steering
        //     // float steerDirection;
        //     // Vector3 movement;
        //     // Steering = _input.PlayerInput.Steering.x;
        //     // if (_isDrifting)
        //     // {
        //     //     if (DriftDirection == 0)
        //     //         DriftDirection = Steering == 0 ? 0 : (int)Mathf.Sign(Steering);
        //     //
        //     //     steerDirection =
        //     //         DriftDirection * (RacerMovement.TurningSpeed * 1.25f) +
        //     //         Steering * (RacerMovement.TurningSpeed * 0.75f);
        //     //
        //     //     // Add outward velocity 
        //     //     var dir = (transform.forward + transform.right * RacerMovement.OutwardDriftPercentage * -DriftDirection).normalized;
        //     //     movement = dir * CurrSpeed;
        //     //
        //     //     // Calculate turbo progress
        //     //     ++_driftProgress;
        //     //     if (DriftLevel < 3 && _driftProgress >= 225)
        //     //         DriftLevel = 3;
        //     //     else if (DriftLevel < 2 && _driftProgress >= 150)
        //     //         DriftLevel = 2;
        //     //     else if (DriftLevel < 1 && _driftProgress >= 75)
        //     //         DriftLevel = 1;
        //     // }
        //     // else
        //     // {
        //     //     DriftDirection = 0;
        //     //     steerDirection = Steering * RacerMovement.TurningSpeed;
        //     //
        //     //     movement = transform.forward * CurrSpeed;
        //     // }
        //     //
        //     // // Apply gravity
        //     // // var pos = transform.position;
        //     // if (isGrounded)
        //     // {
        //     //     var  cPos = transform.TransformPoint(_collider.center + new Vector3(0, -_collider.size.y,  0) / 2);
        //     //     var tlPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y,  _collider.size.z) / 2);
        //     //     var trPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x, -_collider.size.y,  _collider.size.z) / 2);
        //     //     var blPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y, -_collider.size.z) / 2);
        //     //     var brPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x, -_collider.size.y, -_collider.size.z) / 2);
        //     //
        //     //     # region Debug Draw Gravity Rays
        //     //     Debug.DrawRay( cPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(tlPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(trPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(blPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(brPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(groundedHitInfo.point, groundedHitInfo.normal * (RacerMovement.GroundedDist + _collider.size.y / 2), Color.blue);
        //     //     #endregion
        //     //
        //     //     var pos = transform.position;
        //     //     pos.y = groundedHitInfo.point.y + RacerMovement.GroundedDist + (pos.y - cPos.y);
        //     //     transform.position = pos;
        //     //
        //     //     // Project the forward & surface normal using the dot product
        //     //     // Set the rotation w/ relative forward and up axes
        //     //     var groundNormal = groundedHitInfo.normal;
        //     //     var rotForward = transform.forward - groundNormal * Vector3.Dot(transform.forward, groundNormal);
        //     //     transform.rotation = Quaternion.Slerp(
        //     //         transform.rotation,
        //     //         Quaternion.LookRotation(rotForward.normalized, groundNormal),
        //     //         8f * Time.fixedDeltaTime);
        //     // }
        //     // else
        //     // {
        //     //     movement.y -= RacerMovement.GravitySpeed;
        //     // }
        //     //
        //     // // Move & rotate player
        //     // if (CurrSpeed.IsZero() == false)
        //     //     transform.Rotate(transform.up, steerDirection * Time.fixedDeltaTime);
        //     //
        //     // var newVelocity = 50f * movement * Time.fixedDeltaTime;
        //     // _rb.velocity = newVelocity;
        // }

        public override void Steer(out float steerDirection, out Vector3 movement)
        {
            Steering = _input.PlayerInput.Steering.x;
            if (_isDrifting)
            {
                if (DriftDirection == 0)
                    DriftDirection = Steering == 0 ? 0 : (int)Mathf.Sign(Steering);

                steerDirection =
                    DriftDirection * (RacerMovement.TurningSpeed * 1.25f) +
                    Steering * (RacerMovement.TurningSpeed * 0.75f);

                // Add outward velocity 
                var dir = (transform.forward + transform.right * RacerMovement.OutwardDriftPercentage * -DriftDirection).normalized;
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
                DriftDirection = 0;
                steerDirection = Steering * RacerMovement.TurningSpeed;

                movement = transform.forward * CurrSpeed;
            }
        }
    }
}
