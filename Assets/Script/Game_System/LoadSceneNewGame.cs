using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class LoadSceneNewGame : MonoBehaviour
    {
        private GameManager GM;
        private GameInstance GI;

        public GameManager gameManager
        {
            set => GM = value;
        }
        
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
                OnSpawnPeople();
                Destroy(transform.gameObject);
            }
        }

        private void OnSpawnPeople()
        {
            Villager_Object_Pool_Script _villagerObjectPool = FindObjectOfType<Villager_Object_Pool_Script>();
            Worker_Object_Pool_Script _workerObjectPool = FindObjectOfType<Worker_Object_Pool_Script>();
            HumanResourceManager HRM = HumanResourceManager.Instance;
            GM = GameManager.Instance;

            for (int i = 0; i < GM.villagers_start; i++)
            {
                People_System_Script peopleSystemScript = _villagerObjectPool.Spawn();
                print("Is Vill Nullll : " + (peopleSystemScript == null));
                HRM.AddPeople<Villager_System_Script>(peopleSystemScript);
            }

            for (int i = 0; i < GM.workers_start; i++)
            {
                People_System_Script peopleSystemScript = _workerObjectPool.Spawn();
                print("Is Wok Nullll : " + (peopleSystemScript == null));
                HRM.AddPeople<Worker_System_Script>(peopleSystemScript);
            }
        }
    }
}