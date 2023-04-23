using KartRacer.Data.Config;
using KartRacer.Manager;
using UnityEngine;
using Util.UI.Controllers.Selectables.Buttons;

namespace KartRacer.UI.ButtonControllers
{
    public class SetGameModeButtonController : AButtonController
    {
        [SerializeField] public GameMode TargetGameMode;

        protected override void OnClick()
        {
            GameManager.Instance.GameMode = TargetGameMode;
        }
    }
}