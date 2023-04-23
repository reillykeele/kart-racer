using System;
using Data.Item;
using Data.Racer.Computer;
using KartRacer.Manager;
using KartRacer.ScriptableObject.Racer.Computer;
using UnityEngine;
using Util.Helpers;
using Random = UnityEngine.Random;

namespace KartRacer.Actor.Racer.Computer
{
    [RequireComponent(typeof(ComputerController))]
    public class ComputerMovementController : RacerMovementController
    {
        private ComputerController _computerController;

        [SerializeField] private ComputerRacerBehaviourScriptableObject _computerRacerBehaviourScriptableObject;
        private ComputerRacerBehaviourData ComputerRacerBehaviourData;

        private Vector3 _currTargetPoint;
        private Vector3 _currLookaheadTargetPoint;
        private float _currTargetVariance = 0f;
        private float _currLookaheadVariance = 0f;
        private float _currTargetAngle = 0f;
        private float _currTargetDistance = 0f;

        protected override bool IsAccelerating => true;
        protected override bool IsBraking => false;

        protected override void Awake()
        {
            base.Awake();

            _computerController = GetComponent<ComputerController>();
            ComputerRacerBehaviourData = _computerRacerBehaviourScriptableObject.ComputerRacerBehaviourData;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_computerController.HasItem)
                TryUseItem();
        }

        public override void Steer(out float steerDirection, out Vector3 movement)
        {
            // Calculate and apply steering
            var targetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached);
            var lookaheadTargetCheckpoint = GameManager.Instance.RaceManager.GetNextCheckpoint(_racerController.CheckpointsReached + 1);

            if (targetCheckpoint.gameObject != _target)
            {
                _currTargetVariance = _currLookaheadVariance;
                _currLookaheadVariance = Random.Range(-ComputerRacerBehaviourData.Variance, ComputerRacerBehaviourData.Variance);

                _currTargetPoint = Vector3.Lerp(targetCheckpoint.Loose, targetCheckpoint.Tight, ComputerRacerBehaviourData.Tightness + _currTargetVariance);
                _currLookaheadTargetPoint = Vector3.Lerp(lookaheadTargetCheckpoint.Loose, lookaheadTargetCheckpoint.Tight, ComputerRacerBehaviourData.Tightness + _currLookaheadVariance);
            }

            var points = new[] { transform.position, _currTargetPoint, _currLookaheadTargetPoint };
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

            _target = targetCheckpoint.gameObject;
            _lookaheadTarget = lookaheadTargetCheckpoint.gameObject;

            var angle = Vector3.Cross(transform.forward, targetDirection);
            var dir = Vector3.Dot(angle, Vector3.up);
            _currTargetAngle = dir;
            _currTargetDistance = MathHelper.Distance2(transform.position, _currTargetPoint);

            Steering = dir > SteeringTolerance ? 1f : dir < -SteeringTolerance ? -1f : dir;
            
            steerDirection = Steering * RacerMovement.TurningSpeed;
            movement = transform.forward * CurrSpeed;
        }

        private void TryUseItem()
        {
            if (Random.value > ComputerRacerBehaviourData.ItemSpamminess)
                return;

            var itemType = _computerController.Item.ItemData.ItemType;
            switch (itemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Mushroom:
                    TryUseMushroom();
                    break;
                case ItemType.TripleMushroom:
                    TryUseMushroom();
                    break;
                case ItemType.Banana:
                    break;
                case ItemType.TripleBanana:
                    break;
                case ItemType.GreenShell:
                    break;
                case ItemType.RedShell:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
        }

        private void TryUseMushroom()
        {
            if (!IsBoosting && Mathf.Abs(_currTargetAngle) < 20 && _currTargetDistance > 10f)
            {
                _computerController.UseItem();
            }
        }
    }
}
