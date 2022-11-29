using Manager;

namespace UI.ButtonControllers
{
    public class QuitButtonController : AButtonController
    {
        public override void OnClick()
        {
            _audioController.FadeOutBackgroundMusic();
            LoadingManager.Instance.QuitGame();
        }
    }
}