using Data.Player;
using Util.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] public PlayerMovementData PlayerMovement;

        private CharacterController _characterController;
        private PlayerInputController _input;

        private float _currSpeed;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputController>();
        }

        void FixedUpdate()
        {
            if (_input.PlayerInput.IsAccelerating == _input.PlayerInput.IsBraking)
            {
                Debug.Log($"{_currSpeed} Deccelerating");
                var newSpeed = _currSpeed + (_currSpeed > 0 ? -1 : 1) * PlayerMovement.DeccelerationSpeed;
                _currSpeed = _currSpeed.IsZero() || _currSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.PlayerInput.IsAccelerating)
            {
                Debug.Log($"{_currSpeed} Accelerating");
                _currSpeed = _currSpeed >= PlayerMovement.MaxSpeed ? PlayerMovement.MaxSpeed : _currSpeed + PlayerMovement.AccelerationSpeed;
            } 
            else if (_input.PlayerInput.IsBraking)
            {
                // if moving forward, slow down, else reverse
                if (_currSpeed > 0)
                {
                    Debug.Log($"{_currSpeed} Braking");
                    _currSpeed = _currSpeed <= 0 ? 0 : _currSpeed - PlayerMovement.BrakeSpeed;
                }
                else
                {
                    Debug.Log($"{_currSpeed} Reversing");
                    _currSpeed = _currSpeed <= -PlayerMovement.MaxReverseSpeed ? -PlayerMovement.MaxReverseSpeed : _currSpeed - PlayerMovement.ReverseAccelerationSpeed;
                }
            }                

            var forward = _characterController.transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var steerDirection = _input.PlayerInput.Steering.x * PlayerMovement.TurningSpeed;

            if (!_currSpeed.IsZero())
                _characterController.transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            var movement = forward * _currSpeed * Time.fixedDeltaTime;
            if (!_characterController.isGrounded)
                movement.y -= PlayerMovement.GravitySpeed;
            else
                movement.y -= PlayerMovement.ConstantGravitySpeed;

            _characterController.Move(movement);
        }
    }
}
