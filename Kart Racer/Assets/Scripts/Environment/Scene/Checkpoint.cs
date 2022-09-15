using Actor.Racer;
using UnityEngine;

namespace Environment.Scene
{
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {
        public int CheckpointIndex = 1;

        protected virtual void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer != null)
            {
                racer.TriggerCheckpoint(CheckpointIndex);
            }
        }
    }
}
