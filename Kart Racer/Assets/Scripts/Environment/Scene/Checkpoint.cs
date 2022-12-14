using Actor.Racer;
using Data.Environment;
using UnityEngine;

namespace Environment.Scene
{
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {
        public int CheckpointIndex = 1;

        [SerializeField] private Vector3 _tight;
        [SerializeField] private Vector3 _loose;
        public Vector3 Tight => transform.position + transform.rotation * _tight;
        public Vector3 Loose => transform.position + transform.rotation * -_tight; //_loose;

        protected virtual void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer == null)
                return;

            // Check if a racer is heading in the correct direction of the checkpoint
            if (Vector3.Dot(racer.transform.forward, transform.forward) > 0)
                racer.TriggerCheckpoint(CheckpointIndex, transform.forward);
        }

        public virtual CheckpointType GetCheckpointType() => CheckpointType.Checkpoint;
    }
}
