using UnityEngine;
using UnityEngine.UI;

namespace UI.ButtonControllers
{
    [RequireComponent(typeof(Button))]
    public abstract class AButtonController : MonoBehaviour
    {
        protected CanvasController _canvasController;
        protected Button _button;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClick);
        }

        public void Select() => _button.Select();

        public abstract void OnClick();

    }
}
