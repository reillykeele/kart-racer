using Data.Config;
using Data.Item;
using UnityEngine;

namespace KartRacer.ScriptableObject.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Configuration/Game Config")]
    public class GameConfigScriptableObject : UnityEngine.ScriptableObject
    {
        public int CountdownLength = 3;
        public ItemConfigData ItemConfig;
        public UIConfigData UIConfig;
    }
}
