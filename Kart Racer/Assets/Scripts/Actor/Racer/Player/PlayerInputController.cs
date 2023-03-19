using Data.Racer.Player;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        public PlayerInputData PlayerInput;

        public void OnAccelerate(InputValue val) => PlayerInput.IsAccelerating = val.isPressed;

        public void OnBrakeReverse(InputValue val) => PlayerInput.IsBraking = val.isPressed;

        public UnityEvent<bool> OnDriftEvent;
        public void OnDrift(InputValue val)
        {
            PlayerInput.IsDrifting = val.isPressed;
            OnDriftEvent.Invoke(PlayerInput.IsDrifting);
        }

        public UnityEvent OnItemEvent;
        public void OnItem(InputValue val)
        {
            PlayerInput.IsUsingItem = val.isPressed;
            if (val.isPressed)
                OnItemEvent.Invoke();
        }

        public void OnSteer(InputValue val) => PlayerInput.Steering = val.Get<Vector2>();

        public UnityEvent<bool> OnLookBehindEvent;
        public void OnLookBehind(InputValue val)
        {
            PlayerInput.IsLookingBehind = val.isPressed;
            OnLookBehindEvent.Invoke(PlayerInput.IsLookingBehind);
        }

        public void OnPause(InputValue val)
        {
            if (val.isPressed)
                GameManager.Instance.PauseGame();
        }
    }
}
