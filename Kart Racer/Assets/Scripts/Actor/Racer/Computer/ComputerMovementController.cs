using Data.Environment;
using Data.Racer.Computer;
using Environment.Track;
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

        private bool isAccelerating = true;
        private bool isBraking = false;

        protected override void Awake()
        {
            base.Awake();

            _computerController = GetComponent<ComputerController>();
            ComputerRacerBehaviourData = _computerRacerBehaviourScriptableObject.ComputerRacerBehaviourData;
        }

        protected override void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            if (UseAutopilot)
            {
                Autopilot();
                return;
            }

            var isGrounded = IsGrounded(out var groundHitInfo);

            // Check the surface we are on 
            var trackSurfaceModifier = new TrackSurfaceModifierData();
            if (isGrounded)
            {
                var trackSurface = groundHitInfo.collider.GetComponent<TrackSurface>();
                if (trackSurface != null)
                    trackSurfaceModifier = trackSurface.TrackSurfaceModifierData;
            }

            CurrSpeed = CalculateSpeed(CurrSpeed, isAccelerating, isBraking, trackSurfaceModifier);

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

            var forward = transform.forward;

            var angle = Vector3.Cross(transform.forward, targetDirection);
            var dir = Vector3.Dot(angle, Vector3.up);

            Steering = dir > SteeringTolerance ? 1f : dir < -SteeringTolerance ? -1f : dir;
            var steeringDir = Steering * RacerMovement.TurningSpeed;
            
            if (!CurrSpeed.IsZero())
                transform.Rotate(transform.up, steeringDir * Time.fixedDeltaTime);

            Debug.DrawRay(transform.position, 3 * forward, Color.yellow);
            var movement = forward * CurrSpeed * Time.fixedDeltaTime;

            // Apply gravity
            var pos = transform.position;
            if (isGrounded)
            {
                pos.y = groundHitInfo.point.y + 0.5f; // TODO: Change to some "dist to ground" var

                var groundNormal = groundHitInfo.normal;
                var forwardDirection = forward.normalized;
                
                // Project the forward & surface normal using the dot product
                // Set the rotation w/ relative forward and up axes
                var rotForward = forwardDirection - groundNormal * Vector3.Dot (forwardDirection, groundNormal);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation(rotForward.normalized, groundNormal), 
                    2f * Time.fixedDeltaTime);
            }
            else
                movement.y -= RacerMovement.GravitySpeed;

            // Move player
            transform.position = pos + movement;
        }
    }
}
