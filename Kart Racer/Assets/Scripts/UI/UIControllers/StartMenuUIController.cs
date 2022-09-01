using UnityEngine.InputSystem;

namespace UI.UIControllers
{
    public class StartMenuUIController : UIController
    {
        public Util.Enums.UI TargetUI = Util.Enums.UI.MainMenu;

        void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame ||
                Gamepad.current.startButton.wasPressedThisFrame)
            {
                _canvasController.SwitchUI(TargetUI);
            }
        }
    }
}
