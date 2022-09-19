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
            var steerDirection = _input.PlayerInput.Steering.x * RacerMovement.TurningSpeed;

            if (!CurrSpeed.IsZero())
                transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            var movement = forward * CurrSpeed * Time.fixedDeltaTime;
            
            // Apply gravity
            movement.y -= !IsGrounded() ? RacerMovement.GravitySpeed : RacerMovement.ConstantGravitySpeed;

            // Move player
            transform.position += movement;
        }
    }
}
