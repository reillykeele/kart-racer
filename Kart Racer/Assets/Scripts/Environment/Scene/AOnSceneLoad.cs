using Manager;
using UnityEngine;

namespace Environment.Scene
{
    public abstract class AOnSceneLoad : MonoBehaviour
    {
        protected virtual void Awake()
        {
            LoadingManager.Instance.OnSceneLoadedEvent.AddListener(OnSceneLoad);
        }

        protected abstract void OnSceneLoad();
    }
}
