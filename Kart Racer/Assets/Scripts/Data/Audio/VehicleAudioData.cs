using System;
using UnityEngine;
using UnityEngine.Audio;
using Util.Audio;

namespace Data.Audio
{
    [Serializable]
    public class VehicleAudioData
    {
        public AudioClip IdleAudioClip;
        public AudioClip LoopAudioClip;
        public AudioClip UpAudioClip;
        public AudioClip DownAudioClip;
        public AudioMixerGroup MixingGroup;

        [Range(0f, 1f)] public float Volume = 1f;
        [Range(0f, 1f)] public float SpatialBlend = 1f;
    }

    public static class VehicleAudioDataExtension
    {
        public static AudioSource Initialize(this AudioSource audioSource, VehicleAudioData audioData)
        {
            audioSource.clip = audioData.LoopAudioClip;
            audioSource.outputAudioMixerGroup = audioData.MixingGroup;
            audioSource.volume = audioData.Volume;
            audioSource.spatialBlend = audioData.SpatialBlend;
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.Set3DSoundSettings();

            return audioSource;
        }

        public static AudioSource CreateNewAudioSource(this VehicleAudioData audioData, GameObject gameObject)
        {
            return gameObject.AddComponent<AudioSource>().Initialize(audioData);
        }
    }
}