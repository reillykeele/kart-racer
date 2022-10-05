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
            if (racer == null)
                return;

            // Check if a racer is heading in the correct direction of the checkpoint
            if (Vector3.Dot(racer.transform.forward, transform.forward) > 0)
                racer.TriggerCheckpoint(CheckpointIndex, transform.forward);
        }
    }
}
