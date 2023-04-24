using System.Linq;
using KartRacer.Actor.Racer.Player;
using UnityEngine;
using Util.Attributes;
using Util.Helpers;

namespace KartRacer
{
    public class GravityTest : MonoBehaviour
    {
        private PlayerInputController _kartInput;
        private Rigidbody _rb;
        private BoxCollider _collider;

        [Header("Move")] 
        [SerializeField, Min(0f)] private float _turnSpeed = 1f;
        [SerializeField, Min(0f)] private float _speed = 1f;
        [SerializeField, Min(0f)] private float _topSpeed = 10f;
        [SerializeField, Range(0f, 1f)] private float _gripiness = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _torqiness = 1.0f;
        [SerializeField] private AnimationCurve _accelerationCurve;

        [Header("Suspension")]
        [SerializeField, Min(0f)] private float _groundCheckDist = 1f;
        [SerializeField, Min(0f)] private float _maxOffset = 0.4f;
        [SerializeField, Min(0f)] private float _strength = 10f;
        [SerializeField, Min(0f)] private float _damping = 10f;
        [SerializeField, Min(0f)] private float _scale = 1f;

        [Header("Debug")] 
        [SerializeField, ReadOnly] private Vector3 _horizontalVelocity;
        [SerializeField, ReadOnly] private float _forwardSpeed;
        [SerializeField, ReadOnly] private float _rightSpeed;
        [SerializeField, ReadOnly] private float _torque;

        private float _accelCurveTopSpeed => _accelerationCurve.keys.Last().value;

        void Awake()
        {
            _kartInput = GetComponent<PlayerInputController>();
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
        }

        void FixedUpdate()
        {
            var isGrounded = CheckGround();

            if (isGrounded)
            {
                _horizontalVelocity = _rb.velocity.GetHorizontal();
                _forwardSpeed = Vector3.Dot(transform.forward, _horizontalVelocity);
                _rightSpeed = Vector3.Dot(transform.right, _horizontalVelocity);

                // calculate forward motion
                if (_kartInput.IsAccelerating)
                {
                    // if (horizontalVelocity.magnitude < _topSpeed)
                    //     _rb.AddForce(_speed * transform.forward);

                    if (_forwardSpeed < _accelCurveTopSpeed)
                    {
                        var nextSpeed = GetNextSpeed(_forwardSpeed);

                        var deltaSpeed = nextSpeed - _forwardSpeed;

                        _rb.AddForce(deltaSpeed * transform.forward);
                    }
                }
                else if (_kartInput.IsBraking)
                {
                    if (_forwardSpeed > -10f)
                        _rb.AddForce(-5f * transform.forward);
                }

                // calculate sliding motion 
                var slip = -_rightSpeed * _gripiness;
                _rb.AddForce(slip / Time.deltaTime * transform.right);


                if (_kartInput.Steering.x != 0f && Mathf.Abs(_forwardSpeed) >= 1.0f)
                {
                    var torque = _kartInput.Steering.x * _turnSpeed;
                    _rb.AddTorque(torque * transform.up);
                }
                else
                {
                    var torque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);
                    if (torque != 0f)
                        _rb.AddTorque(-torque * _torqiness * transform.up);
                }
            }
        }

        private float GetNextSpeed(float currSpeed)
        {
            var leftTime = 0f;
            var rightTime = _accelerationCurve.keys.Last().time;

            var leftSpeed = _accelerationCurve.Evaluate(leftTime);
            var rightSpeed = _accelerationCurve.Evaluate(rightTime);

            var time = 0f;
            if (leftSpeed == currSpeed)
                time = leftTime;
            else if (rightSpeed == currSpeed)
                time = rightTime;
            else 
                time = BinarySearchCurve(currSpeed , leftTime, rightTime, 0);

            return _accelerationCurve.Evaluate(time + Time.deltaTime);
        }

