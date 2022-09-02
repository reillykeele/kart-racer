using UnityEngine;
using Util.Enums;

namespace UI.ButtonControllers
{
    public class QuitButtonController : AButtonController
    {
        public override void OnClick() => Application.Quit(0);
    }
}