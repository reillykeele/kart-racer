using Util.Coroutine;
using Util.Enums;

namespace UI.ButtonControllers
{
    public class ChangeSceneButtonController : AButtonController
    {
        public Scene TargetScene;
        public float Delay = 0f;
    
        public override void OnClick()
        {
            StartCoroutine(CoroutineUtil.WaitForExecute(() => _canvasController.SwitchScene(TargetScene), Delay));
        }
    }
}
