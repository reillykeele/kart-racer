using Manager;

namespace Environment.Scene
{
    public class CourseController : AOnSceneLoad
    {
        protected override void OnSceneLoad()
        {
            GameManager.Instance.StartCountdown();
        }
    }
}
