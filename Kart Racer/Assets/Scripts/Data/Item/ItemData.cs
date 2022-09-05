using System;
using UnityEngine;
using Util.Enums;

namespace Data.Item
{
    [Serializable]
    public class ItemData
    {
        public string Name;

        public Sprite Icon;

        public ItemType ItemType;
        // range of positions you can get the item
        // frequency
        // number of uses ?
    }
}
