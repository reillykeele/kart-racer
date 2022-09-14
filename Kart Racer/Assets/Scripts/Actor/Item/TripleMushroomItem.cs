using Data.Item;

namespace Actor.Item
{
    public class TripleMushroomItem : Item
    {
        public TripleMushroomItem(ItemData data) : base(data)
        {
            Uses = 3;
        }

        public override void UseItem()
        {

            --Uses;
            _owner.MovementController.Boost();
        }
    }
}
