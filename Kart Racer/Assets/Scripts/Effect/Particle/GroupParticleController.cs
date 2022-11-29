using System.Collections.Generic;
using System.Linq;
using Effect.Particle.Interface;
using UnityEngine;

namespace Effect.Particle
{
    public abstract class GroupParticleController : MonoBehaviour, IParticleController
    {
        protected List<ParticleController> _particleControllers;

        protected Color _startColor;

        protected virtual void Awake()
        {
            _particleControllers = GetComponentsInChildren<ParticleController>().ToList();

            // GameManager.Instance.OnPauseGameEvent.AddListener(PauseSystem);
            // GameManager.Instance.OnResumeGameEvent.AddListener(ResumeSystem);
        }

        public void StartSystem() => _particleControllers.ForEach(x => x.StartSystem());
        public void StopSystem() =>  _particleControllers.ForEach(x => x.StopSystem());
        public void PauseSystem() => _particleControllers.ForEach(x => x.PauseSystem());
        public void ResumeSystem() => _particleControllers.ForEach(x => x.ResumeSystem());

        public void SetStartColor(Color color) => _particleControllers.ForEach(x => x.SetStartColor(color));
        public void SetStartColorAlpha(float alpha) => _particleControllers.ForEach(x => x.SetStartColorAlpha(alpha));
    }
}
