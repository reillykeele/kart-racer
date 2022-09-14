using Data.Item;

namespace Actor.Item
{
    public class BananaItem : Item
    {
        public BananaItem(ItemData data) : base(data) { }

        public override void UseItem()
        {
            --Uses;
        }
    }
}
