using UnityEngine;

namespace Actor.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputController _input;

        public int Position { get; set; }
        public int Lap { get; set; }

        public int Item { get; set; }

        public void Awake()
        {
            _input = GetComponent<PlayerInputController>();
        }

        public void PickupItem()
        {
            Debug.Log("Get Item");
            // OnPickupItem event

            // Set item based on position
            // Create a helper ?
        }
    }
}
