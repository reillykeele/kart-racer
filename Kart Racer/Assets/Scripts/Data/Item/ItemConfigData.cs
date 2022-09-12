using System;
using System.Collections.Generic;
using ScriptableObject.Item;

namespace Data.Item
{
    [Serializable]
    public class ItemConfigData
    {
        public float TimeToRespawn;
        public List<ItemDataScriptableObject> Items;
    }
}
