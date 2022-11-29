using UnityEngine.EventSystems;

namespace UI.ButtonControllers
{
    public class PlaySoundOnSelectButtonController : AButtonController
    {
        // public AudioDataScriptableObject AudioData;

        public override void OnSelect(BaseEventData eventData) => _audioController.Play(CanvasAudioController.CanvasAudioSoundType.Tick);
    }
}