        private const float _maxRuns = 100;
        private float BinarySearchCurve(float speed, float leftTime, float rightTime, int runs)
        {
            var middleTime = rightTime / 2 + leftTime;
            if (runs >= _maxRuns)
                return middleTime;

            var middleSpeed = _accelerationCurve.Evaluate(middleTime);

            if (Mathf.Abs(speed - middleSpeed) < 0.01)
                return middleTime;
            if (middleSpeed < speed)
                return BinarySearchCurve(speed, leftTime, middleTime, runs + 1);
            if (middleSpeed > speed)
                return BinarySearchCurve(speed, middleTime, rightTime, runs + 1);

            return middleTime;
        }

        private bool CheckGround()
        {
            var cPos = transform.TransformPoint(_collider.center + new Vector3(0, -_collider.size.y, 0) / 2);
            var tlPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y, _collider.size.z) / 2);
            var trPos = transform.TransformPoint(_collider.center + new Vector3(_collider.size.x, -_collider.size.y, _collider.size.z) / 2);
            var blPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y, -_collider.size.z) / 2);
            var brPos = transform.TransformPoint(_collider.center + new Vector3(_collider.size.x, -_collider.size.y, -_collider.size.z) / 2);

            var pos = new[] { cPos, tlPos, trPos, blPos, brPos };

            var groundRays = new (bool isHit, RaycastHit hitInfo)[5];
            groundRays[0].isHit = Physics.Raycast(cPos, -transform.up * _groundCheckDist,
                out groundRays[0].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[1].isHit = Physics.Raycast(tlPos, -transform.up * _groundCheckDist,
                out groundRays[1].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[2].isHit = Physics.Raycast(trPos, -transform.up * _groundCheckDist,
                out groundRays[2].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[3].isHit = Physics.Raycast(blPos, -transform.up * _groundCheckDist,
                out groundRays[3].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[4].isHit = Physics.Raycast(brPos, -transform.up * _groundCheckDist,
                out groundRays[4].hitInfo, LayerMask.NameToLayer("Track"));

            var isGrounded = false;
            for (var i = 1; i < groundRays.Length; ++i)
            {
                // Debug.DrawRay(pos[i], -transform.up * _groundCheckDist, Color.red);
                if (groundRays[i].isHit)
                {
                    var distFromGround = _groundCheckDist - groundRays[i].hitInfo.distance;
                    var offset = Mathf.Clamp(distFromGround, -_maxOffset, _maxOffset);

                    // project the tire's velocity along its up
                    var tireVel = _rb.GetPointVelocity(pos[i]);
                    var vel = Vector3.Dot(transform.up, tireVel);

                    var suspensionForce = CalculateSuspensionForce(offset, vel);
                    _rb.AddForceAtPosition(transform.up * suspensionForce, pos[i]);

                    isGrounded = true;
                }
            }

            return isGrounded;
        }

        private float CalculateSuspensionForce(float offset, float velocity) => (offset * _strength) - (velocity * _damping);

        void OnDrawGizmos()
        {
            var collider = GetComponent<BoxCollider>();

            var cPos = transform.TransformPoint(collider.center + new Vector3(0, -collider.size.y, 0) / 2);
            var tlPos = transform.TransformPoint(collider.center + new Vector3(-collider.size.x, -collider.size.y, collider.size.z) / 2);
            var trPos = transform.TransformPoint(collider.center + new Vector3(collider.size.x, -collider.size.y, collider.size.z) / 2);
            var blPos = transform.TransformPoint(collider.center + new Vector3(-collider.size.x, -collider.size.y, -collider.size.z) / 2);
            var brPos = transform.TransformPoint(collider.center + new Vector3(collider.size.x, -collider.size.y, -collider.size.z) / 2);

            var pos = new[] { cPos, tlPos, trPos, blPos, brPos };

            for (var i = 1; i < pos.Length; ++i)
            {
                Debug.DrawRay(pos[i], -transform.up * _groundCheckDist, Color.red);
            }
        }
    }
}

