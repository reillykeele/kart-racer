using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Effect.Particle
{
    public abstract class GroupParticleController : MonoBehaviour
    {
        protected List<ParticleSystem> _particleSystems;
        protected List<ParticleSystem.MainModule> _particleSystemMains;

        protected Color _startColor;

        protected virtual void Awake()
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>().ToList();
            _particleSystemMains = _particleSystems.Select(x => x.main).ToList();
        }

        public void StartSystem() => _particleSystems.ForEach(x => x.Play());
        public void StopSystem() =>  _particleSystems.ForEach(x => x.Stop());

        public void SetStartColor(Color color) => _particleSystemMains.ForEach(x => x.startColor = color);
        public void SetStartColorAlpha(float alpha)
        {
            _particleSystemMains.ForEach(x =>
            {
                var c = x.startColor.color;
                x.startColor = new Color(c.r, c.g, c.b, alpha);
            });
        }
    }
}
