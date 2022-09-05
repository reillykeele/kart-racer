using System.Collections;
using UnityEngine;
using Util.Singleton;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentGameState { get; set; }

        // number of racers
        // positions
        // map ? (show people as percentage of the track maybe???)
        // countdown
        // timer

        public bool IsPlaying() => CurrentGameState == GameState.Playing;

        public void StartCountdown()
        {
            StartCoroutine(Countdown(3));
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


    }
}
