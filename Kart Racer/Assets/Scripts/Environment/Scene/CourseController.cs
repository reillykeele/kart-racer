using System.Collections.Generic;
using System.Linq;
using Manager;

namespace Environment.Scene
{
    public class CourseController : AOnSceneLoad
    {
        public int Laps = 3;
        public List<Checkpoint> Checkpoints { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Checkpoints = FindObjectsOfType<Checkpoint>().ToList();
        }

        protected override void OnSceneLoad()
        {
            GameManager.Instance.StartCountdown();
        }
    }
}
