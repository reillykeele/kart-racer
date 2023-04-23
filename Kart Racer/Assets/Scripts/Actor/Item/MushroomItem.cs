using Data.Item;
using KartRacer.Manager;

namespace Actor.Item
{
    public class MushroomItem : Item
    {
        public MushroomItem(ItemData data) : base(data) { }

        public override void UseItem()
        {
            --Uses;
            _owner.MovementController.Boost(GameManager.Instance.Config.ItemConfig.MushroomBoostPower, 
                                            GameManager.Instance.Config.ItemConfig.MushroomBoostDuration);
        }
    }
}
