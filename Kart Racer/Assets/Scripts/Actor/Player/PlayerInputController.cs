using Data.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        public PlayerInputData PlayerInput;

        public void OnAccelerate(InputValue val) => PlayerInput.IsAccelerating = val.isPressed;

        public void OnBrakeReverse(InputValue val) => PlayerInput.IsBraking = val.isPressed;

        public void OnDrift(InputValue val) => PlayerInput.IsDrifting = val.isPressed;

        public void OnItem(InputValue val) => PlayerInput.IsUsingItem = val.isPressed;

        public void OnSteer(InputValue val) => PlayerInput.Steering = val.Get<Vector2>();

    }
}
