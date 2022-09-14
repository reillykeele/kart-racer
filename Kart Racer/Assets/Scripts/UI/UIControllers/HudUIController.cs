using Actor.Racer;
using Data.Item;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.UIControllers
{
    public class HudUIController : UIController
    {
        private GameObject _itemUiGroup;
        private Image _itemImage;

        void Awake()
        {
            _itemUiGroup = gameObject.GetChildObject("ItemGroup");
            _itemImage = _itemUiGroup.GetChildObject("ItemImage").GetComponent<Image>();
        }

        void Start()
        {
            var racerController = FindObjectOfType<RacerController>();

            if (racerController)
            {
                racerController.PickupItemEvent.AddListener(PickupItem);
                racerController.ClearItemEvent.AddListener(ClearItem);
            }
        }

        public void PickupItem(ItemData item)
        {
            Debug.Log("Pickup Item UI Controller");
            _itemImage.sprite = item.Icon;
        }

        public void ClearItem()
        {
            Debug.Log("Clear Item UI Controller");
            _itemImage.sprite = GameManager.Instance.Config.UIConfig.NoneSprite;
        }
    }
}
