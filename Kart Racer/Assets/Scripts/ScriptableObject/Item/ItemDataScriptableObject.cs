using Data.Item;
using UnityEngine;

namespace ScriptableObject.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 2)]
    public class ItemDataScriptableObject : UnityEngine.ScriptableObject
    {
        public ItemData ItemData;
    }
}
