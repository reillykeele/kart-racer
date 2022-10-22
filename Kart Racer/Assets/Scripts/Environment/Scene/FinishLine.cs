using Actor.Racer;
using Data.Environment;
using UnityEngine;

namespace Environment.Scene
{
    [RequireComponent(typeof(Collider))]
    public class FinishLine : Checkpoint
    {
        protected override void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer == null)
                return;

            // Check if a racer is heading in the correct direction of the checkpoint
            if (Vector3.Dot(racer.transform.forward, transform.forward) > 0)
                racer.TriggerFinishLine();
        }

        public override CheckpointType GetCheckpointType() => CheckpointType.FinishLine;
    }
}
