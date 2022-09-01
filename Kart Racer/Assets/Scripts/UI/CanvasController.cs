using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = Util.Enums.Scene;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public Util.Enums.UI DefaultUI;

        private List<UIController> uiControllers;
        private Hashtable uiHashtable;

        private Util.Enums.UI lastActiveUI;

        void Awake()
        {
            uiControllers = GetComponentsInChildren<UIController>().ToList();
            uiHashtable = new Hashtable();

            RegisterUIControllers(uiControllers);
        }

        void Start()
        {
            foreach (var controller in uiControllers)
                controller.Disable();

            EnableUI(DefaultUI);
        }

        public void EnableUI(Util.Enums.UI target)
        {
            if (target == Util.Enums.UI.None) return;
        
            GetUI(target)?.Enable();
            lastActiveUI = target;
        }

        public void DisableUI(Util.Enums.UI target)
        {
            if (target == Util.Enums.UI.None) return;

            GetUI(target)?.Disable();
        }

        public void SwitchUI(Util.Enums.UI target)
        {
            if (lastActiveUI == target) return;

            DisableUI(lastActiveUI);
            EnableUI(target);
            lastActiveUI = target;
        }

        public void SwitchScene(Scene scene)
        {
            if (scene == Scene.None) return;

            SwitchUI(Util.Enums.UI.LoadingScreen);
            StartCoroutine(LoadingScreen(scene));
        }

        private UIController GetUI(Util.Enums.UI ui) => (UIController) uiHashtable[ui];

        private void RegisterUIControllers(IEnumerable<UIController> controllers)
        {
            foreach (var controller in controllers)
            {
                if (!UIExists(controller.UIType))
                    uiHashtable.Add(controller.UIType, controller);
            }
        }

        private bool UIExists(Util.Enums.UI ui) => uiHashtable.ContainsKey(ui);

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
