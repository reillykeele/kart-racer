using Data.Racer.Computer;
using Manager;
using ScriptableObject.Racer.Computer;
using UnityEngine;
using Util.Helpers;

namespace Actor.Racer.Computer
{
    [RequireComponent(typeof(ComputerController))]
    public class ComputerMovementController : RacerMovementController
    {
        private ComputerController _computerController;

        [SerializeField] private ComputerRacerBehaviourScriptableObject _computerRacerBehaviourScriptableObject;
        private ComputerRacerBehaviourData ComputerRacerBehaviourData;

        private float currTargetVariance = 0f;
        private float currLookaheadVariance = 0f;

        protected override bool IsAccelerating => true;
        protected override bool IsBraking => false;

        protected override void Awake()
        {
            base.Awake();

            _computerController = GetComponent<ComputerController>();
            ComputerRacerBehaviourData = _computerRacerBehaviourScriptableObject.ComputerRacerBehaviourData;
        }

        // protected override void FixedUpdate()
        // {
        //     if (!GameManager.Instance.IsPlaying()) return;
        //
        //     if (UseAutopilot)
        //     {
        //         Autopilot();
        //         return;
        //     }
        //
        //     base.FixedUpdate();
        //     return;
        //
        //     // // Check the surface we are on 
        //     // var isGrounded = IsGrounded(out var groundedHitInfo, out var trackSurfaceModifier);
        //     //
        //     // // Calculate what our current speed should be
        //     // var velocity = _rb.velocity;
        //     // velocity.y = 0;
        //     // CurrSpeed = velocity.magnitude * Mathf.Sign(CurrSpeed);
        //     // CurrSpeed = CalculateSpeed(CurrSpeed, IsAccelerating, IsBraking, trackSurfaceModifier);
        //     //
        //     // // Calculate direction
        //     // var forward = transform.forward;
        //     // Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
        //     //
        //     // // Calculate and apply steering
        //     // var targetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached);
        //     // var lookaheadTargetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached + 1);
        //     //
        //     // // var distToTarget = (targetCheckpoint.transform.position - transform.position).magnitude;
        //     // // var targetTolookahead = (lookaheadTargetCheckpoint.transform.position - targetCheckpoint.transform.position);
        //     // // if (distToTarget <= targetTolookahead.magnitude)
        //     // // { 
        //     //
        //     // if (targetCheckpoint.gameObject != _target)
        //     // {
        //     //     currTargetVariance = currLookaheadVariance;
        //     //     currLookaheadVariance = Random.Range(-ComputerRacerBehaviourData.Variance, ComputerRacerBehaviourData.Variance);
        //     // }
        //     //
        //     // var targetPoint = Vector3.Lerp(targetCheckpoint.Loose, targetCheckpoint.Tight, ComputerRacerBehaviourData.Tightness + currTargetVariance);
        //     // var lookaheadPoint = Vector3.Lerp(lookaheadTargetCheckpoint.Loose, lookaheadTargetCheckpoint.Tight, ComputerRacerBehaviourData.Tightness + currLookaheadVariance);
        //     // var points = new[] { transform.position, targetPoint, lookaheadPoint };
        //     // var smoothedPath = PathHelper.SmoothPath(points, 6);
        //     //
        //     // for (var i = 0; i < smoothedPath.Length; i++)
        //     // {
        //     //     smoothedPath[i].y = transform.position.y;
        //     //     DebugDrawHelper.DrawBox(smoothedPath[i], new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity, Color.green);
        //     // }
        //     //
        //     // var targetDirection = smoothedPath[1] - transform.position;
        //     // targetDirection.y = 0;
        //     // targetDirection.Normalize();
        //     // Debug.DrawRay(transform.position, 4 * targetDirection, Color.green);
        //     //
        //     // // }
        //     // // else
        //     // // {
        //     // //     targetDirection = targetCheckpoint.transform.position - transform.position;
        //     // //     targetDirection.y = 0;
        //     // //     targetDirection.Normalize();
        //     // //     Debug.DrawRay(transform.position, 3 * targetDirection, Color.red);
        //     // // }
        //     //
        //     // _target = targetCheckpoint.gameObject;
        //     // _lookaheadTarget = lookaheadTargetCheckpoint.gameObject;
        //     //
        //     // var angle = Vector3.Cross(transform.forward, targetDirection);
        //     // var dir = Vector3.Dot(angle, Vector3.up);
        //     //
        //     // Steering = dir > SteeringTolerance ? 1f : dir < -SteeringTolerance ? -1f : dir;
        //     // var steerDirection = Steering * RacerMovement.TurningSpeed;
        //     //
        //     // var movement = forward * CurrSpeed;
        //     //
        //     // // Apply gravity and rotate to ground normal
        //     // if (isGrounded)
        //     // {
        //     //     var  cPos = transform.TransformPoint(_collider.center + new Vector3(0, -_collider.size.y,  0) / 2);
        //     //     var tlPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y,  _collider.size.z) / 2);
        //     //     var trPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x, -_collider.size.y,  _collider.size.z) / 2);
        //     //     var blPos = transform.TransformPoint(_collider.center + new Vector3(-_collider.size.x, -_collider.size.y, -_collider.size.z) / 2);
        //     //     var brPos = transform.TransformPoint(_collider.center + new Vector3( _collider.size.x, -_collider.size.y, -_collider.size.z) / 2);
        //     //
        //     //     # region Debug Draw Gravity Rays
        //     //     Debug.DrawRay( cPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(tlPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(trPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(blPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(brPos, Vector3.down * RacerMovement.GroundCheckDist, Color.green);
        //     //     Debug.DrawRay(groundedHitInfo.point, groundedHitInfo.normal * (RacerMovement.GroundedDist + _collider.size.y / 2), Color.blue);
        //     //     #endregion
        //     //
        //     //     var pos = transform.position;
        //     //     pos.y = groundedHitInfo.point.y + RacerMovement.GroundedDist + (pos.y - cPos.y);
        //     //     transform.position = pos;
        //     //
        //     //     // Project the forward & surface normal using the dot product
        //     //     // Set the rotation w/ relative forward and up axes
        //     //     var groundNormal = groundedHitInfo.normal;
        //     //     var rotForward = forward - groundNormal * Vector3.Dot(forward, groundNormal);
        //     //     transform.rotation = Quaternion.Slerp(
        //     //         transform.rotation,
        //     //         Quaternion.LookRotation(rotForward.normalized, groundNormal),
        //     //         8f * Time.fixedDeltaTime);
        //     // }
        //     // else
        //     // {
        //     //     movement.y -= RacerMovement.GravitySpeed;
        //     // }
        //     //
        //     // // Move & rotate player
        //     // if (CurrSpeed.IsZero() == false)
        //     //     transform.Rotate(transform.up, steerDirection * Time.fixedDeltaTime);
        //     //
        //     // var newVelocity = 50f * movement * Time.fixedDeltaTime;
        //     // _rb.velocity = newVelocity;
        // }

