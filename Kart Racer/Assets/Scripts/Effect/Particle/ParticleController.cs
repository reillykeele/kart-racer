using UnityEngine;
using Util.Helpers;

namespace Effect.Particle
{
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class ParticleController : MonoBehaviour
    {
        protected ParticleSystem _particleSystem;
        protected ParticleSystem.MainModule _main;

        [Header("Start Speed")] 
        [SerializeField] protected float _minStartSpeed = 0f;
        [SerializeField] protected float _maxStartSpeed = 1f;

        protected Color _startColor;

        protected virtual void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _main = _particleSystem.main;

            _startColor = _main.startColor.color;
        }

        public void StartSystem() => _particleSystem.Play();
        public void StopSystem() => _particleSystem.Stop();

        public void SetStartSpeed(float percent)
        {
            _main.startSpeed = MathHelper.PercentOfRange(percent, _minStartSpeed, _maxStartSpeed);
        }

        public void SetStartColor(Color color) => _main.startColor = color;
        public void SetStartColorAlpha(float alpha)
        {
            var c = _main.startColor.color;
            _main.startColor = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
