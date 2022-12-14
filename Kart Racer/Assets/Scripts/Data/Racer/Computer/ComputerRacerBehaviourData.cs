using System;
using UnityEngine;

namespace Data.Racer.Computer
{
    [Serializable]
    public class ComputerRacerBehaviourData
    {
        [Range(0f, 1f)] public float Tightness = 1f;
        [Range(0f, 1f)] public float Variance = 0f;
    }
}
