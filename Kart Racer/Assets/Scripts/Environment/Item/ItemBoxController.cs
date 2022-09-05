using Actor.Player;
using UnityEngine;
using Util.Coroutine;
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

            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().PickupItem();
            }
            
            gameObject.Disable();

            Invoke("Respawn", 3f);
        }

        public void Respawn()
        {
            gameObject.Enable();
        }
    }
}
