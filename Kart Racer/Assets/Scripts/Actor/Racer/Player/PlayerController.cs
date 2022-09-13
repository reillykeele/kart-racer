using Manager;
using UnityEngine;

namespace Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovementController MovementController { get; private set; }
        private PlayerInputController _input;

        public int Position { get; set; }
        public int Lap { get; set; }

        public Item.Item Item { get; set; }

        public void Awake()
        {
            MovementController = GetComponent<PlayerMovementController>();
            _input = GetComponent<PlayerInputController>();

            _input.OnItemEvent.AddListener(UseItem);
        }

        public void PickupItem()
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

        public void UseItem()
        {
            if (Item == null) return;

            Debug.Log($"Using {Item.ItemData.Name}");
            Item.UseItem();
            if (Item.Uses <= 0)
                Item = null;
        }
    }
}
