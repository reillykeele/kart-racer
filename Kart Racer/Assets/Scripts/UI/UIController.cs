using System.Collections.Generic;
using System.Linq;
using UI.ButtonControllers;
using Util.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.Enums;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public UIPageType UiPageType;
        public UIPageType ReturnUiPage;

        public Button initialSelectedButton = null;

        protected CanvasController _canvasController;
        protected List<AButtonController> _buttonControllers;

        protected Button lastSelectedButton = null;

        protected IEnumerable<Animator> _animators;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _buttonControllers = GetComponentsInChildren<AButtonController>().ToList();

            _animators = GetComponentsInChildren<Animator>();
        }

        public virtual void Reset()
        {
            lastSelectedButton = null;
        }

        public virtual void Enable(bool resetOnSwitch = false)
        {
            if (resetOnSwitch)
                Reset();

            if (lastSelectedButton != null)
                lastSelectedButton.Select();
            else if (initialSelectedButton != null)
                initialSelectedButton.Select();
            else
                _buttonControllers.FirstOrDefault()?.Select();

            gameObject.Enable();

            foreach (var animator in _animators)
                animator.TrySetBool("transitionIn", resetOnSwitch);
        }

        public virtual void Disable(bool resetOnSwitch = false)
        {
            lastSelectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                
            gameObject.Disable();
        }

        public virtual void ReturnToUI()
        {
            if (ReturnUiPage != UIPageType.None) {_canvasController.SwitchUI(ReturnUiPage, resetTargetOnSwitch: false);}
        }

    }
}
