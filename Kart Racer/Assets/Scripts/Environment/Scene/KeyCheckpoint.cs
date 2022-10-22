using Actor.Racer;
using Data.Environment;
using UnityEngine;

namespace Environment.Scene
{
    [RequireComponent(typeof(Collider))]
    public class KeyCheckpoint : Checkpoint
    {
        public int KeyCheckPointIndex;

        protected override void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer == null)
                return;

            // Check if a racer is heading in the correct direction of the checkpoint
            if (Vector3.Dot(racer.transform.forward, transform.forward) > 0)
                racer.TriggerKeyCheckpoint(CheckpointIndex, KeyCheckPointIndex, transform.forward);
        }

        public override CheckpointType GetCheckpointType() => CheckpointType.KeyCheckpoint;
    }
}
