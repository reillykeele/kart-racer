using Environment.Scene;
using Manager;

public class CourseController : AOnSceneLoad
{
    protected override void OnSceneLoad()
    {
        GameManager.Instance.StartCountdown();
    }
}
