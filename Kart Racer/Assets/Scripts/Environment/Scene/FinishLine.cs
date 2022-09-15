using Actor.Racer;
using UnityEngine;

namespace Environment.Scene
{
    [RequireComponent(typeof(Collider))]
    public class FinishLine : MonoBehaviour
    {
        void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer != null)
            {
                racer.TriggerFinishLine();
            }
        }
    }
}
