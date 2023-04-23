using System;
using Data.Item;
using KartRacer.Actor.Item;
using KartRacer.Actor.Racer;
using Util.Enums;

namespace Actor.Item
{
    [Serializable]
    public abstract class Item
    {
        public static Item CreateItem(ItemData data)
        {
            switch (data.ItemType)
            {
                case ItemType.Mushroom:
                    return new MushroomItem(data);
                case ItemType.TripleMushroom:
                    return new TripleMushroomItem(data);
                case ItemType.Banana:
                    return new BananaItem(data);
                case ItemType.TripleBanana:
                    return new TripleBananaItem(data);
                case ItemType.GreenShell:
                    return new GreenShell(data);
                case ItemType.RedShell:
                    return new RedShellItem(data);
            }

            return null;
        }

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
