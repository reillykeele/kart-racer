using System.Collections;
using System.Linq;
using Data.Racer;
using Environment.Scene;
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

        protected BoxCollider _collider;

        // Movement
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

        protected virtual void Awake()
        {
            _racerController = GetComponent<RacerController>();

            RacerMovement = _racerMovementData?.RacerMovement ?? new RacerMovementData();

            _collider = GetComponent<BoxCollider>();

            OnIsDriftingChangedEvent.AddListener(OnDriftChange);
        }

        protected virtual void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            CurrSpeed = RacerMovement.MaxSpeed;

            Vector3 targetDirection = Vector3.zero;

            var targetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached);
            var lookaheadTargetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached + 1);
            _target = targetCheckpoint.gameObject;
            _lookaheadTarget = lookaheadTargetCheckpoint.gameObject;
            
            var distToTarget = (targetCheckpoint.transform.position - transform.position).magnitude;
            var targetTolookahead = (lookaheadTargetCheckpoint.transform.position - targetCheckpoint.transform.position);

            if (distToTarget <= targetTolookahead.magnitude)
            { 
                // var points = new[] { transform.position, targetCheckpoint.transform.position, lookaheadTargetCheckpoint.transform.position };
                var points = new[] { transform.position, targetCheckpoint.Tight, lookaheadTargetCheckpoint.Tight };
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
            }
            else
            {
                targetDirection = targetCheckpoint.transform.position - transform.position;
                targetDirection.y = 0;
                targetDirection.Normalize();
                Debug.DrawRay(transform.position, 3 * targetDirection, Color.red);
            }

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
            if (IsGrounded(out var hitInfo))
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

        public bool IsGrounded() => IsGrounded(out _);
        public bool IsGrounded(out RaycastHit hitInfo)
        {
            var dist = 0.15f;
            Debug.DrawRay(_collider.bounds.center, Vector3.down * (_collider.bounds.extents.y/* + dist*/), Color.red);
            var hit = Physics.Raycast(
                _collider.bounds.center, 
                Vector3.down, 
                out hitInfo,
                _collider.bounds.extents.y + dist,
                LayerMask.GetMask("Track"));

            return hit;
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
