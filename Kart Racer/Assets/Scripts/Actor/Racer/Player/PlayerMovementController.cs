using UnityEngine;

namespace KartRacer.Actor.Racer.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMovementController : RacerMovementController
    {
        private PlayerInputController _input;

        protected override bool IsAccelerating => _input.IsAccelerating;
        protected override bool IsBraking => _input.IsBraking;
        // protected bool IsDrifting => _input.IsDrifting;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInputController>();
        }

        public override void Steer(out float steerDirection, out Vector3 movement)
        {
            Steering = _input.Steering.x;
            if (_isDrifting)
            {
                if (DriftDirection == 0)
                    DriftDirection = Steering == 0 ? 0 : (int)Mathf.Sign(Steering);

                steerDirection =
                    DriftDirection * (RacerMovement.TurningSpeed * 1.25f) +
                    Steering * (RacerMovement.TurningSpeed * 0.75f);

                // Add outward velocity 
                var dir = (transform.forward + transform.right * RacerMovement.OutwardDriftPercentage * -DriftDirection).normalized;
                movement = dir * CurrSpeed;

                // Calculate turbo progress
                ++_driftProgress;
                if (DriftLevel < 3 && _driftProgress >= 225)
                    DriftLevel = 3;
                else if (DriftLevel < 2 && _driftProgress >= 150)
                    DriftLevel = 2;
                else if (DriftLevel < 1 && _driftProgress >= 75)
                    DriftLevel = 1;
            }
            else
            {
                DriftDirection = 0;
                steerDirection = Steering * RacerMovement.TurningSpeed;

                movement = transform.forward * CurrSpeed;
            }
        }
    }
}
