using Manager;
using UnityEngine.InputSystem;
using Util.Enums;

namespace UI.UIControllers
{
    public class StartMenuUIController : UIController
    {
        public Util.Enums.UIPageType TargetUiPageType = Util.Enums.UIPageType.MainMenu;

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.CurrentGameState = GameState.Menu;
        }

        void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame ||
                Gamepad.current.startButton.wasPressedThisFrame)
            {
                _canvasController.SwitchUI(TargetUiPageType, true);
            }
        }
    }
}
