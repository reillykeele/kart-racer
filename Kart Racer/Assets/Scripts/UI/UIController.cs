using Util.Helpers;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public Util.Enums.UI UIType;

        protected CanvasController _canvasController;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
        }

        public virtual void Enable() => gameObject.Enable();

        public virtual void Disable() => gameObject.Disable();
    }
}
