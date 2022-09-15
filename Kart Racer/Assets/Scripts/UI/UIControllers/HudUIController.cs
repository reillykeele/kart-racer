using Actor.Racer;
using Data.Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.Coroutine;
using Util.Helpers;

namespace UI.UIControllers
{
    public class HudUIController : UIController
    {
        // Item
        private GameObject _itemUiGroup;
        private Image _itemImage;

        private TextMeshProUGUI _countdownText;

        private Image _positionImage;
        
        private TextMeshProUGUI _lapText;

        void Awake()
        {
            _itemUiGroup = gameObject.GetChildObject("ItemGroup");
            _itemImage = _itemUiGroup.GetChildObject("ItemImage").GetComponent<Image>();

            _countdownText = gameObject.GetChildObject("CountdownText").GetComponent<TextMeshProUGUI>();

            _positionImage = gameObject.GetChildObject("PositionImage").GetComponent<Image>();

            _lapText = gameObject.GetChildObject("LapText").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            var racerController = FindObjectOfType<RacerController>();

            GameManager.Instance.CountdownTickEvent.AddListener(CountdownTick);
            GameManager.Instance.CountdownEndEvent.AddListener(CountdownEnd);

            if (racerController)
            {
                racerController.PickupItemEvent.AddListener(PickupItem);
                racerController.ClearItemEvent.AddListener(ClearItem);
                racerController.ChangeLapEvent.AddListener(ChangeLap);

                ChangePosition(racerController.Position);
                ChangeLap(racerController.CurrentLap);
            }
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
            _itemImage.sprite = item?.Icon ?? GameManager.Instance.Config.UIConfig.NoneSprite;
        }

        public void ClearItem()
        {
            _itemImage.sprite = GameManager.Instance.Config.UIConfig.NoneSprite;
        }

        public void ChangePosition(int position)
        {
            _positionImage.sprite = GameManager.Instance.Config.UIConfig.PositionSprites[position];
        }

        public void ChangeLap(int lap)
        {
            _lapText.text = $"Lap {lap} / {GameManager.Instance.NumLaps}";
        }
    }
}
