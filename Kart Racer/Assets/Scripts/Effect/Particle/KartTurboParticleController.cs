using System.Collections;
using System.Collections.Generic;
using Actor.Racer;
using Effect.Particle;
using UnityEngine;

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

    public void SetTurbo(bool isBoosting)
    {
        if (isBoosting) 
            StartSystem(); 
        else 
            StopSystem();
    }
}
