using Data.Environment;
using UnityEngine;

namespace ScriptableObject.Track
{
    [CreateAssetMenu(fileName = "TrackSurfaceModifier", menuName = "ScriptableObjects/Track Surface Modifier", order = 2)]
    public class TrackSurfaceModifierScriptableObject : UnityEngine.ScriptableObject
    {
        public TrackSurfaceModifierData ModifierData;
    }
}
