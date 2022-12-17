using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace Environment.Scene
{
    public class CourseController : AOnSceneLoad
    {
        [Header("Course Properties")]
        public string CourseName = "";
        public int Laps = 3;
        public GameObject[] PlayerRacers;

        [Header("Race Configuration")] 
        public Vector3[] RacerSpawns;
        public GameObject[] ComputerRacers;

        [Header("Time Trial Configuration")] 
        public Vector3 TimeTrialSpawn;

        public List<Checkpoint> Checkpoints { get; private set; }

        [Header("Debug")]
        public bool DrawCheckPointPath = true;
        public Color CheckPointPathColor = Color.white;

        public bool DrawTightCheckPointPath = true;
        public Color TightCheckPointPathColor = Color.white;

        public bool DrawLooseCheckPointPath = true;
        public Color LooseCheckPointPathColor = Color.white;


        [HideInInspector] public CourseAudioController CourseAudioController;

        void OnDrawGizmos()
        {
            var checkpoints = GetComponentsInChildren<Checkpoint>();
            for (var i = 0; i < checkpoints.Length; ++i)
            {
                var prevCheckpoint = i > 0 ? checkpoints[i - 1] : checkpoints.Last();
                var currCheckpoint = checkpoints[i];

                // Draw the tight line
                if (DrawTightCheckPointPath)
                {
                    Gizmos.color = TightCheckPointPathColor;
                    Gizmos.DrawLine(prevCheckpoint.Tight, currCheckpoint.Tight);
                }

                // Draw the tight line
                if (DrawLooseCheckPointPath)
                {
                    Gizmos.color = LooseCheckPointPathColor;
                    Gizmos.DrawLine(prevCheckpoint.Loose, currCheckpoint.Loose);
                }

                
                // Draw the default line
                if (DrawCheckPointPath)
                {
                    Gizmos.color = CheckPointPathColor;
                    Gizmos.DrawLine(prevCheckpoint.transform.position, currCheckpoint.transform.position);
                }
            }

            // if (DrawSmoothedCheckpointPath)
            // {
            //     var points = checkpoints.Select(x => x.transform.position).ToArray();
            //     var smoothedPath = PathHelper.SmoothPath(points, checkpoints.Length);
            //     Debug.Log(smoothedPath.Length);
            //     for (var i = 0; i < smoothedPath.Length; ++i)
            //     {
            //         var prev = i > 0 ? smoothedPath[i - 1] : smoothedPath.Last();
            //         var curr = smoothedPath[i];
            //
            //         Gizmos.DrawLine(prev, curr);
            //     }
            // }
        }

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.InitRace();

            Checkpoints = FindObjectsOfType<Checkpoint>().ToList();
        }

        void Start()
        {
            CourseAudioController = GetComponent<CourseAudioController>();

            GameManager.Instance.RaceManager.LoadUI();
        }

        protected override void OnSceneLoad()
        {
            GameManager.Instance.RaceManager.StartCountdown();
        }

        public Checkpoint GetFinishLine() => Checkpoints.Single(x => x.CheckpointIndex == 0);
        public Checkpoint GetCheckpoint(int index) => Checkpoints.Single(x => x.CheckpointIndex == index);
        
        public Checkpoint GetNextCheckpoint(int index) =>
            GetCheckpoint((index + 1) % (Checkpoints.Count - 1));
            // index + 1 >= Checkpoints.Count ? 
            // GetFinishLine() :
            // GetCheckpoint(index + 1);
        public Vector3 GetNextPositionCheckpoint(int index) => GetNextCheckpoint(index).transform.position;
    }
}
