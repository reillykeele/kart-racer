using Data.Item;

namespace Actor.Item
{
    public class RedShellItem : Item
    {
        public RedShellItem(ItemData data) : base(data) { }

        public override void UseItem()
        {
            --Uses;
            _owner.MovementController.Boost();
        }
    }
}
