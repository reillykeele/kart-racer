using Effect.Particle.Interface;
using UnityEngine;
using Util.Helpers;
using Util.Systems;

namespace Effect.Particle
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleController : MonoBehaviour, IParticleController
    {
        protected ParticleSystem _particleSystem;
        protected ParticleSystem.MainModule _main;

        [Header("Start Speed")] 
        [SerializeField] protected float _minStartSpeed = 0f;
        [SerializeField] protected float _maxStartSpeed = 1f;

        protected Color _startColor;

        private bool _isEmitting = false;

        protected virtual void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _main = _particleSystem.main;

            _startColor = _main.startColor.color;

            GameSystem.Instance.OnPauseGameEvent.AddListener(PauseSystem);
            GameSystem.Instance.OnResumeGameEvent.AddListener(ResumeSystem);
        }

        public void StartSystem() => _particleSystem.Play();
        public void StopSystem() => _particleSystem.Stop();
        public void PauseSystem()
        {
            _isEmitting = _particleSystem.isEmitting;
            if (_isEmitting)
            {
                _particleSystem.Pause();
            }
        }

        public void ResumeSystem()
        {
            if (_particleSystem.isPaused && _isEmitting)
            {
                Debug.Log($"RESUMING {gameObject.name} isPaused={_particleSystem.isPaused}, isEmitting={_particleSystem.isEmitting}, isPlaying={_particleSystem.isPlaying}, isStopped={_particleSystem.isStopped}, isAlive={_particleSystem.IsAlive()}");
                _particleSystem.Play();
            }
        }

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
