using KartRacer.Input;
using UnityEngine;
using Util.Attributes;
using Util.Systems;

namespace KartRacer.Actor.Racer.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        // Player State Values
        [SerializeField, ReadOnly] public Vector2 Steering;
        [SerializeField, ReadOnly] public bool IsAccelerating;
        [SerializeField, ReadOnly] public bool IsBraking;
        [SerializeField, ReadOnly] public bool IsDrifting;
        [SerializeField, ReadOnly] public bool IsUsingItem;
        [SerializeField, ReadOnly] public bool IsLookingBehind;

        void OnEnable()
        {
            _inputReader.SteerEvent += OnSteer;
            _inputReader.AccelerateEvent += OnAccelerateStarted;
            _inputReader.AccelerateCancelledEvent += OnAccelerateCancelled;
            _inputReader.BrakeEvent += OnBrakeStarted;
            _inputReader.BrakeCancelledEvent += OnBrakeCancelled;
            _inputReader.DriftEvent += OnDriftStarted;
            _inputReader.DriftCancelledEvent += OnDriftCancelled;
            _inputReader.ItemEvent += OnItemStarted;
            _inputReader.ItemCancelledEvent += OnItemCancelled;
            _inputReader.RearCameraEvent += OnRearCameraStarted;
            _inputReader.RearCameraCancelledEvent += OnRearCameraCancelled;
        }

        void OnDisable()
        {
            _inputReader.SteerEvent -= OnSteer;
            _inputReader.AccelerateEvent -= OnAccelerateStarted;
            _inputReader.AccelerateCancelledEvent -= OnAccelerateCancelled;
            _inputReader.BrakeEvent -= OnBrakeStarted;
            _inputReader.BrakeCancelledEvent -= OnBrakeCancelled;
            _inputReader.DriftEvent -= OnDriftStarted;
            _inputReader.DriftCancelledEvent -= OnDriftCancelled;
            _inputReader.ItemEvent -= OnItemStarted;
            _inputReader.ItemCancelledEvent -= OnItemCancelled;
            _inputReader.RearCameraEvent -= OnRearCameraStarted;
            _inputReader.RearCameraCancelledEvent -= OnRearCameraCancelled;
        }

        public void OnSteer(Vector2 val) => Steering = val;

        public void OnAccelerateStarted() => IsAccelerating = true;
        public void OnAccelerateCancelled() => IsAccelerating = false;

        public void OnBrakeStarted() => IsBraking = true;
        public void OnBrakeCancelled() => IsBraking = false;

        public void OnDriftStarted() => IsDrifting = true;
        public void OnDriftCancelled() => IsDrifting = false;

        public void OnItemStarted() => IsUsingItem = true;   
        public void OnItemCancelled() => IsUsingItem = false;

        public void OnRearCameraStarted() => IsLookingBehind = true;
        public void OnRearCameraCancelled() => IsLookingBehind = false;

        public void OnPause() => GameSystem.Instance.PauseGame();
    }
}
