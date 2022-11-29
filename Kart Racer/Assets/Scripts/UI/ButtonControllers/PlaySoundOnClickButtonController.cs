namespace UI.ButtonControllers
{
    public class PlaySoundOnClickButtonController : AButtonController
    {
        // public AudioDataScriptableObject AudioData;

        public override void OnClick() => _audioController.Play(CanvasAudioController.CanvasAudioSoundType.Select);
    }
}
