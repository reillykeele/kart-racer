using Data.Item;
using KartRacer.Manager;
using UnityEngine;

namespace Actor.Item
{
    public class TripleMushroomItem : Item
    {
        private static readonly int _maxUses = 3;

        public TripleMushroomItem(ItemData data) : base(data)
        {
            Uses = _maxUses;
        }

        public override void UseItem()
        {
            --Uses;
            _owner.MovementController.Boost(GameManager.Instance.Config.ItemConfig.MushroomBoostPower, 
                                            GameManager.Instance.Config.ItemConfig.MushroomBoostDuration);

            var currUse = Mathf.Clamp(_maxUses - Uses, 0, _maxUses - 1);
            _owner.ChangeItemSpriteEvent.Invoke(ItemData.Icon[currUse]);
        }
    }
}
