using Data.Audio;
using KartRacer.ScriptableObject.Audio;
using UnityEngine;
using Util.Systems;

namespace KartRacer.Actor.Racer
{
    [RequireComponent(typeof(RacerMovementController))]
    public class RacerAudioController : MonoBehaviour
    {
        [Header("Vehicle Sound Effects")] 
        [SerializeField] private VehicleAudioDataScriptableObject _vehicleAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _boostAudioDataScriptableObject;
        [SerializeField] private LoopingMusicAudioDataScriptableObject _driftAudioDataScriptableObject;
        // [SerializeField] private AudioDataScriptableObject _miniTurboChangeAudioDataScriptableObject;

        [Header("Volume Properties")] 
        [SerializeField] [Range(0f, 1.0f)] private float _engineRunningMinVolume;
        [SerializeField] [Range(0f, 1.0f)] private float _engineRunningMaxVolume;
        [SerializeField] [Range(0f, 1.0f)] private float _engineRunningBoostingMaxVolume;
        [SerializeField] [Range(0f, 1.0f)] private float _engineRunningReverseMaxVolume;

        [Header("Pitch Properties")] 
        [SerializeField] [Range(0f, 2.0f)] private float _engineRunningMinPitch;
        [SerializeField] [Range(0f, 2.0f)] private float _engineRunningMaxPitch;
        [SerializeField] [Range(0f, 2.0f)] private float _engineRunningBoostingMaxPitch;
        [SerializeField] [Range(0f, 2.0f)] private float _engineRunningReverseMaxPitch;

        // Vehicle
        private AudioSource _vehicleAudioSource;
        private AudioSource _boostAudioSource;
        private AudioSource _driftAudioSource;
        private AudioSource _miniTurboChangeAudioSource;

        private RacerMovementController _racerMovementController;

        private VehicleAudioClipType _vehicleAudioClipType = VehicleAudioClipType.None;

        void Start()
        {
            _racerMovementController = GetComponent<RacerMovementController>();

            _vehicleAudioSource = _vehicleAudioDataScriptableObject.VehicleAudioData.CreateNewAudioSource(gameObject);
            _boostAudioSource = _boostAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
            _driftAudioSource = _driftAudioDataScriptableObject.LoopingMusicAudioData.CreateNewAudioSource(gameObject);
            // _miniTurboChangeAudioSource = _miniTurboChangeAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);

            ChangeVehicleAudioClip(VehicleAudioClipType.Loop);

            // Set Events
            GameSystem.Instance.OnPauseGameEvent.AddListener(PauseAudio);
            GameSystem.Instance.OnResumeGameEvent.AddListener(ResumeAudio);
            _racerMovementController.OnIsBoostingChangedEvent.AddListener(PlayBoostSoundEffect);
            _racerMovementController.OnIsDriftingChangedEvent.AddListener(PlayDriftSoundEffect);
        }

        void Update()
        {
            if (GameSystem.Instance.IsPaused()) return;

            if (_racerMovementController.CurrSpeed < 0.0f)
            {
                var speedPercentage = _racerMovementController.CurrSpeed / -_racerMovementController.RacerMovement.MaxReverseSpeed;
                
                _vehicleAudioSource.volume = 0.0f;
                _vehicleAudioSource.volume = Mathf.Lerp(_engineRunningMinVolume, _engineRunningReverseMaxVolume, speedPercentage);
                _vehicleAudioSource.pitch = Mathf.Lerp(_engineRunningMinPitch, _engineRunningReverseMaxPitch, speedPercentage + (Mathf.Sin(Time.time) * .1f));
            }
            else if (_racerMovementController.CurrSpeed > (_racerMovementController.RacerMovement.MaxSpeed * 1.05f))
            {
                var speedPercentage =
                    (_racerMovementController.CurrSpeed - _racerMovementController.RacerMovement.MaxSpeed)  / 
                    (_racerMovementController.RacerMovement.MaxSpeed * _racerMovementController.CurrBoostPower);
                
                _vehicleAudioSource.volume = 0.0f;
                _vehicleAudioSource.volume = Mathf.Lerp(_engineRunningMaxVolume, _engineRunningBoostingMaxVolume, speedPercentage);
                _vehicleAudioSource.pitch = Mathf.Lerp(_engineRunningMaxPitch, _engineRunningBoostingMaxPitch, speedPercentage + (Mathf.Sin(Time.time) * .1f));
            }
            else
            {
                var speedPercentage = _racerMovementController.CurrSpeed / _racerMovementController.RacerMovement.MaxSpeed;
                
                _vehicleAudioSource.volume = 0.0f;
                _vehicleAudioSource.volume = Mathf.Lerp(_engineRunningMinVolume, _engineRunningMaxVolume, speedPercentage);
                _vehicleAudioSource.pitch = Mathf.Lerp(_engineRunningMinPitch, _engineRunningMaxPitch, speedPercentage + (Mathf.Sin(Time.time) * .1f));
            }
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
            // _vehicleAudioSource.volume = 1.0f;
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

        public void PlayBoostSoundEffect(bool isBoosting) { if (isBoosting) PlayBoostSoundEffect(); /*else _boostAudioSource.Stop();*/ }
        public void PlayBoostSoundEffect() => _boostAudioSource.Play();

        public void PlayDriftSoundEffect(bool isDrifting) { if (isDrifting) PlayDriftSoundEffect(); else _driftAudioSource.Stop(); }

        public void PlayDriftSoundEffect()
        {
            // StartCoroutine(AudioHelper.PlayLoopingAudioData(_driftAudioSource,
            //     _driftAudioDataScriptableObject.LoopingMusicAudioData));
        }

        public void PlayMiniTurboChangeSoundEffect(int level) => _miniTurboChangeAudioSource.Play();

        public void PauseAudio()
        {
            _vehicleAudioSource.Pause();
            _boostAudioSource.Pause();
            _driftAudioSource.Pause();
        }

        public void ResumeAudio()
        {
            _vehicleAudioSource.UnPause();
            _boostAudioSource.UnPause();
            _driftAudioSource.UnPause();
        }
    }
}
