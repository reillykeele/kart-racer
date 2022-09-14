using Data.Item;
using Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Racer
{
    [RequireComponent(typeof(RacerMovementController))]
    public abstract class RacerController : MonoBehaviour
    {
        public string Name;
        public GUID RacerId;

        public RacerMovementController MovementController { get; protected set; }

        public int Position { get; set; }
        public int Lap { get; set; }

        public Item.Item Item { get; set; }

        protected virtual void Awake()
        {
            RacerId = GUID.Generate();

            MovementController = GetComponent<RacerMovementController>();
        }

        public UnityEvent<ItemData> PickupItemEvent;
        public virtual void PickupItem()
        {
            // Set item based on position
            if (Item == null)
            {
                Item = GameManager.Instance.GetRandomItem();
                Item.SetOwner(this);

                PickupItemEvent.Invoke(Item.ItemData);

                Debug.Log($"{Name} picked up {Item.ItemData.Name}.");
            }
            else
            {
                Debug.Log($"{Name} already has {Item.ItemData.Name}.");
            }
        }

        // public UnityEvent UseItemEvent;
        public UnityEvent ClearItemEvent;
        public virtual void UseItem()
        {
            if (Item == null) return;

            Debug.Log($"Using {Item.ItemData.Name}");
            // UseItemEvent.Invoke();
            Item.UseItem();
            if (Item.Uses <= 0)
            {
                Item = null;
                ClearItemEvent.Invoke();
            }
        }
    }
}
