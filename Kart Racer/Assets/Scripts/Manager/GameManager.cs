using ScriptableObject.Config;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util.Enums;
using Util.Singleton;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Configuration")]
        public GameConfigScriptableObject Config;

        [Header("Game State")]
        public UnityEvent OnPauseGameEvent;
        public UnityEvent OnResumeGameEvent;
        private GameState _currentGameState;
        public GameState CurrentGameState
        {
            get => _currentGameState;
            set => _currentGameState = value;
        }

        [Header("Game Mode")]
        public GameMode GameMode = GameMode.Free;
        public RaceManager RaceManager;

        public int NumComputerPlayers = 3;

        // Layers
        public static int RacerLayer => LayerMask.NameToLayer("Racer");
        public static int TrackLayer => LayerMask.NameToLayer("Track");
        public static int WallsLayer => LayerMask.NameToLayer("Walls");

        // Audio
        // TODO: Should this be elsewhere?
        [Header("Audio")]
        public AudioMixer Mixer;

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

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
                Application.Quit(0);
#endif
        }
    }
}
