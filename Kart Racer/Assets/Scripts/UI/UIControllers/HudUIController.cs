using System.Linq;
using Data.Item;
using KartRacer.Actor.Racer;
using KartRacer.Actor.Racer.Player;
using KartRacer.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.Coroutine;
using Util.Enums;
using Util.Helpers;
using Util.Systems;
using Util.UI;
using Util.UI.Controllers;

namespace UI.UIControllers
{
    public class HudUIController : UIController
    {
        public bool DisplayTimer = true;

        // Item
        private GameObject _itemUiGroup;
        private Image _itemImage;

        // Race Countdown
        private TextMeshProUGUI _countdownText;

        // Race Information
        private Image _positionImage;
        private TextMeshProUGUI _timeText;
        private TextMeshProUGUI _lapText;

        [Header("UI Pages")] 
        [SerializeField] private UIPage _pauseMenu;
        [SerializeField] private UIPage _finishRace;

        protected override void Awake()
        {
            base.Awake();

            _itemUiGroup = gameObject.GetChildObject("ItemContainer").GetChildObject("ItemGroup");
            _itemImage = _itemUiGroup.GetChildObject("ItemImage").GetComponent<Image>();

            _countdownText = gameObject.GetChildObject("CountdownText").GetComponent<TextMeshProUGUI>();

            var positionGroup = gameObject.GetChildObject("PositionContainer")?.GetChildObject("PositionGroup");
            if (positionGroup != null)
                _positionImage = positionGroup.GetChildObject("PositionImage").GetComponent<Image>();

            var textGroup = gameObject.GetChildObject("TextContainer").GetChildObject("TextGroup");
            _timeText = textGroup.GetChildObject("TimeText").GetComponent<TextMeshProUGUI>();
            _lapText = textGroup.GetChildObject("LapText").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            var racerController = FindObjectOfType<PlayerController>() ?? FindObjectOfType<RacerController>();

            GameSystem.Instance.OnPauseGameEvent.AddListener(PauseGame);
            GameSystem.Instance.OnResumeGameEvent.AddListener(ResumeGame);
            
            GameManager.Instance.RaceManager.CountdownTickEvent.AddListener(CountdownTick);
            GameManager.Instance.RaceManager.CountdownEndEvent.AddListener(CountdownEnd);

            if (racerController)
            {
                racerController.PickupItemEvent.AddListener(PickupItem);
                racerController.ChangeItemSpriteEvent.AddListener(ChangeItemSprite);
                racerController.ClearItemEvent.AddListener(ClearItem);
                racerController.ChangeLapEvent.AddListener(ChangeLap);
                racerController.FinishRaceEvent.AddListener(FinishRace);
                racerController.PositionChangeEvent.AddListener(ChangePosition);

                ChangePosition(racerController.Position);
                ChangeLap(racerController.CurrentLap);
                PickupItem(racerController.Item?.ItemData);
            }
        }

        void Update()
        {
            var raceTime = GameManager.Instance.RaceManager.RaceTime;
            if (DisplayTimer && raceTime > 0)
                _timeText.text = TimeHelper.FormatTime(raceTime);
        }

        public void PauseGame()
        {
            // _canvasAudioController.Play(CanvasAudioController.CanvasAudioSoundType.Pause);
            _canvasController.DisplayUI(_pauseMenu);
        }

        public void ResumeGame()
        {
            // _canvasAudioController.Play(CanvasAudioController.CanvasAudioSoundType.Resume);
            _canvasController.HideUI(_pauseMenu);
        }

        public void CountdownTick(int num)
        {
            _countdownText.text = "" + num;
        }

        public void CountdownEnd()
        {
            _countdownText.text = "Go!";

            StartCoroutine(CoroutineUtil.WaitForExecute(() => _countdownText.gameObject.Disable(), 1));
        }

        public void PickupItem(ItemData item)
        {
            _itemImage.sprite = item?.Icon?.FirstOrDefault() ?? GameManager.Instance.Config.UIConfig.NoneSprite;
        }

        public void ChangeItemSprite(Sprite itemSprite)
        {
            _itemImage.sprite = itemSprite;
        }

        public void ClearItem()
        {
            _itemImage.sprite = GameManager.Instance.Config.UIConfig.NoneSprite;
        }

        public void ChangePosition(int position)
        {
            if (_positionImage != null)
                _positionImage.sprite = GameManager.Instance.Config.UIConfig.PositionSprites[position];
        }

        public void ChangeLap(int lap)
        {
            _lapText.text = $"Lap {lap} / {GameManager.Instance.RaceManager.NumLaps}";
        }

        public void FinishRace(float time)
        {
            _timeText.text = TimeHelper.FormatTime(GameManager.Instance.RaceManager.RaceStartTime, Time.time);
            DisplayTimer = false;

            _canvasController.SwitchUI(_finishRace);
        }
    }
}
