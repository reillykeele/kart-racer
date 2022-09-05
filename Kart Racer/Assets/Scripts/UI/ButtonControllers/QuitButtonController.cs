using UnityEngine;

namespace UI.ButtonControllers
{
    public class QuitButtonController : AButtonController
    {
        public override void OnClick() => Application.Quit(0);
    }
}