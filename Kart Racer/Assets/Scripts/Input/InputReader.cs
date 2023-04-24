using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Util.Input;

namespace KartRacer.Input
{
	[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : UnityEngine.ScriptableObject, GameInput.IGameplayActions, GameInput.IMenuActions, IInputReader
    {
        private GameInput _gameInput;

		#region Gameplay Events
		
        public event UnityAction<Vector2> SteerEvent = delegate { };
        public event UnityAction AccelerateEvent = delegate { };
        public event UnityAction AccelerateCancelledEvent = delegate { };
        public event UnityAction BrakeEvent = delegate { };
        public event UnityAction BrakeCancelledEvent = delegate { };
        public event UnityAction DriftEvent = delegate { };
        public event UnityAction DriftCancelledEvent = delegate { };
        public event UnityAction ItemEvent = delegate { };
        public event UnityAction ItemCancelledEvent = delegate { };
        public event UnityAction RearCameraEvent = delegate { };
        public event UnityAction RearCameraCancelledEvent = delegate { };

        #endregion

        #region Menu Events

        public event UnityAction<Vector2> MenuNavigateEvent = delegate { };
        public event UnityAction MenuNavigateCancelledEvent = delegate { };
        public event UnityAction MenuPauseEvent = delegate { };
        public event UnityAction MenuUnpauseEvent = delegate { };
        public event UnityAction MenuAcceptButtonEvent = delegate { };
        public event UnityAction MenuCancelButtonEvent = delegate { };
        public event UnityAction<int> ChangeTabEvent = delegate { };

        #endregion

        private void OnEnable()
		{
			if (_gameInput == null)
			{
				_gameInput = new GameInput();

				_gameInput.Gameplay.SetCallbacks(this);
				_gameInput.Menu.SetCallbacks(this);

				// Default
				EnableGameplayInput();
                // EnableMenuInput();
            }
        }

		private void OnDisable()
		{
			DisableAllInput();
		}

		public void DisableAllInput()
		{
			_gameInput.Gameplay.Disable();
			_gameInput.Menu.Disable();
		}

        #region Gameplay Actions

		public void EnableGameplayInput()
		{
			_gameInput.Menu.Disable();
			_gameInput.Gameplay.Enable();
		}

        public void OnSteer(InputAction.CallbackContext context)
        {
            SteerEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAccelerate(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    AccelerateEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    AccelerateCancelledEvent.Invoke();
                    break;
            }
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    BrakeEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    BrakeCancelledEvent.Invoke();
                    break;
            }
        }

        public void OnDrift(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    DriftEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DriftCancelledEvent.Invoke();
                    break;
            }
        }

        public void OnItem(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    ItemEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    ItemCancelledEvent.Invoke();
                    break;
            }
        }

        public void OnRearCamera(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    RearCameraEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    RearCameraCancelledEvent.Invoke();
                    break;
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuPauseEvent.Invoke();
        }

        #endregion

		#region Menu Actions

		public void EnableMenuInput()
		{
			_gameInput.Gameplay.Disable();
			_gameInput.Menu.Enable();
		}

        public void OnNavigate(InputAction.CallbackContext context)
        {
            // Handled by event system, but we still may want to access
            // ex. slider controls, enum pickers, etc.

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    MenuNavigateEvent.Invoke(context.ReadValue<Vector2>());
                    break;
                case InputActionPhase.Canceled:
                    MenuNavigateCancelledEvent.Invoke();
                    break;
            }
        }

        public void OnAccept(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuAcceptButtonEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuCancelButtonEvent.Invoke();
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuUnpauseEvent.Invoke();
        }

        public void OnChangeTab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                var val = context.ReadValue<float>();
                ChangeTabEvent.Invoke(Mathf.RoundToInt(val));
            }
        }

        #endregion

        void OnDeviceLost()
        {

        }

    }
}