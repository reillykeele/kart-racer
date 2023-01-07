using System;
using ScriptableObject.Audio;
using UnityEngine;

namespace Actor.Racer
{
    [RequireComponent(typeof(RacerMovementController))]
    public class RacerAudioController : MonoBehaviour
    {
        [Header("Vehicle Sound Effects")] 
        [SerializeField] private VehicleAudioDataScriptableObject _vehicleAudioDataScriptableObject;

        // [Header("Sound Effects")]
        // [SerializeField] private AudioDataScriptableObject _itemBlockAudioDataScriptableObject;

        // Vehicle
        private AudioSource _vehicleAudioSource;

        private RacerMovementController _racerMovementController;

        private VehicleAudioClipType _vehicleAudioClipType = VehicleAudioClipType.None;

        void Start()
        {
            _racerMovementController = GetComponent<RacerMovementController>();

            _vehicleAudioSource = _vehicleAudioDataScriptableObject.VehicleAudioData.CreateNewAudioSource(gameObject);
            // _vehicleAudioSource.Play();
        }

        void Update()
        {
            if (_racerMovementController.IsIdling)
                ChangeVehicleAudioClip(VehicleAudioClipType.Idle);
            else
                ChangeVehicleAudioClip(VehicleAudioClipType.Loop);
        }

        private void ChangeVehicleAudioClip(VehicleAudioClipType clipType, bool loop = true)
        {
            if (clipType == _vehicleAudioClipType) return;

            var clip = clipType switch
            {
                VehicleAudioClipType.Idle => _vehicleAudioDataScriptableObject.VehicleAudioData.IdleAudioClip,
                VehicleAudioClipType.Loop => _vehicleAudioDataScriptableObject.VehicleAudioData.LoopAudioClip,
                _ => null
            };

            _vehicleAudioClipType = clipType;
            _vehicleAudioSource.clip = clip;
            _vehicleAudioSource.loop = loop;
            _vehicleAudioSource.Play();
        }

        internal enum VehicleAudioClipType
        {
            None,
            Idle,
            Loop
        }
    }
}
