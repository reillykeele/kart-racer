using Data.Racer;
using ScriptableObject.Racer;
using UnityEngine;
using Util.Coroutine;

namespace Actor.Racer
{
    [RequireComponent(typeof(Collider))]
    public abstract class RacerMovementController : MonoBehaviour
    {

        [SerializeField] private RacerMovementScriptableObject _racerMovementData;
        [HideInInspector] public RacerMovementData RacerMovement;

        protected Collider _collider;

        protected float _currSpeed;
        
        protected bool _isDrifting;
        protected int _driftLevel;

        protected bool _isBoosting;
        protected float _currBoostPower;

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
            _isBoosting = true;
            _currBoostPower = boostPower;

            StartCoroutine(CoroutineUtil.WaitForExecute(() => _isBoosting = false, RacerMovement.BoostDuration));
        }
    }
}
