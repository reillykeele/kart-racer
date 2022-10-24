using System.Collections;
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
    [RequireComponent(typeof(CanvasGroup))]
    public class UIController : MonoBehaviour
    {
        public UIPageType UiPageType;
        public UIPageType ReturnUiPage;

        public Button initialSelectedButton = null;

        private CanvasGroup _canvasGroup;
        protected CanvasController _canvasController;
        protected List<AButtonController> _buttonControllers;

        protected Button lastSelectedButton = null;

        protected IEnumerable<Animator> _animators;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _buttonControllers = GetComponentsInChildren<AButtonController>().ToList();

            _animators = GetComponentsInChildren<Animator>();

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Reset()
        {
            lastSelectedButton = null;
        }

        public virtual void Enable(bool resetOnSwitch = false, bool fadeIn = false)
        {
            if (resetOnSwitch)
                Reset();

            if (lastSelectedButton != null)
                lastSelectedButton.Select();
            else if (initialSelectedButton != null)
                initialSelectedButton.Select();
            else
                _buttonControllers?.FirstOrDefault()?.Select();

            gameObject.Enable();

            if (fadeIn)
                StartCoroutine(UIHelper.FadeIn(_canvasGroup));
            else if (_canvasGroup != null)
                _canvasGroup.alpha = 1f;

            if (_animators != null)
                foreach (var animator in _animators)
                    animator.TrySetBool("transitionIn", resetOnSwitch);
        }

        public virtual void Disable(bool resetOnSwitch = false, bool fadeOut = false)
        {
            lastSelectedButton = EventSystem.current?.currentSelectedGameObject?.GetComponent<Button>();
                
            if (fadeOut)
                StartCoroutine(UIHelper.FadeOut(_canvasGroup, () => gameObject.Disable()));
            else
                gameObject.Disable();
        }

        public virtual void ReturnToUI()
        {
            if (ReturnUiPage != UIPageType.None) {_canvasController.SwitchUI(ReturnUiPage, resetTargetOnSwitch: false, fadeOut: true);}
        }

        public IEnumerator FadeIn()
        {
            _canvasGroup.alpha = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += 0.05f;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut()
        {
            _canvasGroup.alpha = 1f;

            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= 0.05f;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
        }
    }
}
