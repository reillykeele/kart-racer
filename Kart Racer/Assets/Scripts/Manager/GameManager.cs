using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor.Item;
using ScriptableObject.Config;
using UnityEngine;
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

        private List<Item> _itemPool;

        protected override void Awake()
        {
            base.Awake();

            _itemPool = Config.ItemConfig.Items.Select(x => ItemHelper.GetItemFromData(x.ItemData)).ToList();
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
