using UnityEngine;

namespace Actor.Racer.Kart
{
    public class SteeringWheelController : MonoBehaviour
    {
        [SerializeField] private float _steeringWheelTurnAmount = 45f;
        [SerializeField] private float _steeringWheelTurnSpeed = 1f;

        private float _steeringWheelRotZ;

        private RacerMovementController _movement;

        void Awake()
        {
            _steeringWheelRotZ = transform.localRotation.z;

            _movement = GetComponentInParent<RacerMovementController>();
        }

        void Update()
        {
            var steering = _movement.Steering;
            var eulers = transform.localEulerAngles;

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                Quaternion.Euler(eulers.x, eulers.y, _steeringWheelRotZ + steering * _steeringWheelTurnAmount),
                _steeringWheelTurnSpeed * Time.deltaTime);
        }
    }
}
