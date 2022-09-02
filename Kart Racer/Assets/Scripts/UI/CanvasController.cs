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
        public Util.Enums.UIType DefaultUI;

        private List<UIController> uiControllers;
        private Hashtable uiHashtable;

        private Util.Enums.UIType lastActiveUI;

        void Awake()
        {
            uiControllers = GetComponentsInChildren<UIController>().ToList();
            uiHashtable = new Hashtable();

            RegisterUIControllers(uiControllers);
        }

        void Update()
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
                GetUI(lastActiveUI)?.ReturnToUI();
        }

        void Start()
        {
            foreach (var controller in uiControllers)
                controller.Disable();

            EnableUI(DefaultUI);
        }

        public void EnableUI(Util.Enums.UIType target)
        {
            if (target == Util.Enums.UIType.None) return;
        
            GetUI(target)?.Enable();
            lastActiveUI = target;
        }

        public void DisableUI(Util.Enums.UIType target)
        {
            if (target == Util.Enums.UIType.None) return;

            GetUI(target)?.Disable();
        }

        public void SwitchUI(Util.Enums.UIType target)
        {
            if (lastActiveUI == target) return;

            DisableUI(lastActiveUI);
            EnableUI(target);
            lastActiveUI = target;
        }

        public void SwitchScene(Scene scene)
        {
            if (scene == Scene.None) return;

            SwitchUI(Util.Enums.UIType.LoadingScreen);
            StartCoroutine(LoadingScreen(scene));
        }

        private UIController GetUI(Util.Enums.UIType uiType) => (UIController) uiHashtable[uiType];

        private void RegisterUIControllers(IEnumerable<UIController> controllers)
        {
            foreach (var controller in controllers)
            {
                if (!UIExists(controller.UIType))
                    uiHashtable.Add(controller.UIType, controller);
            }
        }

        private bool UIExists(Util.Enums.UIType uiType) => uiHashtable.ContainsKey(uiType);

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
