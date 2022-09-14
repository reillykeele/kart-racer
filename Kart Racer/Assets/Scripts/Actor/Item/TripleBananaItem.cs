using Data.Item;

namespace Actor.Item
{
    public class TripleBananaItem : Item
    {
        public TripleBananaItem(ItemData data) : base(data)
        {
            // Uses = 3
        }

        public override void UseItem()
        {
            --Uses;
        }
    }
}
