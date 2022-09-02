using Util.Enums;

namespace UI.ButtonControllers
{
    public class ChangeUIButtonController : AButtonController
    {
        public UIType TargetUiType;
    
        public override void OnClick()
        {
            _canvasController.SwitchUI(TargetUiType);
        }
    }
}