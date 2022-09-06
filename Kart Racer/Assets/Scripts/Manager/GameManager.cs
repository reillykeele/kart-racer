using System.Collections;
using Data.Item;
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

        public ItemData GetRandomItem() => Config.ItemConfig.Items.GetRandomElement().Item;
    }
}
