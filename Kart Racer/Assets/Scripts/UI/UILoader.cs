using System.Collections.Generic;
using UnityEngine;
using Scene = Util.Enums.Scene;

namespace UI
{
    public class UILoader : MonoBehaviour
    {
        public List<Util.Enums.UIPageType> UIToLoad;
        public List<Scene> ScenesToLoad;

        void Start()
        {
            // foreach (var ui in UIToLoad)
            // {
            //     if (SceneManager.GetSceneByName(ui.ToString()).isLoaded == false)
            //         SceneManager.LoadSceneAsync(ui.ToString(), LoadSceneMode.Additive);
            // }
            //
            // foreach (var scene in ScenesToLoad)
            // {
            //     if (SceneManager.GetSceneByName(scene.ToString()).isLoaded == false)
            //         SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
            // }
        }
    }
}
