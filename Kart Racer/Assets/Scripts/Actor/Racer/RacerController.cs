using Data.Item;
using Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Racer
{
    [RequireComponent(typeof(RacerMovementController))]
    public abstract class RacerController : MonoBehaviour
    {
        public string Name;
        public GUID RacerId;

        public RacerMovementController MovementController { get; private set; }

        public int Position { get; set; } = 1; // 1 through number of racers
        public int CurrentLap { get; private set; } = 1; // starting at 1
        protected int _checkpointsReached { get; set; } = 0;

        public Item.Item Item { get; set; }

        protected virtual void Awake()
        {
            RacerId = GUID.Generate();

            MovementController = GetComponent<RacerMovementController>();
        }

        public UnityEvent<ItemData> PickupItemEvent;
        public virtual void PickupItem()
        {
            // Set item based on position
            if (Item == null)
            {
                Item = GameManager.Instance.GetRandomItem();
                Item.SetOwner(this);

                PickupItemEvent.Invoke(Item.ItemData);

                Debug.Log($"{Name} picked up {Item.ItemData.Name}.");
            }
            else
            {
                Debug.Log($"{Name} already has {Item.ItemData.Name}.");
            }
        }

        // public UnityEvent UseItemEvent;
        public UnityEvent ClearItemEvent;
        public virtual void UseItem()
        {
            if (Item == null) return;

            Debug.Log($"Using {Item.ItemData.Name}");
            // UseItemEvent.Invoke();
            Item.UseItem();
            if (Item.Uses <= 0)
            {
                Item = null;
                ClearItemEvent.Invoke();
            }
        }

        public virtual void TriggerCheckpoint(int checkpointIndex)
        {
            if (_checkpointsReached == checkpointIndex - 1)
            {
                ++_checkpointsReached;
                Debug.Log($"Crossed checkpoint #{checkpointIndex}");
            }
            else
            {
                Debug.Log($"Crossed checkpoint #{checkpointIndex} but not in order. {_checkpointsReached} checkpoints reached.");
            }
        }

        public virtual void TriggerShortcut(int checkpointIndex)
        {
            if (_checkpointsReached < checkpointIndex)
            {
                _checkpointsReached = checkpointIndex;
                Debug.Log($"Crossed shortcut checkpoint #{checkpointIndex}");
            }
            else
            {
                Debug.Log($"Crossed shortcut checkpoint #{checkpointIndex} but not in order. {_checkpointsReached} checkpoints reached.");
            }
        }

        public UnityEvent<int> ChangeLapEvent;
        public UnityEvent FinishRaceEvent;
        public virtual void TriggerFinishLine()
        {
            if (_checkpointsReached >= GameManager.Instance.NumCheckpoints)
            {
                ++CurrentLap;
                _checkpointsReached = 0;

                if (CurrentLap > GameManager.Instance.NumLaps)
                {
                    Debug.Log("Race is over!");
                    FinishRaceEvent.Invoke();
                }
                else
                {
                    ChangeLapEvent.Invoke(CurrentLap);
                    Debug.Log($"Lap {CurrentLap}");
                }
            }
            else
            {
                Debug.Log("Crossed finish line but not in order.");
            }
        }
    }
}
