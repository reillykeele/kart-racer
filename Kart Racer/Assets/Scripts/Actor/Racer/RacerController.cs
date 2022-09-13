using Manager;
using UnityEngine;

namespace Actor.Racer
{
    [RequireComponent(typeof(RacerMovementController))]
    public abstract class RacerController : MonoBehaviour
    {
        public RacerMovementController MovementController { get; protected set; }

        public int Position { get; set; }
        public int Lap { get; set; }

        public Item.Item Item { get; set; }

        protected virtual void Awake()
        {
            MovementController = GetComponent<RacerMovementController>();
        }

        public virtual void PickupItem()
        {
            // OnPickupItem event

            // Set item based on position
            if (Item == null)
            {
                Item = GameManager.Instance.GetRandomItem();
                Item.SetOwner(this);

                Debug.Log($"Player picked up {Item.ItemData.Name}.");
            }
            else
            {
                Debug.Log("Player already has an item.");
            }
        }

        public virtual void UseItem()
        {
            if (Item == null) return;

            Debug.Log($"Using {Item.ItemData.Name}");
            Item.UseItem();
            if (Item.Uses <= 0)
                Item = null;
        }
    }
}
