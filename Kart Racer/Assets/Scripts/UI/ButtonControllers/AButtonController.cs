using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ButtonControllers
{
    [RequireComponent(typeof(Button))]
    public abstract class AButtonController : MonoBehaviour, ISelectHandler
    {
        protected CanvasController _canvasController;
        protected CanvasAudioController _canvasAudioController;
        protected Button _button;

        protected virtual void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _canvasAudioController = GetComponentInParent<CanvasAudioController>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClick);
        }

        public virtual void Select() => _button.Select();

        public virtual void OnClick() { }
        public virtual void OnSelect(BaseEventData eventData) { }
    }
}
