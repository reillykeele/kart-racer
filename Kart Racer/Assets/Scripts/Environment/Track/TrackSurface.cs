using Data.Environment;
using KartRacer.ScriptableObject.Track;
using UnityEngine;

namespace KartRacer.Environment.Track
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
