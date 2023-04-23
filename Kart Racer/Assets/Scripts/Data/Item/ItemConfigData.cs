using System;
using System.Collections.Generic;
using KartRacer.ScriptableObject.Item;

namespace Data.Item
{
    [Serializable]
    public class ItemConfigData
    {
        public float TimeToRespawn;
        public List<ItemDataScriptableObject> Items;

        public float MushroomBoostPower;
        public float MushroomBoostDuration;
    }
}
