using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class LoadSceneNewGame : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 0)
            {
                HumanResourceManager HRM = HumanResourceManager.Instance;
                GameManager GM = GameManager.Instance;
                HRM.OnSpawnPeople(GM.villagers_start, GM.workers_start);
                Destroy(transform.gameObject);
            }
            
            Destroy(gameObject);
        }
    }
}