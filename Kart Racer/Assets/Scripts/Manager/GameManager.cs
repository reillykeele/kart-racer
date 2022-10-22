using ScriptableObject.Config;
using UnityEngine;
using Util.Enums;
using Util.Singleton;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public GameConfigScriptableObject Config;
        public GameState CurrentGameState;

        // Game mode  
        public GameMode GameMode = GameMode.Free;
        public RaceManager RaceManager;

        public int NumComputerPlayers = 0;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(transform.root.gameObject);
        }

        public bool IsPlaying() => CurrentGameState == GameState.Playing;

        // public void StartCountdown()
        // {
        //     // StartCoroutine(Countdown(Config.CountdownLength));
        // }
        //
        // public UnityEvent<int> CountdownTickEvent;
        // public UnityEvent CountdownEndEvent;
        // IEnumerator Countdown(int seconds)
        // {
        //     var count = seconds + 1;
        //     while (count > 0)
        //     {
        //         Debug.Log(count);
        //         yield return new WaitForSeconds(1);
        //         
        //         --count;
        //         CountdownTickEvent.Invoke(count);
        //     }
        //
        //     // Start Event
        //     CountdownEndEvent.Invoke();
        //     RaceManager.RaceStartTime = Time.time;
        //     CurrentGameState = GameState.Playing;
        // }

        public void InitRace()
        {
            SpawnRaceManager();
        }

        public void SpawnRaceManager()
        {
            var raceManagerGameObject = new GameObject("Race Manager");

            switch(GameMode)
            {
                case GameMode.None:
                case GameMode.Free:
                case GameMode.Race:
                    RaceManager = raceManagerGameObject.AddComponent<RaceManager>();
                    break;
                case GameMode.TimeTrial:
                    RaceManager = raceManagerGameObject.AddComponent<TimeTrialManager>();
                    break;
            }
        }
    }
}
