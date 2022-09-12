using Data.Item;

namespace Actor.Item
{
    public class MushroomItem : Item
    {
        public MushroomItem(ItemData data) : base(data) { }

        public override void UseItem()
        {
            --Uses;
            _owner.MovementController.Boost();
        }
    }
}
