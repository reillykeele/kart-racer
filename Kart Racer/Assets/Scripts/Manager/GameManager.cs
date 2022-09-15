using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor.Item;
using Environment.Scene;
using ScriptableObject.Config;
using UnityEngine;
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
        // countdown
        // timer

        // Course information
        public int NumLaps { get; set; }
        public int NumCheckpoints { get; set; }

        private List<Item> _itemPool;

        private CourseController _course;

        protected override void Awake()
        {
            base.Awake();

            _itemPool = Config.ItemConfig.Items.Select(x => ItemHelper.GetItemFromData(x.ItemData)).ToList();

            _course = FindObjectOfType<CourseController>();
        }

        void Start()
        {
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

        IEnumerator Countdown(int seconds)
        {
            var count = seconds;
            while (count > 0)
            {
                Debug.Log(count);
                yield return new WaitForSeconds(1);
                --count;
            }

            // Start Event
            CurrentGameState = GameState.Playing;
        }

        public Item GetRandomItem() => _itemPool.GetRandomElement();
    }
}
