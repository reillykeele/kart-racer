using Data.Item;
using Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Util.Helpers;

namespace Actor.Racer
{
    [RequireComponent(typeof(RacerMovementController))]
    public abstract class RacerController : MonoBehaviour
    {
        public string Name;
        public GUID RacerId;

        public RacerMovementController MovementController { get; private set; }

        public UnityEvent<int> PositionChangeEvent;
        private int _position = 1;
        public int Position
        {
            get => _position;
            set
            {
                _position = value;
                PositionChangeEvent.Invoke(_position);
            }
        }

        public int CurrentLap { get; private set; } = 1; // starting at 1
        public int CheckpointsReached { get; set; } = 0;
        public int KeyCheckpointsReached { get; set; } = 0;

        public float RaceFinishTime { get; private set; }

        private Item.Item _item;
        public Item.Item Item
        {
            get => _item;
            set
            {
                Debug.Log("SETTING ITEM");
                _item = value;

                if (_item == null)
                    ClearItemEvent.Invoke();
                else
                {
                    _item.SetOwner(this);
                    PickupItemEvent.Invoke(_item.ItemData);
                }
            }
        }

        protected Vector3 CourseForward { get; set; }
        protected GameObject CoursePlane { get; private set; }

        protected virtual void Awake()
        {
            RacerId = GUID.Generate();

            MovementController = GetComponent<RacerMovementController>();
            CoursePlane = gameObject.GetChildObject("PositionPlane");
        }

        protected virtual void Start()
        {
            CourseForward = GameManager.Instance.RaceManager.GetFinishLineForward();
        }

        protected virtual void LateUpdate()
        {
            CoursePlane.transform.forward = CourseForward;
        }

        public UnityEvent<ItemData> PickupItemEvent;
        public virtual void PickupItem()
        {
            // Set item based on position
            if (Item == null)
            {
                Item = GameManager.Instance.RaceManager.GetRandomItem();

                // Debug.Log($"{Name} picked up {Item.ItemData.Name}.");
            }
            else
            {
                // Debug.Log($"{Name} already has {Item.ItemData.Name}.");
            }
        }

        // public UnityEvent UseItemEvent;
        public UnityEvent ClearItemEvent;
        public virtual void UseItem()
        {
            if (Item == null) return;

            // Debug.Log($"Using {Item.ItemData.Name}");
            // UseItemEvent.Invoke();
            Debug.Log($"USE ITEM {Item.Uses}");
            Item.UseItem();
            if (Item.Uses <= 0)
            {
                Item = null;
            }
        }

        public virtual void TriggerCheckpoint(int checkpointIndex,Vector3 courseForward)
        {
            if (CheckpointsReached == checkpointIndex - 1)
            {
                ++CheckpointsReached;
                // Debug.Log($"Crossed checkpoint #{checkpointIndex}");

                CourseForward = courseForward;
            }
            else
            {
                // Debug.Log($"Crossed checkpoint #{checkpointIndex} but not in order. {CheckpointsReached} checkpoints reached.");
            }
        }

        public virtual void TriggerKeyCheckpoint(int checkpointIndex, int keyCheckpointIndex, Vector3 courseForward)
        {
            if (KeyCheckpointsReached == keyCheckpointIndex - 1)
                ++KeyCheckpointsReached;

            if (CheckpointsReached < checkpointIndex)
                CheckpointsReached = checkpointIndex;
        }

        public virtual void TriggerShortcut(int checkpointIndex)
        {
            if (CheckpointsReached < checkpointIndex)
            {
                CheckpointsReached = checkpointIndex;
                // Debug.Log($"Crossed shortcut checkpoint #{checkpointIndex}");
            }
            else
            {
                // Debug.Log($"Crossed shortcut checkpoint #{checkpointIndex} but not in order. {CheckpointsReached} checkpoints reached.");
            }
        }

        public UnityEvent<int> ChangeLapEvent;
        public UnityEvent<float> FinishRaceEvent;
        public virtual void TriggerFinishLine()
        {
            Debug.Log("Num Key Checkpoints: " + GameManager.Instance.RaceManager.NumKeyCheckpoints +
                      ", Key Checkpoints Reached: " + KeyCheckpointsReached);
            if (KeyCheckpointsReached >= GameManager.Instance.RaceManager.NumKeyCheckpoints)
            {
                ++CurrentLap;
                CheckpointsReached = 0;

                if (CurrentLap > GameManager.Instance.RaceManager.NumLaps)
                { 
                    Debug.Log($"{name} finished the race!");
                    RaceFinishTime = Time.time;
                    FinishRaceEvent.Invoke(RaceFinishTime);
                }
                else
                {
                    ChangeLapEvent.Invoke(CurrentLap);
                    Debug.Log($"{name} advances to lap #{CurrentLap}");
                }
            }
            else
            {
                // Debug.Log("Crossed finish line but not in order.");
            }
        }
    }
}
