using System;
using UnityEngine;
using UnityEngine.Audio;
using Util.Audio;

namespace Data.Audio
{
    [Serializable]
    public class LoopingMusicAudioData
    {
        public AudioClip IntroAudioClip;
        public AudioClip LoopAudioClip;
        public AudioMixerGroup MixingGroup;

        [Range(0f, 1f)]
        public float Volume = 1f;

        public bool PlayOnAwake = false;
    }

    public static class LoopingMusicAudioDataExtension
    {
        public static AudioSource Initialize(this AudioSource audioSource, LoopingMusicAudioData audioData)
        {
            audioSource.clip = audioData.IntroAudioClip;
            audioSource.outputAudioMixerGroup = audioData.MixingGroup;
            audioSource.volume = audioData.Volume;
            audioSource.playOnAwake = audioData.PlayOnAwake;
            audioSource.loop = false;

            return audioSource;
        }

        public static AudioSource CreateNewAudioSource(this LoopingMusicAudioData audioData, GameObject gameObject)
        {
            return gameObject.AddComponent<AudioSource>().Initialize(audioData);
        }
    }
}