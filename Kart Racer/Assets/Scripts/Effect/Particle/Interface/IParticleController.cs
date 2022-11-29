using UnityEngine;

namespace Effect.Particle.Interface
{
    public interface IParticleController
    {
        void StartSystem();
        void StopSystem();
        void PauseSystem();
        void ResumeSystem();

        void SetStartColor(Color color);
        void SetStartColorAlpha(float alpha);
    }
}