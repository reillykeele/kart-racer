using Manager;
using UnityEngine;

namespace Actor.Racer.Computer
{
    [RequireComponent(typeof(ComputerController))]
    public class ComputerMovementController : RacerMovementController
    {
        private ComputerController _computerController;

        // private GameObject _target;

        protected override void Awake()
        {
            base.Awake();

            _computerController = GetComponent<ComputerController>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // if (!GameManager.Instance.IsPlaying()) return;
            //
            // CurrSpeed = RacerMovement.MaxSpeed;
            //
            // var targetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_computerController.CheckpointsReached);
            // _target = targetCheckpoint.gameObject;
            //
            // var targetDirection = (_target.transform.position - transform.position);
            // targetDirection.y = 0;
            // targetDirection.Normalize();
            // Debug.DrawRay(transform.position, 3 * targetDirection, Color.red);
            //
            // var forward = Vector3.Slerp(transform.forward, targetDirection, 0.05f);
            //
            // transform.forward = forward;
            //
            // // var forward = transform.forward;
            // Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            // var movement = forward * CurrSpeed * Time.fixedDeltaTime;
            //
            // // Apply gravity
            // var pos = transform.position;
            // if (IsGrounded(out var hitInfo))
            // {
            //     pos.y = hitInfo.point.y + 0.5f; // TODO: Change to some "dist to ground" var
            //
            //     var groundNormal = hitInfo.normal;
            //     var forwardDirection = forward.normalized;
            //     
            //     // Project the forward & surface normal using the dot product
            //     // Set the rotation w/ relative forward and up axes
            //     var rotForward = forwardDirection - groundNormal * Vector3.Dot (forwardDirection, groundNormal);
            //     transform.rotation = Quaternion.Slerp(
            //         transform.rotation, 
            //         Quaternion.LookRotation(rotForward.normalized, groundNormal), 
            //         8f * Time.fixedDeltaTime);
            // }
            // else
            //     movement.y -= RacerMovement.GravitySpeed;
            //
            // // Move player
            // // transform.Rotate(transform.up, steerDirection * Time.fixedDeltaTime);
            // transform.position = pos + movement;
        }
    }
}
