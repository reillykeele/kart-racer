using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor.Item;
using Actor.Racer;
using Environment.Scene;
using ScriptableObject.Config;
using UnityEngine;
using UnityEngine.Events;
using Util.Enums;
using Util.Helpers;
using Util.Singleton;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public GameConfigScriptableObject Config;
        public GameState CurrentGameState;

        // number of racers
        // positions
        // map ? (show people as percentage of the track maybe???)

        // Course information
        public CourseController CourseController;
        public int NumLaps { get; private set; }
        public int NumCheckpoints { get; private set; }
        public float RaceStartTime { get; private set; }

        // Racer information
        private List<RacerController> _racers;
        public int NumRacers => _racers.Count;

        private List<Item> _itemPool;

        protected override void Awake()
        {
            base.Awake();

            _itemPool = Config.ItemConfig.Items.Select(x => ItemHelper.GetItemFromData(x.ItemData)).ToList();
        }

        void Start()
        {
            // Find objects in scene
            CourseController = FindObjectOfType<CourseController>();
            _racers = FindObjectsOfType<RacerController>().ToList();

            if (CourseController != null)
            {
                NumLaps = CourseController.Laps;
                NumCheckpoints = CourseController.Checkpoints.Count();
            }
            else 
                Debug.LogWarning("No course controller found.");

            CalculatePositions();
        }

        public bool IsPlaying() => CurrentGameState == GameState.Playing;

        public void StartCountdown()
        {
            StartCoroutine(Countdown(Config.CountdownLength));
        }

        public UnityEvent<int> CountdownTickEvent;
        public UnityEvent CountdownEndEvent;
        IEnumerator Countdown(int seconds)
        {
            var count = seconds + 1;
            while (count > 0)
            {
                Debug.Log(count);
                yield return new WaitForSeconds(1);
                
                --count;
                CountdownTickEvent.Invoke(count);
            }

            // Start Event
            CountdownEndEvent.Invoke();
            RaceStartTime = Time.time;
            CurrentGameState = GameState.Playing;
        }

        public Item GetRandomItem() => _itemPool.GetRandomElement();

        public void CalculatePositions()
        {
            var i = 1;
            foreach (var racer in _racers
                         .OrderByDescending(x => x.CurrentLap)
                         .ThenByDescending(x => x.CheckpointsReached)
                         .ThenBy(x => MathHelper.Distance2(
                             x.transform.position,
                             CourseController.GetNextPositionCheckpoint(x.CheckpointsReached))))
            {
                racer.Position = i++;
            }
        }
    }
}
