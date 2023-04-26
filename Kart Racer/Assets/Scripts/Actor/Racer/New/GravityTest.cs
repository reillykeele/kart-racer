using System;
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

        [Header("Settings")]
        [SerializeField] private bool _rotateToGroundNormal;
        [SerializeField] private bool _useLinearDrag;
        [SerializeField] private bool _useGrip;
        [SerializeField] private bool _useAngularDrag;

        [Header("Move")] 
        [SerializeField, Min(0f)] private float _turnSpeed = 1f;

        [SerializeField, Range(0f, 1f)] private float _linearDrag = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _gripiness = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _angularDrag = 1.0f;
        
        [SerializeField] private AnimationCurve _accelerationCurve;

        [SerializeField, Min(0f)] private float _topReverseSpeed = 5f;
        [SerializeField, Min(0f)] private float _reverseAccel = 1f;

        [Header("Drift")]
        [SerializeField, Min(0f)] private float _minSpeedToDrift = 0f;
        [SerializeField, Min(0f)] private float _driftMagnitude = 1f;
        [SerializeField, Min(0f)] private float _driftGrip = 0.01f;
        [SerializeField, Min(0f)] private float _driftAngularDrag = 0.01f;
        [SerializeField, Min(0f)] private float _driftNeutralTurnMultiplier = 1.0f;
        [SerializeField, Min(0f)] private float _driftSteerTurnMultiplier = 1.0f;

        [Header("Suspension")] 
        [SerializeField] private float _suspensionOffsetX = 0f;
        [SerializeField] private float _suspensionOffsetY = 0f;
        [SerializeField] private float _suspensionOffsetZ = 0f;

        [SerializeField, Min(0f)] private float _groundCheckDist = 1f;
        [SerializeField, Min(0f)] private float _suspensionHeight = 1f;
        [SerializeField, Min(0f)] private float _maxOffset = 0.4f;
        [SerializeField, Min(0f)] private float _strength = 10f;
        [SerializeField, Min(0f)] private float _damping = 10f;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isGrounded;
        [SerializeField, ReadOnly] private Vector3 _groundNormal;
        [SerializeField, ReadOnly] private Vector3 _velocity;
        [SerializeField, ReadOnly] private float _forwardSpeed;
        [SerializeField, ReadOnly] private float _rightSpeed;
        [SerializeField, ReadOnly] private float _torque;
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
            _isGrounded = CheckGround();

            _velocity = _rb.velocity;
            _forwardSpeed = Vector3.Dot(transform.forward, _velocity);
            _rightSpeed = Vector3.Dot(transform.right, _velocity);

            // Determine if we are drifting
            if (_kartInput.IsDrifting && _forwardSpeed >= _minSpeedToDrift)
            {
                if (_isDrifting == false)
                {
                    _driftDirection = _kartInput.Steering.x < -0.05f ? 1 : _kartInput.Steering.x > 0.05f ? -1 : 0;
                    _isDrifting = _driftDirection != 0;
                }
            }
            else
            {
                _driftDirection = 0;
                _isDrifting = false;
            }

            // rotate towards ground normal
            if (_rotateToGroundNormal && _groundNormal != Vector3.zero)
            {
                var rotForward = transform.forward - _groundNormal * Vector3.Dot(transform.forward, _groundNormal);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.LookRotation(rotForward.normalized, _groundNormal),
                    90f);
            }

            // Movement logic
            if (_isDrifting)
                Drift();
            else
                Drive();
        }

        private void Drive()
        {
            // calculate forward motion
            if (_isGrounded)
            {
                if (_kartInput.IsAccelerating)
                {
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
                        _rb.AddForce(-_reverseAccel * transform.forward);
                    }
                }
                else
                {
                    // deceleration
                    if (_useLinearDrag)
                    {
                        var forwardDrag = -_forwardSpeed * _linearDrag;
                        _rb.AddForce(forwardDrag / Time.fixedDeltaTime * transform.forward);
                    }
                }
            }

            // calculate sliding motion 
            if (_useGrip)
            {
                var rightDrag = -_rightSpeed * _gripiness;
                _rb.AddForce(rightDrag / Time.fixedDeltaTime * transform.right);
            }

            // calculate rotational force
            if (_isGrounded && _kartInput.Steering.x != 0f && Mathf.Abs(_forwardSpeed) >= 1.0f)
            {
                var prevTorque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);

                var nextTorque = _kartInput.Steering.x * _turnSpeed;

                var deltaTorque = nextTorque - prevTorque;

                _rb.AddTorque(deltaTorque * transform.up);
            }
            else if (_useAngularDrag)
            {
                _torque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);
                var oppTorque = -_torque * _angularDrag;
                if (oppTorque != 0f)
                    _rb.AddTorque(oppTorque / Time.fixedDeltaTime * transform.up);
            }
        }

        private void Drift()
        {
            // calculate forward motion
            if (_isGrounded)
            {
                if (_kartInput.IsAccelerating)
                {
                    // DRIFTING
                    var nextSpeed = GetNextSpeed(_forwardSpeed);

                    var deltaSpeed = nextSpeed - _forwardSpeed;

                    _rb.AddForce(deltaSpeed * transform.forward +
                                    transform.right * _driftMagnitude * _driftDirection);
                }
                else if (_kartInput.IsBraking)
                {
                    // TODO: ?
                    if (_forwardSpeed > -_topReverseSpeed)
                    {
                        _rb.AddForce(-_reverseAccel * transform.forward);
                    }
                }
                else
                {
                    // deceleration
                    if (_useLinearDrag)
                    {
                        var forwardDrag = -_forwardSpeed * _linearDrag;
                        _rb.AddForce(forwardDrag / Time.fixedDeltaTime * transform.forward);
                    }
                }
            }
            
            // add horizontal drifting force
            _rb.AddForce(_driftMagnitude * _driftDirection * transform.right);

            // calculate sliding motion 
            var rightDrag = -_rightSpeed * _driftGrip;
            _rb.AddForce(rightDrag / Time.fixedDeltaTime * transform.right);


            // if (_isGrounded && _kartInput.Steering.x != 0f && Mathf.Abs(_forwardSpeed) >= 1.0f)
            // {
                var prevTorque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);

                // We want to drift inwards
                var nextTorque =
                    -_driftDirection * (_turnSpeed * _driftNeutralTurnMultiplier) +
                    _kartInput.Steering.x * (_turnSpeed * _driftSteerTurnMultiplier);

                var deltaTorque = nextTorque - prevTorque;

                _rb.AddTorque(deltaTorque * transform.up);
            
                if (_useAngularDrag)
            {
                _torque = Vector3.Dot(_rb.angularVelocity, _rb.inertiaTensor);
                var oppTorque = -_torque * _driftAngularDrag;
                if (oppTorque != 0f)
                    _rb.AddTorque(oppTorque / Time.fixedDeltaTime * transform.up);
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
            var cPos  = transform.TransformPoint(_collider.center + new Vector3(0, -_collider.size.y, 0) / 2);
            var flPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x + _suspensionOffsetX, -_collider.size.y + _suspensionOffsetY,  _collider.size.z - _suspensionOffsetZ) / 2);
            var frPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x - _suspensionOffsetX, -_collider.size.y + _suspensionOffsetY,  _collider.size.z - _suspensionOffsetZ) / 2);
            var blPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x + _suspensionOffsetX, -_collider.size.y + _suspensionOffsetY, -_collider.size.z + _suspensionOffsetZ) / 2);
            var brPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x - _suspensionOffsetX, -_collider.size.y + _suspensionOffsetY, -_collider.size.z + _suspensionOffsetZ) / 2);

            var pos = new[] { cPos, flPos, frPos, blPos, brPos };

            var groundRays = new (bool isHit, RaycastHit hitInfo)[5];
            groundRays[0].isHit = Physics.Raycast(cPos,  -transform.up, out groundRays[0].hitInfo, _groundCheckDist,  LayerMask.GetMask("Track"));
            groundRays[1].isHit = Physics.Raycast(flPos, -transform.up, out groundRays[1].hitInfo, _suspensionHeight, LayerMask.GetMask("Track"));
            groundRays[2].isHit = Physics.Raycast(frPos, -transform.up, out groundRays[2].hitInfo, _suspensionHeight, LayerMask.GetMask("Track"));
            groundRays[3].isHit = Physics.Raycast(blPos, -transform.up, out groundRays[3].hitInfo, _suspensionHeight, LayerMask.GetMask("Track"));
            groundRays[4].isHit = Physics.Raycast(brPos, -transform.up, out groundRays[4].hitInfo, _suspensionHeight, LayerMask.GetMask("Track"));

            if (groundRays[0].isHit)
            {
                DebugDrawHelper.DrawSphere(groundRays[0].hitInfo.point, 0.1f, Color.green, quality: 1);
                _groundNormal = groundRays[0].hitInfo.normal;
            }
            else _groundNormal = Vector3.up;

            var isGrounded = false;
            for (var i = 1; i < groundRays.Length; ++i)
            {
                if (groundRays[i].isHit)
                {
                    DebugDrawHelper.DrawSphere(groundRays[i].hitInfo.point, 0.1f, Color.green, quality:1);

                    var distFromGround = _suspensionHeight - groundRays[i].hitInfo.distance;
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
            var flPos = transform.TransformPoint(collider.center + new Vector3(-collider.size.x + _suspensionOffsetX, -collider.size.y + _suspensionOffsetY, collider.size.z - _suspensionOffsetZ) / 2);
            var frPos = transform.TransformPoint(collider.center + new Vector3(collider.size.x - _suspensionOffsetX, -collider.size.y + _suspensionOffsetY, collider.size.z - _suspensionOffsetZ) / 2);
            var blPos = transform.TransformPoint(collider.center + new Vector3(-collider.size.x + _suspensionOffsetX, -collider.size.y + _suspensionOffsetY, -collider.size.z + _suspensionOffsetZ) / 2);
            var brPos = transform.TransformPoint(collider.center + new Vector3(collider.size.x - _suspensionOffsetX, -collider.size.y + _suspensionOffsetY, -collider.size.z + _suspensionOffsetZ) / 2);

            var pos = new[] { cPos, flPos, frPos, blPos, brPos };

            // draw ground check ray
            Debug.DrawRay(pos[0], -transform.up * _groundCheckDist, Color.green);

            // draw suspension
            for (var i = 1; i < pos.Length; ++i)
            {
                Debug.DrawRay(pos[i], -transform.up * _suspensionHeight, Color.red);
            }
        }
    }
}

