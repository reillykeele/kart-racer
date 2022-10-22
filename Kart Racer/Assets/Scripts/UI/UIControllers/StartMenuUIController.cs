using UnityEngine.InputSystem;

namespace UI.UIControllers
{
    public class StartMenuUIController : UIController
    {
        public Util.Enums.UIPageType TargetUiPageType = Util.Enums.UIPageType.MainMenu;

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
