using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class GameManager:Singleton<GameManager>
    {
        private string Name = "";
        private int Level = 0;
        private int EXP = 0;
        private int Score = 0;
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

        public string GetName
        {
            get { return Name; } 
            set { Name = value; } 
        }
        public int GetLevel
        {
            get { return Level; } 
            set { Level = value; } 
        }
        
        public int GetEXP
        {
            get { return EXP; } 
            set { EXP = value; } 
        }
        
        public int GetScore
        {
            get { return Score; } 
            set { Score = value; } 
        }
        
        private void OnGUI()
        {
            string s = "Building count : " + gameInstance.buildingSystemScript.Count + " | Road count : " + gameInstance.roadSystemScripts.Count;
            GUI.Label(new Rect(new Vector2(5, 20), new Vector2(350, 30)), "Time : " + s);
        }
        
        public void OnGameLoad()
        {
            LoadSceneWithSaveData loadSceneWithSaveData;
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