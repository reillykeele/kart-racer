using Util.UI;
using Util.UI.Controllers.Selectables.Buttons;

namespace KartRacer.UI.ButtonControllers
{
    public class ChangeUIPageButtonController : AButtonController
    {
        public UIPage TargetUiPage;

        protected override void OnClick()
        {
            _canvasController.SwitchUI(TargetUiPage);
        }
    }
}