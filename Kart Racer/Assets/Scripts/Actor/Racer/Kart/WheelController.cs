using UnityEngine;
using Util.Helpers;

namespace KartRacer.Actor.Racer.Kart
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private float _wheelTurnAmount = 45f;
        [SerializeField] private float _wheelTurnSpeed = 1f;

        private Transform _flWheel;
        private Transform _frWheel;
        private Transform _blWheel;
        private Transform _brWheel;

        private float _flWheelRotY;
        private float _frWheelRotY;

        private RacerMovementController _movement;

        void Awake()
        {
            var frontWheels = gameObject.GetChildObject("FrontWheels");
            var backWheels = gameObject.GetChildObject("BackWheels");

            _flWheel = frontWheels.GetChildObject("FrontLeftWheel").transform;
            _frWheel = frontWheels.GetChildObject("FrontRightWheel").transform;

            _blWheel = backWheels.GetChildObject("BackLeftWheel").transform;
            _brWheel = backWheels.GetChildObject("BackRightWheel").transform;

            _flWheelRotY = _flWheel.localEulerAngles.y;
            _frWheelRotY = _frWheel.localEulerAngles.y;

            _movement = GetComponentInParent<RacerMovementController>();
        }

        void Update()
        {
            var steering = _movement.Steering;

            var flEulers = _flWheel.localEulerAngles;
            var frEulers = _frWheel.localEulerAngles;

        
            _flWheel.localRotation = Quaternion.Slerp(
                _flWheel.localRotation,
                Quaternion.Euler(flEulers.x, _flWheelRotY + steering * _wheelTurnAmount, flEulers.z),
                _wheelTurnSpeed * Time.deltaTime);
            _frWheel.localRotation = Quaternion.Slerp(
                _frWheel.localRotation,
                Quaternion.Euler(frEulers.x, _frWheelRotY + steering * _wheelTurnAmount, frEulers.z),
                _wheelTurnSpeed * Time.deltaTime);
        }
    }
}
