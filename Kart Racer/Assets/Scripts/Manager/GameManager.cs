using KartRacer.Data.Config;
using KartRacer.ScriptableObject.Config;
using UnityEngine;
using Util.Enums;
using Util.Singleton;
using Util.Systems;

namespace KartRacer.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Configuration")]
        public GameConfigScriptableObject Config;

        [Header("Game Mode")]
        public GameMode GameMode = GameMode.Free;
        public RaceManager RaceManager;

        public int NumComputerPlayers = 3;

        // Layers
        public static int RacerLayer => LayerMask.NameToLayer("Racer");
        public static int TrackLayer => LayerMask.NameToLayer("Track");
        public static int WallsLayer => LayerMask.NameToLayer("Walls");
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(transform.root.gameObject);
        }
        public void InitRace()
        {
            GameSystem.Instance.ChangeGameState(GameState.Cutscene);

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
