using Actor.Racer;
using Data.Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.UIControllers
{
    public class HudUIController : UIController
    {
        // Item
        private GameObject _itemUiGroup;
        private Image _itemImage;

        private TextMeshProUGUI _lapText;

        void Awake()
        {
            _itemUiGroup = gameObject.GetChildObject("ItemGroup");
            _itemImage = _itemUiGroup.GetChildObject("ItemImage").GetComponent<Image>();

            _lapText = gameObject.GetChildObject("LapText").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            var racerController = FindObjectOfType<RacerController>();

            if (racerController)
            {
                racerController.PickupItemEvent.AddListener(PickupItem);
                racerController.ClearItemEvent.AddListener(ClearItem);
                racerController.ChangeLapEvent.AddListener(ChangeLap);
            }
        }

        public void PickupItem(ItemData item)
        {
            _itemImage.sprite = item.Icon;
        }

        public void ClearItem()
        {
            _itemImage.sprite = GameManager.Instance.Config.UIConfig.NoneSprite;
        }

        public void ChangePosition(int position)
        {
            
        }

        public void ChangeLap(int lap)
        {
            _lapText.text = "" + lap;
        }
    }
}
