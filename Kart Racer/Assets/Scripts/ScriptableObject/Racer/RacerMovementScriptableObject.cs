using Data.Racer;
using UnityEngine;

namespace ScriptableObject.Racer
{
    [CreateAssetMenu(fileName = "RacerMovement", menuName = "ScriptableObjects/Racer/Racer Movement")]
    public class RacerMovementScriptableObject : UnityEngine.ScriptableObject
    {
        public RacerMovementData RacerMovement;
    }
}
