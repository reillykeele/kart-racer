using Manager;
using UnityEngine;

namespace Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerController : RacerController
    {
        private PlayerInputController _input;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInputController>();

            _input.OnItemEvent.AddListener(UseItem);
        }
    }
}
