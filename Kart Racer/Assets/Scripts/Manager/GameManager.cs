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
        private CourseController _course;
        public int NumLaps { get; private set; }
        public int NumCheckpoints { get; private set; }

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
            _course = FindObjectOfType<CourseController>();
            _racers = FindObjectsOfType<RacerController>().ToList();

            if (_course != null)
            {
                NumLaps = _course.Laps;
                NumCheckpoints = _course.Checkpoints.Count();
            }
            else 
                Debug.LogWarning("No course controller found.");
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
            CurrentGameState = GameState.Playing;
        }

        public Item GetRandomItem() => _itemPool.GetRandomElement();
    }
}
