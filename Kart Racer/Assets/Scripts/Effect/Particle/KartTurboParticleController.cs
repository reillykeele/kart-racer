using KartRacer.Actor.Racer;

namespace Effect.Particle
{
    public class KartTurboParticleController : ParticleController
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

            _racerMovementController.OnIsBoostingChangedEvent.AddListener(SetTurbo);
        }

        private void SetTurbo(bool isBoosting)
        {
            if (isBoosting) 
                StartSystem(); 
            else 
                StopSystem();
        }
    }
}
