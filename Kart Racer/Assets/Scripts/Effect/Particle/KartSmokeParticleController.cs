using Actor.Racer;
using UnityEngine;
using Util.Helpers;

namespace Effect.Particle
{
    public class KartSmokeParticleController : ParticleController
    {
        private RacerMovementController _racerMovementController;

        [Header("Kart Smoke Particle Controller")]
        [Range(0, 255)] public float minAlpha = 0f;
        [Range(0, 255)] public float maxAlpha = 150f;

        protected override void Awake()
        {
            base.Awake();

            _racerMovementController = GetComponentInParent<RacerMovementController>();
        }

        void Update()
        {
            var speedPercent = 1 - Mathf.Clamp01(Mathf.Abs(_racerMovementController.CurrSpeed / _racerMovementController.RacerMovement.MaxSpeed));
            SetParticle(speedPercent);
        }

        public void SetParticle(float percent)
        {
            SetStartColorAlpha(MathHelper.PercentOfRange(percent, minAlpha / 255, maxAlpha / 255));
        }
    }
}
