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
            if (_isBoosting)
            {
                // Boosting
                _currSpeed = RacerMovement.MaxSpeed + RacerMovement.MaxSpeed * _currBoostPower;
            }
            if (_input.PlayerInput.IsAccelerating == _input.PlayerInput.IsBraking || _currSpeed >= RacerMovement.MaxSpeed)
            {
                // Coasting
                var newSpeed = _currSpeed + (_currSpeed > 0 ? -1 : 1) * RacerMovement.DecelerationSpeed;
                _currSpeed = _currSpeed.IsZero() || _currSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.PlayerInput.IsAccelerating)
            {
                // Acceleration
                _currSpeed = _currSpeed >= RacerMovement.MaxSpeed
                    ? RacerMovement.MaxSpeed
                    : _currSpeed + RacerMovement.AccelerationSpeed;
            } 
            else if (_input.PlayerInput.IsBraking)
            {
                // Braking or reversing
                if (_currSpeed > 0)
                    _currSpeed = _currSpeed <= 0 ? 
                        0 : 
                        _currSpeed - RacerMovement.BrakeSpeed;
                else
                    _currSpeed = _currSpeed <= -RacerMovement.MaxReverseSpeed
                        ? -RacerMovement.MaxReverseSpeed
                        : _currSpeed - RacerMovement.ReverseAccelerationSpeed;
            }                

            // Calculate direction
            var forward = transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var steerDirection = _input.PlayerInput.Steering.x * RacerMovement.TurningSpeed;

            if (!_currSpeed.IsZero())
                transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            var movement = forward * _currSpeed * Time.fixedDeltaTime;
            
            // Apply gravity
            movement.y -= !IsGrounded() ? RacerMovement.GravitySpeed : RacerMovement.ConstantGravitySpeed;

            // Move player
            transform.position += movement;
        }
    }
}
