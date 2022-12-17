using ScriptableObject.Config;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util.Enums;
using Util.Singleton;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public GameConfigScriptableObject Config;

        // Game state
        public UnityEvent OnPauseGameEvent;
        public UnityEvent OnResumeGameEvent;
        private GameState _currentGameState;
        public GameState CurrentGameState
        {
            get => _currentGameState;
            set => _currentGameState = value;
        }

        // Game mode  
        public GameMode GameMode = GameMode.Free;
        public RaceManager RaceManager;

        public int NumComputerPlayers = 3;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(transform.root.gameObject);

            if (FindObjectOfType<LoadingManager>() == null)
                SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        }

        public bool IsPlaying() => CurrentGameState == GameState.Playing;
        public bool IsPaused() => CurrentGameState == GameState.Paused;

        public void PauseGame()
        {
            if (CurrentGameState != GameState.Playing) return;

            Debug.Log("resume game");

            CurrentGameState = GameState.Paused;
            OnPauseGameEvent.Invoke();

            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            if (CurrentGameState != GameState.Paused) return;

            Debug.Log("resume game");

            CurrentGameState = GameState.Playing;
            OnResumeGameEvent.Invoke();

            Time.timeScale = 1;
        }

        public void TogglePaused() { if (IsPlaying()) PauseGame(); else ResumeGame(); }

        public void InitRace()
        {
            CurrentGameState = GameState.Cutscene;

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
