using System;
using UnityEngine;

namespace Data.Player
{
    [Serializable]
    public struct PlayerMovementData
    {
        [Header("Speed")]
        public float AccelerationSpeed;
        public float MaxSpeed;

        public float ReverseAccelerationSpeed;
        public float MaxReverseSpeed;

        public float DecelerationSpeed;
        public float BrakeSpeed;

        [Header("Handling")]
        public float TurningSpeed;

        [Header("Gravity")]
        public float GravitySpeed;
        public float ConstantGravitySpeed;

        [Header("Boost")] 
        public float BoostDuration;
    }
}