using UnityEngine;
using Util.Audio;
using Util.Systems;
using Util.UI.Controllers.Selectables.Buttons;

namespace KartRacer.UI.ButtonControllers
{
    public class PlaySoundOnClickButtonController : AButtonController
    {
        [SerializeField] private AudioSoundSO _audioSound;

        protected override void OnClick() => AudioSystem.Instance.PlayAudioSound(_audioSound);
    }
}
