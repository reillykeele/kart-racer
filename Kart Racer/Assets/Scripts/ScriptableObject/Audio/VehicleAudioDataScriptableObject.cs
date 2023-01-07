using Data.Audio;
using UnityEngine;

namespace ScriptableObject.Audio
{
    [CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio/Vehicle Audio Data Clip")]
    public class VehicleAudioDataScriptableObject : UnityEngine.ScriptableObject
    {
        public VehicleAudioData VehicleAudioData;
    }

    public static class VehicleAudioDataExtension
    {
        public static AudioSource Initialize(this AudioSource audioSource, VehicleAudioData audioData)
        {
            audioSource.clip = audioData.IdleAudioClip;
            audioSource.outputAudioMixerGroup = audioData.MixingGroup;
            audioSource.volume = audioData.Volume;
            audioSource.playOnAwake = false;
            audioSource.loop = true;

            return audioSource;
        }

        public static AudioSource CreateNewAudioSource(this VehicleAudioData audioData, GameObject gameObject)
        {
            return gameObject.AddComponent<AudioSource>().Initialize(audioData);
        }
    }
}