using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class GameAppFlowManager : MonoBehaviour
    {
        protected static bool IsSceneOptionsLoaded;

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void RandomLoadScene(int r)
        {
            r = Random.Range(1, 15);
            Debug.LogWarning("RRRRR : " + r);
            if (r >= 1 && r <= 5)
            {
                SceneManager.LoadScene("map2", LoadSceneMode.Single);
            }
            if (r >= 6 && r <= 10)
            {
                SceneManager.LoadScene("map3", LoadSceneMode.Single);
            }
            if (r >= 11 && r <= 15)
            {
                SceneManager.LoadScene("map4", LoadSceneMode.Single);
            }
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        public void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void LoadOptionsScene(string optionSceneName)
        {
            if (!IsSceneOptionsLoaded)
            {
                SceneManager.LoadScene(optionSceneName, LoadSceneMode.Additive);
                IsSceneOptionsLoaded = true;
                Debug.Log("true");
            }   
        }

        public void UnloadOptionsScene(string optionSceneName)
        {
            if (IsSceneOptionsLoaded)
            {
                SceneManager.UnloadSceneAsync(optionSceneName);
                IsSceneOptionsLoaded = false;
                Debug.Log("false");
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        #region Scene Load and Unload Events Handler

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneUnloadEventHandler;
            SceneManager.sceneLoaded += SceneLoadedEventHandler;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneUnloadEventHandler;
            SceneManager.sceneLoaded -= SceneLoadedEventHandler;
        }

        private void SceneUnloadEventHandler(Scene scene)
        {

        }

        private void SceneLoadedEventHandler(Scene scene, LoadSceneMode mode)
        {
            //If the loaded scene is not the SceneOptions, set flag IsOptionsLoaded to false
            //
            if (scene.name.CompareTo("SceneOptions") != 0)
            {
                IsSceneOptionsLoaded = false;
            }
        }

        #endregion
    }
}