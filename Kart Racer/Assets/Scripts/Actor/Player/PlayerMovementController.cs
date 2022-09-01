using Util.Helpers;
using UnityEngine;

namespace Actor.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Speed")]
        public float AccelerationSpeed;
        public float MaxSpeed;

        public float ReverseAccelerationSpeed;
        public float MaxReverseSpeed;

        public float DeccelerationSpeed;
        public float BrakeSpeed;

        [Header("Handling")]
        public float TurningSpeed;

        [Header("Gravity")]
        public float GravitySpeed;
        public float ConstantGravitySpeed;

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
            if (_input.IsAccelerating == _input.IsBraking)
            {
                Debug.Log($"{_currSpeed} Deccelerating");
                var newSpeed = _currSpeed + (_currSpeed > 0 ? -1 : 1) * DeccelerationSpeed;
                _currSpeed = _currSpeed.IsZero() || _currSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.IsAccelerating)
            {
                Debug.Log($"{_currSpeed} Accelerating");
                _currSpeed = _currSpeed >= MaxSpeed ? MaxSpeed : _currSpeed + AccelerationSpeed;
            } 
            else if (_input.IsBraking)
            {
                // if moving forward, slow down, else reverse
                if (_currSpeed > 0)
                {
                    Debug.Log($"{_currSpeed} Braking");
                    _currSpeed = _currSpeed <= 0 ? 0 : _currSpeed - BrakeSpeed;
                }
                else
                {
                    Debug.Log($"{_currSpeed} Reversing");
                    _currSpeed = _currSpeed <= -MaxReverseSpeed ? -MaxReverseSpeed : _currSpeed - ReverseAccelerationSpeed;
                }
            }                

            var forward = _characterController.transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var steerDirection = _input.Steering.x * TurningSpeed;

            if (!_currSpeed.IsZero())
                _characterController.transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            var movement = forward * _currSpeed * Time.fixedDeltaTime;
            if (!_characterController.isGrounded)
                movement.y -= GravitySpeed;
            else
                movement.y -= ConstantGravitySpeed;

            _characterController.Move(movement);
        }
    }
}
