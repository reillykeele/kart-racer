using UnityEngine;

namespace UI.ButtonControllers
{
    public abstract class AButtonController : MonoBehaviour
    {
        protected CanvasController _canvasController;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
        }

        public abstract void OnClick();
    }
}
