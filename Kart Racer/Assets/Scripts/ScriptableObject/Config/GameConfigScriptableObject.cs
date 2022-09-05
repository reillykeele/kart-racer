using Data.Item;
using UnityEngine;

namespace ScriptableObject.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Game Config", order = 1)]
    public class GameConfigScriptableObject : UnityEngine.ScriptableObject
    {
        public ItemConfigData ItemConfig;
    }
}
