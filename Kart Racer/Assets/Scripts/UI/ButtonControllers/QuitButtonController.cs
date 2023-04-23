using Util.Systems;
using Util.UI.Controllers.Selectables.Buttons;

namespace KartRacer.UI.ButtonControllers
{
    public class QuitButtonController : AButtonController
    {
        protected override void OnClick()
        {
            // _canvasAudioController?.FadeOutBackgroundMusic();
            LoadingSystem.Instance.QuitGame();
        }
    }
}