using Data.Racer.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        public PlayerInputData PlayerInput;

        public UnityEvent OnItemEvent;

        public void OnAccelerate(InputValue val) => PlayerInput.IsAccelerating = val.isPressed;

        public void OnBrakeReverse(InputValue val) => PlayerInput.IsBraking = val.isPressed;

        public void OnDrift(InputValue val) => PlayerInput.IsDrifting = val.isPressed;

        public void OnItem(InputValue val)
        {
            PlayerInput.IsUsingItem = val.isPressed;
            OnItemEvent.Invoke();
        }

        public void OnSteer(InputValue val) => PlayerInput.Steering = val.Get<Vector2>();

    }
}
