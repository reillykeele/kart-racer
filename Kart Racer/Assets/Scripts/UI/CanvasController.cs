using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Scene = Util.Enums.Scene;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public Util.Enums.UIPageType DefaultUiPage;

        private List<UIController> uiControllers;
        private Hashtable uiHashtable;

        private Util.Enums.UIPageType _lastActiveUiPage;

        void Awake()
        {
            uiControllers = GetComponentsInChildren<UIController>().ToList();
            uiHashtable = new Hashtable();

            RegisterUIControllers(uiControllers);
        }

        void Update()
        {
            if (Gamepad.current?.buttonEast.wasPressedThisFrame == true)
                GetUI(_lastActiveUiPage)?.ReturnToUI();
        }

        void Start()
        {
            foreach (var controller in uiControllers)
                controller.Disable();

            EnableUI(DefaultUiPage);
        }

        public void EnableUI(Util.Enums.UIPageType target, bool resetOnSwitch = false)
        {
            if (target == Util.Enums.UIPageType.None) return;
        
            GetUI(target)?.Enable(resetOnSwitch);
            _lastActiveUiPage = target;
        }

        public void DisableUI(Util.Enums.UIPageType target, bool resetOnSwitch = false)
        {
            if (target == Util.Enums.UIPageType.None) return;

            GetUI(target)?.Disable(resetOnSwitch);
        }

        public void SwitchUI(Util.Enums.UIPageType target, bool resetCurrentOnSwitch = false, bool resetTargetOnSwitch = true)
        {
            if (_lastActiveUiPage == target) return;

            DisableUI(_lastActiveUiPage, resetCurrentOnSwitch);
            EnableUI(target, resetTargetOnSwitch);
            _lastActiveUiPage = target;
        }

        public void SwitchScene(Scene scene)
        {
            if (scene == Scene.None) return;

            SwitchUI(Util.Enums.UIPageType.LoadingScreen);
            StartCoroutine(LoadingScreen(scene));
        }

        private UIController GetUI(Util.Enums.UIPageType uiPageType) => (UIController) uiHashtable[uiPageType];

        private void RegisterUIControllers(IEnumerable<UIController> controllers)
        {
            foreach (var controller in controllers)
            {
                if (!UIExists(controller.UiPageType))
                    uiHashtable.Add(controller.UiPageType, controller);
            }
        }

        private bool UIExists(Util.Enums.UIPageType uiPageType) => uiHashtable.ContainsKey(uiPageType);

        IEnumerator LoadingScreen(Scene scene)
        {
            var result = SceneManager.LoadSceneAsync(scene.ToString());
            while (!result.isDone)
            {
                yield return null;
            }
        }
    }
}
