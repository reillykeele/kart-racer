using Data.Racer;
using UnityEngine;

namespace ScriptableObject.Racer
{
    [CreateAssetMenu(fileName = "RacerMovement", menuName = "ScriptableObjects/Racer Movement", order = 3)]
    public class RacerMovementScriptableObject : UnityEngine.ScriptableObject
    {
        public RacerMovementData RacerMovement;
    }
}
