using System.Collections.Generic;
using System.Linq;
using Data.Audio;
using Manager;
using ScriptableObject.Audio;
using UnityEngine;

namespace Environment.Scene
{
    public class CourseController : AOnSceneLoad
    {
        public int Laps = 3;
        public List<Checkpoint> Checkpoints { get; private set; }

        public bool DrawCheckPointPath = true;

        void OnDrawGizmos()
        {
            if (DrawCheckPointPath == false) return;

            var checkpoints = GetComponentsInChildren<Checkpoint>();

            for (var i = 0; i < checkpoints.Length; ++i)
            {
                Vector3 prev = Vector3.zero;
                Vector3 curr = checkpoints[i].transform.position;

                if (i > 0)
                    prev = checkpoints[i - 1].transform.position;
                else if (i == 0 && checkpoints.Length > 1)
                    prev = checkpoints.Last().transform.position;

                Gizmos.DrawLine(prev, curr);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.InitRace();

            Checkpoints = FindObjectsOfType<Checkpoint>().ToList();
        }

        protected override void OnSceneLoad()
        {
            GameManager.Instance.RaceManager.StartCountdown();
        }

        void Start()
        {
            GameManager.Instance.RaceManager.LoadUI();
        }

        public Checkpoint GetFinishLine() => Checkpoints.Single(x => x.CheckpointIndex == 0);
        public Checkpoint GetCheckpoint(int index) => Checkpoints.Single(x => x.CheckpointIndex == index);
        public Checkpoint GetNextCheckpoint(int index) => 
            index + 1 >= Checkpoints.Count ? 
            GetFinishLine() :
            GetCheckpoint(index + 1);
        public Vector3 GetNextPositionCheckpoint(int index) => GetNextCheckpoint(index).transform.position;
    }
}
