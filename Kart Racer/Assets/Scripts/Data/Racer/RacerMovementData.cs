using System;
using UnityEngine;

namespace Data.Racer
{
    [Serializable]
    public struct RacerMovementData
    {
        [Header("Speed")]
        public float AccelerationSpeed;
        public float MaxSpeed;

        public float ReverseAccelerationSpeed;
        public float MaxReverseSpeed;

        public float DecelerationSpeed;
        public float BrakeSpeed;

        public float OutwardDriftPercentage;

        [Header("Handling")]
        public float TurningSpeed;

        [Header("Gravity")]
        public float GravitySpeed;
        public float ConstantGravitySpeed;

        [Header("Boost")] 
        public float BoostDuration;
    }
}