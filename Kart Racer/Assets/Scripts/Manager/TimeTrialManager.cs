using System.Linq;
using Actor.Item;
using Data.Item;
using KartRacer.Actor.Racer.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Helpers;
using Util.Systems;

namespace KartRacer.Manager
{
    public class TimeTrialManager : RaceManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void InitRace()
        {
            // Spawn in Player Racer
            var player = FindObjectOfType<PlayerController>();
            if (player == null)
            {
                var actors = GameObject.Find("Actors");
                var playerGameObject = Instantiate(CourseController.PlayerRacers.First(), actors.transform);
                player = playerGameObject.GetComponent<PlayerController>();
            }

            player.transform.position = CourseController.TimeTrialSpawn;
            player.Item = new TripleMushroomItem(GameManager.Instance.Config.ItemConfig.Items
                .SingleOrDefault(x => x.ItemData.ItemType == ItemType.TripleMushroom)?.ItemData);

            CourseController.ItemBoxes.ForEach(x => x.gameObject.Disable());
        }

        public override void LoadUI()
        {
            if (SceneManager.GetSceneByName("TimeTrialGameUI").isLoaded == false)
                SceneManager.LoadScene("TimeTrialGameUI", LoadSceneMode.Additive);
        }
    }
}