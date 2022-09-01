using Util.Enums;

namespace UI.ButtonControllers
{
    public class ChangeSceneButtonController : AButtonController
    {
        public Scene TargetScene;
    
        public override void OnClick()
        {
            _canvasController.SwitchScene(TargetScene);
        }
    }
}
