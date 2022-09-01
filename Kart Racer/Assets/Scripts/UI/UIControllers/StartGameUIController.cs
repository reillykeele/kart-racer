using Util.Enums;

namespace UI.UIControllers
{
    public class StartGameUIController : UIController
    {
        public Scene TargetScene = Scene.Game;

        public void OnClick()
        {
            _canvasController.SwitchScene(TargetScene);
        }
    }
}
