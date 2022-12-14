using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Util.Coroutine;
using Util.Enums;
using Scene = Util.Enums.Scene;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public Util.Enums.UIPageType DefaultUiPage;

        public float MinLoadingScreenTime = 0f;

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

        public void EnableUI(Util.Enums.UIPageType target, bool resetOnSwitch = false, bool fadeIn = false)
        {
            if (target == Util.Enums.UIPageType.None) return;

            GetUI(target)?.Enable(resetOnSwitch, fadeIn);
            _lastActiveUiPage = target;
        }

        public IEnumerator EnableUICoroutine(Util.Enums.UIPageType target, bool resetOnSwitch = false)
        {
            if (target == Util.Enums.UIPageType.None) yield break;

            _lastActiveUiPage = target;
            yield return GetUI(target)?.EnableCoroutine(resetOnSwitch);
        }

        public void DisableUI(Util.Enums.UIPageType target, bool resetOnSwitch = false, bool fadeOut = false)
        {
            if (target == Util.Enums.UIPageType.None) return;

            GetUI(target)?.Disable(resetOnSwitch, fadeOut);
        }

        public IEnumerator DisableUICoroutine(Util.Enums.UIPageType target, bool resetOnSwitch = false)
        {
            if (target == Util.Enums.UIPageType.None) yield break;

            yield return GetUI(target)?.DisableCoroutine(resetOnSwitch);
        }

        public void DisplayUI(UIPageType target, bool fadeIn = false) => EnableUI(target, fadeIn: fadeIn);
        public void HideUI(UIPageType target, bool fadeOut = false) => DisableUI(target, fadeOut: fadeOut);

        public void SwitchUI(Util.Enums.UIPageType target, bool resetCurrentOnSwitch = false, bool resetTargetOnSwitch = true, bool fadeIn = false, bool fadeOut = false)
        {
            if (_lastActiveUiPage == target) return;

            StartCoroutine(CoroutineUtil.Sequence(
                DisableUICoroutine(_lastActiveUiPage, resetCurrentOnSwitch),
                EnableUICoroutine(target, resetTargetOnSwitch)
                ));

            // DisableUI(_lastActiveUiPage, resetCurrentOnSwitch, fadeOut);
            // EnableUI(target, resetTargetOnSwitch, fadeIn);
            // _lastActiveUiPage = target;
        }

        public void SwitchScene(Scene scene)
        {
            if (scene == Scene.None) return;

            LoadingManager.Instance.LoadScene(scene);
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
            var minEndTime = Time.time + MinLoadingScreenTime;
            var result = SceneManager.LoadSceneAsync(scene.ToString());
            while (result.isDone == false || Time.time <= minEndTime)
            {
                yield return null;
            }
        }
    }
}
