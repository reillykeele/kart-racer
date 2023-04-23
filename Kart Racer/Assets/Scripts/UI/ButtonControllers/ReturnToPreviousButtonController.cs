using Util.UI.Controllers.Selectables.Buttons;

namespace KartRacer.UI.ButtonControllers
{
    public class ReturnToPreviousButtonController : AButtonController
    {
        protected override void OnClick()
        {
            _canvasController.ReturnToPrevious();
        }
    }
}
