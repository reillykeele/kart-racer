using System.Collections.Generic;
using System.Linq;
using UI.ButtonControllers;
using Util.Helpers;
using UnityEngine;
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

        void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _buttonControllers = GetComponentsInChildren<AButtonController>().ToList();
        }

        public virtual void Enable()
        {
            // Select the first button, if there are any            
            if (initialSelectedButton == null)
                _buttonControllers.FirstOrDefault()?.Select();
            else
                initialSelectedButton?.Select();

            gameObject.Enable();
        }

        public virtual void Disable() => gameObject.Disable();

        public virtual void ReturnToUI()
        {
            if (ReturnUI != UIType.None) _canvasController.SwitchUI(ReturnUI);
        }

    }
}
