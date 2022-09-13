using Manager;

namespace Actor.Racer.Computer
{
    public class ComputerMovementController : RacerMovementController
    {


        void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;
        }
    }
}
