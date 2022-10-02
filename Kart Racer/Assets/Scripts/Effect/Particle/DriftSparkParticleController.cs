using System.Collections.Generic;
using System.Linq;
using Actor.Racer;
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

            _particleSystems = GetComponentsInChildren<ParticleSystem>().ToList();
        }

        private void SetDriftLevel(int level)
        {
            if (level == 0)
            {
                StopSystem();
                return;
            }

            Debug.Log("DRIFT SPARK CONTROLLER " + level);
            StartSystem();
            SetStartColor(TurboLevelColors[level - 1]);
        }
    }
}
