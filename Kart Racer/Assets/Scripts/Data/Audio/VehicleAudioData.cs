using System;
using UnityEngine;
using UnityEngine.Audio;

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

        [Range(0f, 1f)]
        public float Volume = 1f;
    }
}