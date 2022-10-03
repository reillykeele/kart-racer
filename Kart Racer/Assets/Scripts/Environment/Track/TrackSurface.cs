using Data.Environment;
using ScriptableObject.Track;
using UnityEngine;

namespace Environment.Track
{
    public class TrackSurface : MonoBehaviour
    {
        [SerializeField] private TrackSurfaceModifierScriptableObject _TrackSurfaceModifierScriptableObject;
        [HideInInspector] public TrackSurfaceModifierData TrackSurfaceModifierData;

        void Awake()
        {
            TrackSurfaceModifierData = _TrackSurfaceModifierScriptableObject?.ModifierData;
        }
    }
}
