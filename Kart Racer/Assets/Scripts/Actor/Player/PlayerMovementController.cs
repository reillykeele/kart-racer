using Data.Player;
using Manager;
using Util.Helpers;
using UnityEngine;
using Util.Coroutine;

namespace Actor.Player
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] public PlayerMovementData PlayerMovement;

        private Collider _collider;
        private PlayerInputController _input;

        private float _currSpeed;
        
        private bool _isDrifting;
        private int _driftLevel;

        private bool _isBoosting;
        private float _currBoostPower;

        void Awake()
        {
            _collider = GetComponent<Collider>();
            _input = GetComponent<PlayerInputController>();
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            // Handle acceleration and deceleration 
            if (_isBoosting)
            {
                // Boosting
                _currSpeed = PlayerMovement.MaxSpeed + PlayerMovement.MaxSpeed * _currBoostPower;
            }
            if (_input.PlayerInput.IsAccelerating == _input.PlayerInput.IsBraking || _currSpeed >= PlayerMovement.MaxSpeed)
            {
                // Coasting
                var newSpeed = _currSpeed + (_currSpeed > 0 ? -1 : 1) * PlayerMovement.DecelerationSpeed;
                _currSpeed = _currSpeed.IsZero() || _currSpeed * newSpeed < 0 ? 0 : newSpeed;
            }
            else if (_input.PlayerInput.IsAccelerating)
            {
                // Acceleration
                _currSpeed = _currSpeed >= PlayerMovement.MaxSpeed
                    ? PlayerMovement.MaxSpeed
                    : _currSpeed + PlayerMovement.AccelerationSpeed;
            } 
            else if (_input.PlayerInput.IsBraking)
            {
                // Braking or reversing
                if (_currSpeed > 0)
                    _currSpeed = _currSpeed <= 0 ? 
                        0 : 
                        _currSpeed - PlayerMovement.BrakeSpeed;
                else
                    _currSpeed = _currSpeed <= -PlayerMovement.MaxReverseSpeed
                        ? -PlayerMovement.MaxReverseSpeed
                        : _currSpeed - PlayerMovement.ReverseAccelerationSpeed;
            }                

            // Calculate direction
            var forward = transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var steerDirection = _input.PlayerInput.Steering.x * PlayerMovement.TurningSpeed;

            if (!_currSpeed.IsZero())
                transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);

            var movement = forward * _currSpeed * Time.fixedDeltaTime;
            
            // Apply gravity
            movement.y -= !IsGrounded() ? PlayerMovement.GravitySpeed : PlayerMovement.ConstantGravitySpeed;

            // Move player
            transform.position += movement;
        }

        public bool IsGrounded()
        {
            Debug.DrawRay(_collider.bounds.center, Vector3.down * (_collider.bounds.extents.y + 0.05f));
            return Physics.Raycast(_collider.bounds.center, Vector3.down, _collider.bounds.extents.y + 0.05f);
        }

        public void Boost(float boostPower = 1f, float boostDuration = 1f)
        {
            _isBoosting = true;
            _currBoostPower = boostPower;

            StartCoroutine(CoroutineUtil.WaitForExecute(() => _isBoosting = false, PlayerMovement.BoostDuration));
        }
    }
}
