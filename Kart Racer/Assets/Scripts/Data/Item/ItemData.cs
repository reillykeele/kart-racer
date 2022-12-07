using System;
using UnityEngine;
using Util.Enums;

namespace Data.Item
{
    [Serializable]
    public class ItemData
    {
        [SerializeField] public string Name;
        [SerializeField] public Sprite[] Icon;
        [SerializeField] public ItemType ItemType;

        // range of positions you can get the item
        // frequency
        // number of uses ?
    }
}
