using Data.Item;

namespace Actor.Item
{
    public class GreenShell : Item
    {
        public GreenShell(ItemData data) : base(data) { }

        public override void UseItem()
        {
            --Uses;
        }
    }
}
