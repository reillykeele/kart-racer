using Data.Item;
using UnityEngine;

namespace ScriptableObject.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 2)]
    public class ItemScriptableObject : UnityEngine.ScriptableObject
    {
        public ItemData Item;
    }
}
