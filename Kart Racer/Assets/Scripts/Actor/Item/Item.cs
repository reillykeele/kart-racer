using System;
using Actor.Racer;
using Actor.Racer.Player;
using Data.Item;

namespace Actor.Item
{
    [Serializable]
    public abstract class Item
    {
        public ItemData ItemData;
        public int Uses = 1;

        protected RacerController _owner;

        protected Item(ItemData data)
        {
            ItemData = data;
        }

        public void SetOwner(RacerController owner) => _owner = owner;

        public abstract void UseItem();
    }
}
