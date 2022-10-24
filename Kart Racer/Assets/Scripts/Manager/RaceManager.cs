using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor.Item;
using Actor.Racer;
using Data.Environment;
using Environment.Scene;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util.Enums;
using Util.Helpers;
using Util.Singleton;

namespace Manager
{
    public class RaceManager : Singleton<RaceManager>
    {
        // Course information
        public CourseController CourseController { get; private set; }
        public int NumLaps { get; private set; }
        public int NumCheckpoints { get; private set; }
        public int NumKeyCheckpoints { get; private set; }
        
        // Race information
        public float RaceStartTime { get; set; }
        public float RaceTime { get; protected set; }

        // Racer information
        private List<RacerController> _racers;
        public int NumRacers => _racers.Count;

        // Preferences
        private List<Item> _itemPool;

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            // Find objects in scene
            CourseController = FindObjectOfType<CourseController>();
            
            if (CourseController != null)
            {
                NumLaps = CourseController.Laps;
                NumCheckpoints = CourseController.Checkpoints.Count();
                NumKeyCheckpoints = CourseController.Checkpoints.Count(x => x.GetCheckpointType() == CheckpointType.KeyCheckpoint);
            }
            else 
                Debug.LogWarning("No course controller found.");

            _racers = FindObjectsOfType<RacerController>().ToList();
            CalculatePositions();
        }

        protected virtual void Update()
        {
            if (GameManager.Instance.IsPlaying() == false)
                return;

            RaceTime += Time.deltaTime;
        }

        public virtual void LoadUI()
        {
            if (SceneManager.GetSceneByName("RaceGameUI").isLoaded == false)
                SceneManager.LoadSceneAsync("RaceGameUI", LoadSceneMode.Additive);
        }

        public void StartCountdown()
        {
            StartCoroutine(Countdown(GameManager.Instance.Config.CountdownLength));
        }

        public UnityEvent<int> CountdownTickEvent = new UnityEvent<int>();
        public UnityEvent CountdownEndEvent = new UnityEvent();
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
            GameManager.Instance.CurrentGameState = GameState.Playing;
        }

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

        public Item GetRandomItem() => _itemPool.GetRandomElement();

        public Vector3 GetFinishLineForward() => CourseController.GetFinishLine().transform.forward;

        public Checkpoint GetNextCheckpoint(int index) => CourseController.GetNextCheckpoint(index);
    }
}