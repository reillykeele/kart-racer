using System.Collections.Generic;
using KartRacer.Actor.Racer;
using UnityEngine;

namespace Effect.Particle
{
    public class DriftSparkParticleController : GroupParticleController
    {
        private RacerMovementController _racerMovementController;

        public List<Color> TurboLevelColors;

        protected override void Awake()
        {
            base.Awake();

            _racerMovementController = GetComponentInParent<RacerMovementController>();
            if (_racerMovementController == null)
            {
                enabled = false;
                return;
            }

            _racerMovementController.OnDriftLevelChangedEvent.AddListener(SetDriftLevel);
        }

        private void SetDriftLevel(int level)
        {
            if (level == 0)
            {
                StopSystem();
                return;
            }

            StartSystem();
            SetStartColor(TurboLevelColors[level - 1]);
        }
    }
}
