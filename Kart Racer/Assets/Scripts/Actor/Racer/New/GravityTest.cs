using System.Linq;
using KartRacer.Actor.Racer.Player;
using UnityEngine;
using Util.Attributes;

namespace KartRacer
{
    public class GravityTest : MonoBehaviour
    {
        private PlayerInputController _kartInput;
        private Rigidbody _rb;
        private BoxCollider _collider;

        [Header("Move")] 
        [SerializeField, Min(0f)] private float _turnSpeed = 1f;
        [SerializeField, Range(0f, 1f)] private float _linearDrag = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _gripiness = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _angularDrag = 1.0f;
        [SerializeField] private AnimationCurve _accelerationCurve;

        [SerializeField, Min(0f)] private float _topReverseSpeed = 5f;
        [SerializeField, Min(0f)] private float _reverseAccel = 1f;

        [SerializeField, Min(0f)] private float _driftMagnitude = 1f;
        [SerializeField, Min(0f)] private float _driftGrip = 0.01f;

        [Header("Suspension")]
        [SerializeField, Min(0f)] private float _groundCheckDist = 1f;
        [SerializeField, Min(0f)] private float _maxOffset = 0.4f;
        [SerializeField, Min(0f)] private float _strength = 10f;
        [SerializeField, Min(0f)] private float _damping = 10f;

        [Header("Debug")] 
        [SerializeField, ReadOnly] private Vector3 _velocity;
        [SerializeField, ReadOnly] private float _forwardSpeed;
        [SerializeField, ReadOnly] private float _rightSpeed;
        [SerializeField, ReadOnly] private float _torque;
        [SerializeField, ReadOnly] private Vector3 _groundNormal;
        [SerializeField, ReadOnly] private int _driftDirection;
        [SerializeField, ReadOnly] private bool _isDrifting;
        private float _accelCurveTopSpeed => _accelerationCurve.keys.Last().value;

        void Awake()
        {
            _kartInput = GetComponent<PlayerInputController>();
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
        }

        void FixedUpdate()
        {
            // return;

            var isGrounded = CheckGround();

            if (true)
            {
                // rotate towards ground normal
                var rotForward = transform.forward - _groundNormal * Vector3.Dot(transform.forward, _groundNormal);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(rotForward.normalized, _groundNormal),
                    8f * Time.fixedDeltaTime);

                _velocity = _rb.velocity;
                _forwardSpeed = Vector3.Dot(transform.forward, _velocity);
                _rightSpeed = Vector3.Dot(transform.right, _velocity);

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
                    if (_forwardSpeed > -_topReverseSpeed)
                    {
                        _rb.AddForce(_reverseAccel * transform.forward);
                    }
                }
                else
                {
                    // deceleration
                    var forwardDrag = -_forwardSpeed * _linearDrag;
                    _rb.AddForce(forwardDrag / Time.fixedDeltaTime * transform.forward);
                }


                if (_kartInput.IsDrifting)
                {
                    if (_isDrifting == false)
                    {
                        _driftDirection = _kartInput.Steering.x < 0.05f ? 1 : _kartInput.Steering.x > 0.05f ? -1 : 0;

                        _isDrifting = _driftDirection != 0;
                    }
                }
                else
                {
                    _driftDirection = 0;
                    _isDrifting = false;
                }

                // Apply horizonal movement
                if (_isDrifting)
                {
                    // // add horizonal force
                    _rb.AddForce(_driftMagnitude * _driftDirection * transform.right);

                    // calculate sliding motion 
                    var rightDrag = -_rightSpeed * _driftGrip;
                    _rb.AddForce(rightDrag / Time.fixedDeltaTime * transform.right);
                }
                else
                {
                    // calculate sliding motion 
                    var rightDrag = -_rightSpeed * _gripiness;
                    _rb.AddForce(rightDrag / Time.fixedDeltaTime * transform.right);
                }

                if (_kartInput.Steering.x != 0f /*&& Mathf.Abs(_forwardSpeed) >= 1.0f*/)
                {
                    var prevTorque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);
                    
                    var nextTorque = _kartInput.Steering.x * _turnSpeed;
                    
                    var deltaTorque = nextTorque - prevTorque;
                    
                    _rb.AddTorque(deltaTorque * transform.up);
                    // _rb.AddTorque(_kartInput.Steering.x * _turnSpeed * transform.up);
                }
                else
                {
                    _torque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);
                    var oppTorque = -_torque * _angularDrag;
                    if (oppTorque != 0f)
                        _rb.AddTorque(oppTorque / Time.fixedDeltaTime * transform.up);
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

            return _accelerationCurve.Evaluate(time + Time.fixedDeltaTime);
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
            groundRays[0].isHit = Physics.Raycast(cPos, -transform.up * 100f,
                out groundRays[0].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[1].isHit = Physics.Raycast(tlPos, -transform.up * _groundCheckDist,
                out groundRays[1].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[2].isHit = Physics.Raycast(trPos, -transform.up * _groundCheckDist,
                out groundRays[2].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[3].isHit = Physics.Raycast(blPos, -transform.up * _groundCheckDist,
                out groundRays[3].hitInfo, LayerMask.NameToLayer("Track"));
            groundRays[4].isHit = Physics.Raycast(brPos, -transform.up * _groundCheckDist,
                out groundRays[4].hitInfo, LayerMask.NameToLayer("Track"));

            if (groundRays[0].isHit)
            {
                _groundNormal = groundRays[0].hitInfo.normal;
            }

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

