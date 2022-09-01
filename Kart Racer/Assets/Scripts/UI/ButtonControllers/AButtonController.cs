using UnityEngine;
using UnityEngine.UI;

namespace UI.ButtonControllers
{
    public abstract class AButtonController : MonoBehaviour
    {
        protected CanvasController _canvasController;
        protected Button _button;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _button = GetComponent<Button>();
        }

        public void Select() => _button.Select();

        public abstract void OnClick();

    }
}
