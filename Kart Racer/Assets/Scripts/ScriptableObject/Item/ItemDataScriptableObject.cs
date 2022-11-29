using Data.Item;
using UnityEngine;

namespace ScriptableObject.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item/Item Data")]
    public class ItemDataScriptableObject : UnityEngine.ScriptableObject
    {
        public ItemData ItemData;
    }
}
