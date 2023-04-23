using Data.Audio;
using UnityEngine;

namespace KartRacer.ScriptableObject.Audio
{
    [CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio/Audio Data Clip")]
    public class AudioDataScriptableObject : UnityEngine.ScriptableObject
    {
        public AudioData AudioData;
    }
}