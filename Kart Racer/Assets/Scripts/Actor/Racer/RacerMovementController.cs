using System.Collections;
using Data.Environment;
using Data.Racer;
using Environment.Track;
using Manager;
using ScriptableObject.Racer;
using UnityEngine;
using UnityEngine.Events;
using Util.Coroutine;
using Util.Helpers;

namespace Actor.Racer
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class RacerMovementController : MonoBehaviour
    {

        [SerializeField] private RacerMovementScriptableObject _racerMovementData;
        public RacerMovementData RacerMovement { get; private set; }

        // Components
        protected Rigidbody _rb;
        protected BoxCollider _collider;

        // Movement
        [SerializeField] public bool UseAutopilot = false;
        [SerializeField] public bool UseAverageGroundNormals = false;
        public float Steering { get; protected set; }
        public Vector3 ControllerForward { get; protected set; }
        public Vector3 ControllerRotation { get; protected set; }
        public float CurrSpeed { get; protected set; }

        // Drift
        public UnityEvent<bool> OnIsDriftingChangedEvent;
        protected bool _isDrifting;
        public bool IsDrifting
        {
            get => _isDrifting;
            protected set
            {
                _isDrifting = value;
                OnIsDriftingChangedEvent.Invoke(_isDrifting);
            }
        }
        public UnityEvent<int> OnDriftLevelChangedEvent;
        protected int _driftProgress;
        protected int _driftLevel;
        public int DriftLevel
        {
            get => _driftLevel;
            protected set
            {
                _driftLevel = value;
                OnDriftLevelChangedEvent.Invoke(_driftLevel);
            }
        }
        public int DriftDirection { get; protected set; }

        // Boost
        public UnityEvent<bool> OnIsBoostingChangedEvent;
        protected bool _isBoosting;
        public bool IsBoosting
        {
            get => _isBoosting;
            protected set
            {
                _isBoosting = value;
                OnIsBoostingChangedEvent.Invoke(_isBoosting);
            }
        }
        public float CurrBoostPower { get; protected set; }

        // Autopilot
        public float SteeringTolerance = 0.1f;
        protected RacerController _racerController;
        protected GameObject _target;
        protected GameObject _lookaheadTarget;

        protected virtual bool IsAccelerating => false;
        protected virtual bool IsBraking => false;

        public bool IsIdling => IsAccelerating == false && IsBraking == false && CurrSpeed.IsZero();
        // public bool 

        protected virtual void Awake()
        {
            _racerController = GetComponent<RacerController>();

            RacerMovement = _racerMovementData?.RacerMovement ?? new RacerMovementData();

            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();

            OnIsDriftingChangedEvent.AddListener(OnDriftChange);
        }

        protected virtual void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            if (UseAutopilot)
            {
                Autopilot();
                return;
            }
            
            var isGrounded = IsGrounded(out var groundedHitInfo, out var trackSurfaceModifier);

            UpdateCurrSpeed(trackSurfaceModifier);
            Steer(out var steerDirection, out var movement);
            ApplyGravity(isGrounded, groundedHitInfo, ref movement);
            MoveAndRotate(steerDirection, movement);
        }

        public void Autopilot()
        {
            CurrSpeed = RacerMovement.MaxSpeed * 0.8f;

            Vector3 targetDirection;

            var targetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached);
            var lookaheadTargetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached + 1);
            _target = targetCheckpoint.gameObject;
            _lookaheadTarget = lookaheadTargetCheckpoint.gameObject;

            // var points = new[] { transform.position, targetCheckpoint.transform.position, lookaheadTargetCheckpoint.transform.position };
            var points = new[] { transform.position, targetCheckpoint.transform.position, lookaheadTargetCheckpoint.transform.position };
            var smoothedPath = PathHelper.SmoothPath(points, 6);

            for (var i = 0; i < smoothedPath.Length; i++)
            {
                smoothedPath[i].y = transform.position.y;
                DebugDrawHelper.DrawBox(smoothedPath[i], new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity, Color.green);
            }

            targetDirection = smoothedPath[1] - transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();
            Debug.DrawRay(transform.position, 4 * targetDirection, Color.green);
            
            var forward = transform.forward;

            var angle = Vector3.Cross(transform.forward, targetDirection);
            var dir = Vector3.Dot(angle, Vector3.up);

            Steering = dir > SteeringTolerance ? 1f : dir < -SteeringTolerance ? -1f : dir;
            var steeringDir = Steering * RacerMovement.TurningSpeed;
            
            if (!CurrSpeed.IsZero())
                transform.Rotate(transform.up, steeringDir * Time.fixedDeltaTime);

            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var movement = forward * CurrSpeed * Time.fixedDeltaTime;

            // Apply gravity
            var pos = transform.position;
            if (IsGrounded(out var hitInfo, out _))
            {
                pos.y = hitInfo.point.y + 0.5f; // TODO: Change to some "dist to ground" var

                var groundNormal = hitInfo.normal;
                var forwardDirection = forward.normalized;
                
                // Project the forward & surface normal using the dot product
                // Set the rotation w/ relative forward and up axes
                var rotForward = forwardDirection - groundNormal * Vector3.Dot (forwardDirection, groundNormal);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation(rotForward.normalized, groundNormal), 
                    2f * Time.fixedDeltaTime);
            }
            else
                movement.y -= RacerMovement.GravitySpeed;

            // Move player
            // transform.Rotate(transform.up, steerDirection * Time.fixedDeltaTime);
            transform.position = pos + movement;
        }

        public virtual void UpdateCurrSpeed(TrackSurfaceModifierData trackSurfaceModifier)
        {
            var velocity = _rb.velocity;
            velocity.y = 0;
            CurrSpeed = velocity.magnitude * Mathf.Sign(CurrSpeed);
            CurrSpeed = CalculateSpeed(CurrSpeed, IsAccelerating, IsBraking, trackSurfaceModifier);
        }

        public virtual void Steer(out float steerDirection, out Vector3 movement)
        {
            steerDirection = 0f;
            movement = Vector3.zero;
        }

        public virtual void ApplyGravity(bool isGrounded, RaycastHit groundedHitInfo, ref Vector3 movement)
        {
            if (isGrounded)
            {
                var  cPos = transform.TransformPoint(_collider.center + new Vector3(0, -_collider.size.y,  0) / 2);
                var tlPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y,  _collider.size.z) / 2);
                var trPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x, -_collider.size.y,  _collider.size.z) / 2);
                var blPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y, -_collider.size.z) / 2);
                var brPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x, -_collider.size.y, -_collider.size.z) / 2);

                # region Debug Draw Gravity Rays
                Debug.DrawRay( cPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
                Debug.DrawRay(tlPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
                Debug.DrawRay(trPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
                Debug.DrawRay(blPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
                Debug.DrawRay(brPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
                // Debug.DrawRay(groundedHitInfo.point, groundedHitInfo.normal * (RacerMovement.GroundedDist + _collider.size.y / 2), Color.blue);
                #endregion

                Vector3 groundNormal = groundedHitInfo.normal;
                if (UseAverageGroundNormals)
                {

                    var groundRays = new (bool isHit, RaycastHit hitInfo)[5];
                    groundRays[0].isHit = Physics.Raycast(cPos, Vector3.down * RacerMovement.GroundCheckDist,
                        out groundRays[0].hitInfo, LayerMask.NameToLayer("Track"));
                    groundRays[1].isHit = Physics.Raycast(tlPos, Vector3.down * RacerMovement.GroundCheckDist,
                        out groundRays[1].hitInfo, LayerMask.NameToLayer("Track"));
                    groundRays[2].isHit = Physics.Raycast(trPos, Vector3.down * RacerMovement.GroundCheckDist,
                        out groundRays[2].hitInfo, LayerMask.NameToLayer("Track"));
                    groundRays[3].isHit = Physics.Raycast(blPos, Vector3.down * RacerMovement.GroundCheckDist,
                        out groundRays[3].hitInfo, LayerMask.NameToLayer("Track"));
                    groundRays[4].isHit = Physics.Raycast(brPos, Vector3.down * RacerMovement.GroundCheckDist,
                        out groundRays[4].hitInfo, LayerMask.NameToLayer("Track"));

                    var averageGroundNormal = Vector3.zero;
                    var numGroundNormals = 0;
                    for (var i = 0; i < groundRays.Length; ++i)
                    {
                        if (groundRays[i].isHit)
                        {
                            numGroundNormals++;
                            averageGroundNormal += groundRays[i].hitInfo.normal;
                        }
                    }

                    groundNormal = (averageGroundNormal / numGroundNormals).normalized;
                }

                var pos = transform.position;
                pos.y = groundedHitInfo.point.y + RacerMovement.GroundedDist + (pos.y - cPos.y);
                transform.position = pos;

                // Project the forward & surface normal using the dot product
                // Set the rotation w/ relative forward and up axes
                // var groundNormal = groundedHitInfo.normal;
                var rotForward = transform.forward - groundNormal * Vector3.Dot( transform.forward, groundNormal);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(rotForward.normalized, groundNormal),
                    8f * Time.fixedDeltaTime);
            }
            else
            {
                movement.y -= RacerMovement.GravitySpeed;
            }
        }

        public virtual void MoveAndRotate(float steerDirection, Vector3 movement)
        {
            if (CurrSpeed.IsZero() == false)
                transform.Rotate(transform.up, steerDirection * Time.fixedDeltaTime);

            var newVelocity = 50f * movement * Time.fixedDeltaTime;
            _rb.velocity = newVelocity;
        }

        public bool IsGrounded() => IsGrounded(out _, out _);
        public bool IsGrounded(out RaycastHit hitInfo, out TrackSurfaceModifierData trackSurfaceModifier)
        {
            var colliderCenter = transform.TransformPoint(_collider.center);
            Debug.DrawRay(colliderCenter, Vector3.down * (_collider.size.y / 2 + RacerMovement.GroundCheckDist), Color.red);
            var isGrounded = Physics.Raycast(
                colliderCenter, 
                Vector3.down, 
                out hitInfo,
                _collider.size.y / 2 + RacerMovement.GroundCheckDist,
                LayerMask.GetMask("Track"));

            trackSurfaceModifier = isGrounded ? hitInfo.collider.GetComponent<TrackSurface>()?.TrackSurfaceModifierData : new TrackSurfaceModifierData();

            return isGrounded;
        }

        public float CalculateSpeed(float prevSpeed, bool isAccelerating, bool isBraking, TrackSurfaceModifierData trackSurfaceModifier)
        {
            var newSpeed = prevSpeed;

            // Handle acceleration and deceleration 
            if (IsBoosting)
            {
                // Boosting
                newSpeed = RacerMovement.MaxSpeed + RacerMovement.MaxSpeed * CurrBoostPower;
            }

            if (isAccelerating == isBraking || newSpeed >= RacerMovement.MaxSpeed)
            {
                // Coasting
                var newSpeed2 = newSpeed + (newSpeed > 0 ? -1 : 1) * RacerMovement.DecelerationSpeed * trackSurfaceModifier.DeccelerationModifier;
                newSpeed = newSpeed.IsZero() || newSpeed * newSpeed2 < 0f ? 0f : newSpeed2;
            }
            else if (isAccelerating)
            {
                // Acceleration
                newSpeed = newSpeed >= RacerMovement.MaxSpeed * trackSurfaceModifier.SpeedModifier
                    ? RacerMovement.MaxSpeed * trackSurfaceModifier.SpeedModifier
                    : newSpeed + RacerMovement.AccelerationSpeed * trackSurfaceModifier.AccelerationModifier;
            }
            else if (isBraking)
            {
                // Braking or reversing
                if (newSpeed > 0)
                    newSpeed = newSpeed <= 0 ? 0 : newSpeed - RacerMovement.BrakeSpeed;
                else
                    newSpeed = newSpeed <= -RacerMovement.MaxReverseSpeed
                        ? -RacerMovement.MaxReverseSpeed
                        : newSpeed - RacerMovement.ReverseAccelerationSpeed;
            }

            return newSpeed;
        }

        private IEnumerator _boostCoroutine;
        public void Boost(float boostPower = 1f, float boostDuration = 1f)
        {
            IsBoosting = true;
            CurrBoostPower = Mathf.Max(boostPower, CurrBoostPower);

            if (_boostCoroutine != null)
                StopCoroutine(_boostCoroutine);

            _boostCoroutine = CoroutineUtil.WaitForExecute(() =>
                {
                    IsBoosting = false;
                    _boostCoroutine = null;
                }, 
                boostDuration);

            StartCoroutine(_boostCoroutine);
        }

        protected  virtual void OnDriftChange(bool isDrifting)
        {
            if (isDrifting == false && DriftLevel > 0)
                Boost(0.5f, 1f + 0.25f * (DriftLevel - 1));

            _driftProgress = 0;
            DriftLevel = 0;
        }
    }
}
