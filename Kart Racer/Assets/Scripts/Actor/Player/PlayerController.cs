using Data.Item;
using Manager;
using UnityEngine;

namespace Actor.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputController _input;

        public int Position { get; set; }
        public int Lap { get; set; }

        public ItemData Item { get; set; }

        public void Awake()
        {
            _input = GetComponent<PlayerInputController>();
        }

        public void PickupItem()
        {
            // OnPickupItem event

            // Set item based on position
            if (Item == null)
            {
                Item = GameManager.Instance.GetRandomItem();

                Debug.Log($"Player picked up {Item.Name}.");
            }
            else
            {
                Debug.Log("Player already has an item.");
            }
        }
    }
}
