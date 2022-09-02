using UnityEngine.InputSystem;

namespace UI.UIControllers
{
    public class StartMenuUIController : UIController
    {
        public Util.Enums.UIType TargetUiType = Util.Enums.UIType.MainMenu;

        void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame ||
                Gamepad.current.startButton.wasPressedThisFrame)
            {
                _canvasController.SwitchUI(TargetUiType);
            }
        }
    }
}
