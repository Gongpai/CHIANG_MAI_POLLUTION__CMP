using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class GameManager:Singleton_With_DontDestroy<GameManager>
    {
        [SerializeField]private int villagers_count = 12;
        [SerializeField]private int workers_count = 12;
        
        private GameInstance _gameInstance = new GameInstance();

        public GameInstance gameInstance
        {
            get { return _gameInstance; }
            set { _gameInstance = value; }
        }

        public object GetSetGameManager
        {
            get
            {
                var fieldValues = MemberInfoGetting.GetFieldValues(gameInstance);

                return fieldValues;
            }
            set
            {
                _gameInstance = value.ConvertTo<GameInstance>();
            }
        }

        public int PM_25
        {
            get => gameInstance.pm_25;
            set => gameInstance.pm_25 = value;
        }

        public int villagers_start
        {
            get => villagers_count;
            set => villagers_count = value;
        }
        
        public int workers_start
        {
            get => workers_count;
            set => workers_count = value;
        }
        /*
        private void OnGUI()
        {
            string s = "Building count : " + gameInstance.buildingSaveDatas.Count + " | Road count : " + gameInstance.RoadSaveDatas.Count + " | Resource count : " + gameInstance.staticResourceSaveDatas.Count;
            GUI.Label(new Rect(new Vector2(5, 40), new Vector2(450, 30)), "Save Data : " + s);
            
            string p = "Villager count : " + gameInstance.villagerSaveDatas.Count + " | Worker count : " + gameInstance.workerSaveDatas.Count;
            GUI.Label(new Rect(new Vector2(5, 80), new Vector2(450, 30)), "Save Data : " + p);
            
            GUILayout.BeginArea(new Rect(10, 75, 500, 1080));
            
            //Villager Data
            foreach (var peopleSystemSave in gameInstance.villagerSaveDatas)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label("Villager\n");
                GUILayout.Label("Health : " + (peopleSystemSave.health * 100) + "%\n");
                GUILayout.Label("Hunger : " + (peopleSystemSave.hunger * 100) + "%\n");
                GUILayout.Label("Content : " + (peopleSystemSave.content * 100) + "%\n");
                GUILayout.Label("Current Job : " + (PeopleJob)peopleSystemSave.job + "\n");
                GUILayout.Label("DailyLife : " + (PeopleDailyLife)peopleSystemSave.dailyLife + "\n");
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
            
            
            GUILayout.BeginArea(new Rect(510, 75, 500, 1080));
            //Worker Data
            foreach (var peopleSystemSave in gameInstance.workerSaveDatas)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label("Worker\n");
                GUILayout.Label("Health : " + (peopleSystemSave.health * 100) + "%\n");
                GUILayout.Label("Hunger : " + (peopleSystemSave.hunger * 100) + "%\n");
                GUILayout.Label("Content : " + (peopleSystemSave.content * 100) + "%\n");
                GUILayout.Label("Current Job : " + (PeopleJob)peopleSystemSave.job + "\n");
                GUILayout.Label("DailyLife : " + (PeopleDailyLife)peopleSystemSave.dailyLife + "\n");
                GUILayout.EndHorizontal();
            }
        
            GUILayout.EndArea();
            
        }
        */
        public void OnGameLoad()
        {
            if (!FindObjectOfType<LoadSceneWithSaveData>())
            {
                GameObject loadObject = new GameObject("Load Scene With Save Data");
                loadObject.AddComponent<LoadSceneWithSaveData>();
                loadObject.GetComponent<LoadSceneWithSaveData>().enabled = false;
                DontDestroyOnLoad(loadObject);
            }
        }
    }
}