using Environment.Scene;
using KartRacer.Environment.Scene;
using UnityEngine;
using Util.Systems;
using Util.UI.Controllers;

namespace UI
{
    [RequireComponent(typeof(UIController))]
    public class DisplayUIOnLoad : AOnSceneLoad
    {
        private UIController _uiController;

        void Awake()
        {
            _uiController = GetComponent<UIController>();
        }

        void Start()
        {
            if (LoadingSystem.Instance.IsLoading)
                _uiController.Disable();
        }

        protected override void OnSceneLoad()
        {
            Debug.Log("bruh");
            _uiController.Enable();
        }
    }
}
