using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace Environment.Scene
{
    public class CourseController : AOnSceneLoad
    {
        public int Laps = 3;
        public List<Checkpoint> Checkpoints { get; private set; }

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
