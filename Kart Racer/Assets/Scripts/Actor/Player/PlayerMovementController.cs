using Data.Player;
using Manager;
using Util.Helpers;
using UnityEngine;

namespace Actor.Player
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] public PlayerMovementData PlayerMovement;

        // private CharacterController _characterController;
        private Collider _collider;
        private PlayerInputController _input;

        private float _currSpeed;

        void Awake()
        {
            // _characterController = GetComponent<CharacterController>();
            _collider = GetComponent<Collider>();
            _input = GetComponent<PlayerInputController>();
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            if (_input.PlayerInput.IsAccelerating == _input.PlayerInput.IsBraking)
            {
                // Debug.Log($"{_currSpeed} Deccelerating");
                var newSpeed = _currSpeed + (_currSpeed > 0 ? -1 : 1) * PlayerMovement.DeccelerationSpeed;
                _currSpeed = _currSpeed.IsZero() || _currSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.PlayerInput.IsAccelerating)
            {
                // Debug.Log($"{_currSpeed} Accelerating");
                _currSpeed = _currSpeed >= PlayerMovement.MaxSpeed ? PlayerMovement.MaxSpeed : _currSpeed + PlayerMovement.AccelerationSpeed;
            } 
            else if (_input.PlayerInput.IsBraking)
            {
                // if moving forward, slow down, else reverse
                if (_currSpeed > 0)
                {
                    // Debug.Log($"{_currSpeed} Braking");
                    _currSpeed = _currSpeed <= 0 ? 0 : _currSpeed - PlayerMovement.BrakeSpeed;
                }
                else
                {
                    // Debug.Log($"{_currSpeed} Reversing");
                    _currSpeed = _currSpeed <= -PlayerMovement.MaxReverseSpeed ? -PlayerMovement.MaxReverseSpeed : _currSpeed - PlayerMovement.ReverseAccelerationSpeed;
                }
            }                

            var forward = transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var steerDirection = _input.PlayerInput.Steering.x * PlayerMovement.TurningSpeed;

            if (!_currSpeed.IsZero())
                transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            var movement = forward * _currSpeed * Time.fixedDeltaTime;
            if (!IsGrounded())
            {
                movement.y -= PlayerMovement.GravitySpeed;
                Debug.Log("Not grounded");
            }
            else
            {
                movement.y -= PlayerMovement.ConstantGravitySpeed;
                Debug.Log("Not grounded");
            }
            
            transform.position += movement;
        }

        public bool IsGrounded()
        {
            Debug.DrawRay(_collider.bounds.center, Vector3.down * (_collider.bounds.extents.y + 0.05f));
            return Physics.Raycast(_collider.bounds.center, Vector3.down, _collider.bounds.extents.y + 0.05f);
        }
    }
}
