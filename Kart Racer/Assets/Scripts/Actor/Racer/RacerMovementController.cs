using Data.Racer;
using Effect.Particle;
using ScriptableObject.Racer;
using UnityEngine;
using UnityEngine.Events;
using Util.Coroutine;

namespace Actor.Racer
{
    [RequireComponent(typeof(Collider))]
    public abstract class RacerMovementController : MonoBehaviour
    {

        [SerializeField] private RacerMovementScriptableObject _racerMovementData;
        public RacerMovementData RacerMovement { get; private set; }

        protected Collider _collider;

        public float CurrSpeed { get; protected set; }

        // Drift
        public UnityEvent<bool> OnIsDriftingChangedEvent;
        private bool _isDrifting;
        public bool IsDrifting
        {
            get => _isDrifting;
            protected set
            {
                _isDrifting = value;
                OnIsBoostingChangedEvent.Invoke(_isDrifting);
            }
        }
        public int DriftLevel { get; protected set; }

        // Boost
        public UnityEvent<bool> OnIsBoostingChangedEvent;
        private bool _isBoosting;
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

        protected virtual void Awake()
        {
            RacerMovement = _racerMovementData?.RacerMovement ?? new RacerMovementData();

            _collider = GetComponent<Collider>();
        }

        public bool IsGrounded()
        {
            Debug.DrawRay(_collider.bounds.center, Vector3.down * (_collider.bounds.extents.y + 0.05f));
            return Physics.Raycast(_collider.bounds.center, Vector3.down, _collider.bounds.extents.y + 0.05f);
        }

        public void Boost(float boostPower = 1f, float boostDuration = 1f)
        {
            IsBoosting = true;
            CurrBoostPower = boostPower;

            StartCoroutine(CoroutineUtil.WaitForExecute(() => IsBoosting = false, RacerMovement.BoostDuration));
        }
    }
}
