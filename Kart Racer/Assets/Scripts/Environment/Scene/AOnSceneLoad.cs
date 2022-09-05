using UnityEngine;

namespace Environment.Scene
{
    public abstract class AOnSceneLoad : MonoBehaviour
    {
        protected virtual void Awake() => OnSceneLoad();

        protected abstract void OnSceneLoad();
    }
}
