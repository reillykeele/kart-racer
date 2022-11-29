using System.Collections.Generic;
using System.Linq;
using Data.Audio;
using Manager;
using ScriptableObject.Audio;
using TMPro;
using UnityEngine;

namespace Environment.Scene
{
    public class CourseAudioController : MonoBehaviour
    {
        private CourseController _courseController;

        [Header("Music")]
        [SerializeField] private AudioDataScriptableObject _musicAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _musicFinalLapAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _musicFinishedRaceAudioDataScriptableObject;

        [Header("Sound Effects")]
        [SerializeField] private AudioDataScriptableObject _countdownAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _goAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _lapAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _finalLapAudioDataScriptableObject;
        [SerializeField] private AudioDataScriptableObject _finishRaceAudioDataScriptableObject;

        // Music
        private AudioSource _musicAudioSource;

        // Effects
        private AudioSource _countdownAudioSource;
        private AudioSource _goAudioSource;
        private AudioSource _lapAudioSource;
        private AudioSource _finalLapAudioSource;
        private AudioSource _finishRaceAudioSource;


        void Awake()
        {
            _courseController = GetComponent<CourseController>();

            _musicAudioSource = _musicAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
            _countdownAudioSource = _countdownAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
            _goAudioSource = _goAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
            _lapAudioSource = _lapAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
            _finalLapAudioSource = _finalLapAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
            _finishRaceAudioSource = _finishRaceAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
        }

        void Start()
        {
            var raceManager = GameManager.Instance.RaceManager;

            raceManager.CountdownTickEvent.AddListener(PlayCountdownEffect);
            raceManager.CountdownEndEvent.AddListener(PlayGoEffect);

            foreach (var racer in raceManager.PlayerRacers)
            {
                racer.ChangeLapEvent.AddListener(PlayAdvanceLapEffect);
                racer.FinishRaceEvent.AddListener(PlayFinishRaceEffect);
            }
        }

        void PlayNormalMusic(float delay = 0f) => ChangeMusic(_musicAudioDataScriptableObject.AudioData, delay);
        void PlayFinalLapMusic(float delay = 0f)=> ChangeMusic(_musicFinalLapAudioDataScriptableObject.AudioData, delay);
        void PlayFinishedRaceMusic(float delay = 0f)=> ChangeMusic(_musicFinishedRaceAudioDataScriptableObject.AudioData, delay);

        void ChangeMusic(AudioData audioData, float delay = 0f)
        {
            StopMusic();
                
            _musicAudioSource.clip = audioData.AudioClip;
            
            if (delay != 0f)
                PlayMusicDelayed(delay);
            else 
                PlayMusic();
        }

        void PlayMusic() => _musicAudioSource.Play();
        void PlayMusicDelayed(float delay) => _musicAudioSource.PlayDelayed(delay);
        void PauseMusic() => _musicAudioSource.Pause();
        void ResumeMusic() => _musicAudioSource.UnPause();
        void StopMusic() => _musicAudioSource.Stop();

        void PlayCountdownEffect(int _) => PlayCountdownEffect();
        void PlayCountdownEffect() => _countdownAudioSource.Play();

        void PlayGoEffect()
        {
            _goAudioSource.Play();
            PlayNormalMusic(_goAudioSource.clip.length / 2);
        }

        void PlayAdvanceLapEffect(int lap)
        {
            if (lap < _courseController.Laps)
                PlayAdvanceLapEffect();
            else
                PlayAdvanceFinalLapEffect();
        }

        void PlayAdvanceLapEffect()
        {
            // PauseMusic();
            _lapAudioSource.Play();

        }

        void PlayAdvanceFinalLapEffect()
        {
            StopMusic();
            _finalLapAudioSource.Play();
            PlayFinalLapMusic(_finalLapAudioSource.clip.length);
        }

        void PlayFinishRaceEffect(float _) => PlayFinishRaceEffect();
        void PlayFinishRaceEffect()
        {
            StopMusic();
            _finishRaceAudioSource.Play();
            PlayFinishedRaceMusic(_finishRaceAudioSource.clip.length * 0.75f);
        }
    }
}
