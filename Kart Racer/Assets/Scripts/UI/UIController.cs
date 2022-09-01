using System.Collections.Generic;
using System.Linq;
using UI.ButtonControllers;
using Util.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public Util.Enums.UI UIType;

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
    }
}
