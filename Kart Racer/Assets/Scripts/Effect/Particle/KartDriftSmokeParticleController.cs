using Actor.Racer;

namespace Effect.Particle
{
    public class KartDriftSmokeParticleController : ParticleController
    {
        private RacerMovementController _racerMovementController;

        protected override void Awake()
        {
            base.Awake();

            _racerMovementController = GetComponentInParent<RacerMovementController>();
            if (_racerMovementController == null)
            {
                enabled = false;
                return;
            }

            _racerMovementController.OnIsDriftingChangedEvent.AddListener(SetDriftSmoke);
        }

        private void SetDriftSmoke(bool isDrifting)
        {
            if (isDrifting) 
                StartSystem(); 
            else 
                StopSystem();
        }
    }
}
