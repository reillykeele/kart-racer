using Util.Enums;

namespace UI.ButtonControllers
{
    public class ChangeUIPageButtonController : AButtonController
    {
        public UIPageType TargetUiPageType;
    
        public override void OnClick()
        {
            _canvasController.SwitchUI(TargetUiPageType);
        }
    }
}