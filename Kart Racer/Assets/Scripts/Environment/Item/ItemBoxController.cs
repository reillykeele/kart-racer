using Actor.Racer;
using Actor.Racer.Player;
using Manager;
using UnityEngine;
using Util.Helpers;

namespace Environment.Item
{
    [RequireComponent(typeof(Collider))]
    public class ItemBoxController : MonoBehaviour
    {
        private Collider _collider;

        void Awake()
        {
            _collider = GetComponent<Collider>();
        }
        
        void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer != null)
            {
                racer.PickupItem();

                gameObject.Disable();

                Invoke("Respawn", GameManager.Instance.Config.ItemConfig.TimeToRespawn);
            }
        }

        public void Respawn()
        {
            gameObject.Enable();
        }
    }
}
