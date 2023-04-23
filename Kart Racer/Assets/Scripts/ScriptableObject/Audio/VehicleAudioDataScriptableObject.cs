using Data.Audio;
using UnityEngine;

namespace KartRacer.ScriptableObject.Audio
{
    [CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio/Vehicle Audio Data Clip")]
    public class VehicleAudioDataScriptableObject : UnityEngine.ScriptableObject
    {
        public VehicleAudioData VehicleAudioData;
    }
}