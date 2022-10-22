using Actor.Item;
using Data.Item;
using Util.Enums;

namespace Util.Helpers
{
    public static class ItemHelper
    {
        public static Item GetItemFromData(ItemData data)
        {
            switch (data.ItemType)
            {
                case ItemType.Mushroom:
                    return new MushroomItem(data);
                
                default:
                    return null;
            }
        }
    }
}
