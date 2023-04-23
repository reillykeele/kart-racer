using Data.Environment;
using UnityEngine;

namespace KartRacer.ScriptableObject.Track
{
    [CreateAssetMenu(fileName = "TrackSurfaceModifier", menuName = "ScriptableObjects/Track/Track Surface Modifier")]
    public class TrackSurfaceModifierScriptableObject : UnityEngine.ScriptableObject
    {
        public TrackSurfaceModifierData ModifierData;
    }
}
