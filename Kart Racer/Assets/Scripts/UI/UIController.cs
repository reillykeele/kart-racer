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
        public UIType UIType;
        public UIType ReturnUI;

        public Button initialSelectedButton = null;

        protected CanvasController _canvasController;
        protected List<AButtonController> _buttonControllers;

        protected Button lastSelectedButton = null;

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _buttonControllers = GetComponentsInChildren<AButtonController>().ToList();
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
        }

        public virtual void Disable(bool resetOnSwitch = false)
        {
            lastSelectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                
            gameObject.Disable();
        }

        public virtual void ReturnToUI()
        {
            if (ReturnUI != UIType.None) {_canvasController.SwitchUI(ReturnUI, resetTargetOnSwitch: false);}
        }

    }
}
