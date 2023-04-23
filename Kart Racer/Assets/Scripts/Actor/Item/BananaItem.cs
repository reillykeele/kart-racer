using Data.Item;

namespace KartRacer.Actor.Item
{
    public class BananaItem : global::Actor.Item.Item
    {
        public BananaItem(ItemData data) : base(data) { }

        public override void UseItem()
        {
            --Uses;
        }
    }
}
