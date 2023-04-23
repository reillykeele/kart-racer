using Data.Audio;
using UnityEngine;

namespace KartRacer.ScriptableObject.Audio
{
    [CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio/Music Audio Data Clip")]
    public class MusicAudioDataScriptableObject : UnityEngine.ScriptableObject
    {
        public MusicAudioData MusicAudioData;
    }
}