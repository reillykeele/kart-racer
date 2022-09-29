using Manager;
using UnityEngine;
using Util.Coroutine;
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

            // Handle acceleration and deceleration 
            if (IsBoosting)
            {
                // Boosting
                CurrSpeed = RacerMovement.MaxSpeed + RacerMovement.MaxSpeed * CurrBoostPower;
            }
            if (_input.PlayerInput.IsAccelerating == _input.PlayerInput.IsBraking || CurrSpeed >= RacerMovement.MaxSpeed)
            {
                // Coasting
                var newSpeed = CurrSpeed + (CurrSpeed > 0 ? -1 : 1) * RacerMovement.DecelerationSpeed;
                CurrSpeed = CurrSpeed.IsZero() || CurrSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.PlayerInput.IsAccelerating)
            {
                // Acceleration
                CurrSpeed = CurrSpeed >= RacerMovement.MaxSpeed
                    ? RacerMovement.MaxSpeed
                    : CurrSpeed + RacerMovement.AccelerationSpeed;
            } 
            else if (_input.PlayerInput.IsBraking)
            {
                // Braking or reversing
                if (CurrSpeed > 0)
                    CurrSpeed = CurrSpeed <= 0 ? 
                        0 : 
                        CurrSpeed - RacerMovement.BrakeSpeed;
                else
                    CurrSpeed = CurrSpeed <= -RacerMovement.MaxReverseSpeed
                        ? -RacerMovement.MaxReverseSpeed
                        : CurrSpeed - RacerMovement.ReverseAccelerationSpeed;
            }

            // Calculate direction
            var forward = transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);

            var movement = forward * CurrSpeed * Time.fixedDeltaTime;

            float steerDirection;
            var steeringX = _input.PlayerInput.Steering.x;
            if (_isDrifting)
            {
                if (DriftDirection == 0)
                    DriftDirection = steeringX == 0 ? 0 : (int) Mathf.Sign(steeringX);
                
                steerDirection = 
                    DriftDirection * (RacerMovement.TurningSpeed * 1.25f) + 
                    steeringX * (RacerMovement.TurningSpeed * 0.75f);
                
                // transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

                movement += transform.right * (CurrSpeed * 0.01f) * -DriftDirection;
            }
            else
            {
                DriftDirection = 0;
                steerDirection = steeringX * RacerMovement.TurningSpeed;
                // if (!CurrSpeed.IsZero())
                //     transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);
            }

            if (!CurrSpeed.IsZero())
                transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            // Apply gravity
            movement.y -= !IsGrounded() ? RacerMovement.GravitySpeed : RacerMovement.ConstantGravitySpeed;

            // Move player
            transform.position += movement;
        }
    }
}
