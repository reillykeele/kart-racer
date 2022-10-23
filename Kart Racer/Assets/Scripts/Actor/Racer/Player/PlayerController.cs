using UnityEngine;

namespace Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerController : RacerController
    {
        private PlayerInputController _input;
        private PlayerMovementController _movement;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInputController>();
            _movement = GetComponent<PlayerMovementController>();

            _input.OnItemEvent.AddListener(UseItem);



            FinishRaceEvent.AddListener(_ => _movement.UseAutopilot = true);
        }
    }
}
