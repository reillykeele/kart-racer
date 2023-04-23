using UnityEngine;
using Util.GameEvents;

namespace KartRacer.Environment.Scene
{
    public abstract class AOnSceneLoad : MonoBehaviour
    {
        [SerializeField] protected VoidGameEventSO _onSceneLoadedEvent = default;

        protected virtual void OnEnable()
        {
            _onSceneLoadedEvent.OnEventRaised += OnSceneLoad;
        }

        protected virtual void OnDisable()
        {
            _onSceneLoadedEvent.OnEventRaised -= OnSceneLoad;
        }

        protected abstract void OnSceneLoad();
    }
}
