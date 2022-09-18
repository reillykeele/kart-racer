using Manager;
using UnityEngine;

namespace Actor.Racer.Computer
{
    [RequireComponent(typeof(ComputerController))]
    public class ComputerMovementController : RacerMovementController
    {
        private ComputerController _computerController;

        private GameObject _target;

        protected override void Awake()
        {
            base.Awake();

            _computerController = GetComponent<ComputerController>();
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            _currSpeed = 10;

            var targetCheckpoint = GameManager.Instance.CourseController.GetNextCheckpoint(_computerController.CheckpointsReached);
            _target = targetCheckpoint.gameObject;

            var targetDirection = (_target.transform.position - transform.position);
            targetDirection.y = 0;
            targetDirection.Normalize();
            Debug.DrawRay(transform.position, 3 * targetDirection, Color.red);

            var forward = Vector3.Slerp(transform.forward, targetDirection, 0.05f);

            transform.forward = forward;

            // var forward = transform.forward;
            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var movement = forward * _currSpeed * Time.fixedDeltaTime;

            // Apply gravity
            movement.y -= !IsGrounded() ? RacerMovement.GravitySpeed : RacerMovement.ConstantGravitySpeed;

            // Move player
            transform.position += movement;
        }
    }
}
