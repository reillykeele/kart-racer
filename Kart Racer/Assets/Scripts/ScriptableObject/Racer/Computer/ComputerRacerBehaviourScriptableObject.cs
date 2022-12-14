using Data.Racer.Computer;
using UnityEngine;

namespace ScriptableObject.Racer.Computer
{
    [CreateAssetMenu(fileName = "ComputerRacerBehaviour", menuName = "ScriptableObjects/Racer/Computer Racer Behaviour")]
    public class ComputerRacerBehaviourScriptableObject : UnityEngine.ScriptableObject
    {
        public ComputerRacerBehaviourData ComputerRacerBehaviourData;
    }
}