        public override void Steer(out float steerDirection, out Vector3 movement)
        {
            // Calculate and apply steering
            var targetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached);
            var lookaheadTargetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached + 1);

            // var distToTarget = (targetCheckpoint.transform.position - transform.position).magnitude;
            // var targetTolookahead = (lookaheadTargetCheckpoint.transform.position - targetCheckpoint.transform.position);
            // if (distToTarget <= targetTolookahead.magnitude)
            // { 
            
            if (targetCheckpoint.gameObject != _target)
            {
                currTargetVariance = currLookaheadVariance;
                currLookaheadVariance = Random.Range(-ComputerRacerBehaviourData.Variance, ComputerRacerBehaviourData.Variance);
            }

            var targetPoint = Vector3.Lerp(targetCheckpoint.Loose, targetCheckpoint.Tight, ComputerRacerBehaviourData.Tightness + currTargetVariance);
            var lookaheadPoint = Vector3.Lerp(lookaheadTargetCheckpoint.Loose, lookaheadTargetCheckpoint.Tight, ComputerRacerBehaviourData.Tightness + currLookaheadVariance);
            var points = new[] { transform.position, targetPoint, lookaheadPoint };
            var smoothedPath = PathHelper.SmoothPath(points, 6);

            for (var i = 0; i < smoothedPath.Length; i++)
            {
                smoothedPath[i].y = transform.position.y;
                DebugDrawHelper.DrawBox(smoothedPath[i], new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity, Color.green);
            }

            var targetDirection = smoothedPath[1] - transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();
            Debug.DrawRay(transform.position, 4 * targetDirection, Color.green);

            // }
            // else
            // {
            //     targetDirection = targetCheckpoint.transform.position - transform.position;
            //     targetDirection.y = 0;
            //     targetDirection.Normalize();
            //     Debug.DrawRay(transform.position, 3 * targetDirection, Color.red);
            // }

            _target = targetCheckpoint.gameObject;
            _lookaheadTarget = lookaheadTargetCheckpoint.gameObject;

            var angle = Vector3.Cross(transform.forward, targetDirection);
            var dir = Vector3.Dot(angle, Vector3.up);

            Steering = dir > SteeringTolerance ? 1f : dir < -SteeringTolerance ? -1f : dir;
            
            steerDirection = Steering * RacerMovement.TurningSpeed;
            movement = transform.forward * CurrSpeed;
        }
    }
}
