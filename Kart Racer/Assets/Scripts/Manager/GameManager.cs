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
