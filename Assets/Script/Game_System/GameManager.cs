using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD
{
    public class GameManager:Singleton_With_DontDestroy<GameManager>
    {
        [SerializeField]private PM2_5_Preset m_pm2_5Preset;
        [SerializeField]private int villagers_count = 12;
        [SerializeField]private int workers_count = 12;
        [SerializeField] private GameObject m_gameover_ui;
        [SerializeField] private GameObject m_resourcelow_ui;
        [SerializeField] private GameObject m_backtocity_ui;
        
        private GameInstance _gameInstance = new GameInstance();

        private LoadingSceneSystem _loadingSceneSystem;
        private TimeManager TM;

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

        public PM2_5_Preset Pm25Preset
        {
            get => m_pm2_5Preset;
        }

        public int villagers_start
        {
            get => villagers_count;
        }
        
        public int workers_start
        {
            get => workers_count;
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

        private void Start()
        {
            TM = TimeManager.Instance;
        }

        private void OnEnable()
        {
            
        }

        public void Reset()
        {
            _gameInstance = new GameInstance();
        }
        
        public void OnGameOver()
        {
            _loadingSceneSystem = FindObjectOfType<LoadingSceneSystem>();
            
            GameObject gameover_Ui = Instantiate(m_gameover_ui);
            gameover_Ui.GetComponent<Canvas>().planeDistance = 1;
            gameover_Ui.GetComponent<Canvas>().worldCamera = Camera.main;

            Time_Controll_UI_Script.SetSpeed(0);
            
            Canvas_Element_List _element = gameover_Ui.GetComponent<Canvas_Element_List>();
            _element.animators[0].SetBool("IsStart", true);
            
            _element.buttons[0].onClick.AddListener((() =>
            {
                gameover_Ui.GetComponent<Canvas>().sortingOrder = 20;
                _loadingSceneSystem.LoadScene("MainMenu");
                _element.animators[0].SetBool("IsStart", false);
                
                Reset();
                Destroy(HumanResourceManager.Instance.gameObject);
                Destroy(ResourcesManager.Instance.gameObject);
                Destroy(FindObjectOfType<LoadSceneWithSaveData>().gameObject);
            }));
        }

        public void OnReourceLow()
        {
            _loadingSceneSystem = FindObjectOfType<LoadingSceneSystem>();
            
            GameObject reourceLow_Ui = Instantiate(m_resourcelow_ui);
            reourceLow_Ui.GetComponent<Canvas>().planeDistance = 1;
            reourceLow_Ui.GetComponent<Canvas>().worldCamera = Camera.main;

            Time_Controll_UI_Script.SetSpeed(0);
            
            Canvas_Element_List _element = reourceLow_Ui.GetComponent<Canvas_Element_List>();
            _element.animators[0].SetBool("IsStart", true);
            
            _element.buttons[0].onClick.AddListener((() =>
            {
                string defaultPath = Application.persistentDataPath;
                string location;
                var default_info = new DirectoryInfo(defaultPath);
                bool is_fount = false;

                //Location Save
                if (gameInstance.auto_save_count > 0)
                    location = Application.persistentDataPath + "/AutoSaveDay " + TimeManager.Instance.getTotalDay + " (" + gameInstance.auto_save_count + ")";
                else
                    location = Application.persistentDataPath + "/AutoSaveDay " + TimeManager.Instance.getTotalDay;
                
                gameInstance.savefile_backtocity = location;
                
                if (gameInstance.day_save_count != TM.getTotalDay)
                {
                    gameInstance.auto_save_count = 0;
                    gameInstance.day_save_count = TM.getTotalDay;
                }

                List<FileInfo> allFiles = default_info.GetFiles("*.json*")
                    .OrderByDescending(f => f.LastWriteTime.Year <= 1601 ? f.CreationTime : f.LastWriteTime).ToList();

                //SaveGame
                foreach (var fileInfo in allFiles)
                {
                    print(fileInfo.Name + " || " + "AutoSaveDay " + TimeManager.Instance.getTotalDay + ".json");
                    if (fileInfo.Name == "AutoSaveDay " + TimeManager.Instance.getTotalDay + ".json")
                    {
                        gameInstance.auto_save_count++;
                        SaveManager.Instance.SaveGamePreferencesData(gameInstance, location);
                        
                        is_fount = true;
                    }
                }

                if(!is_fount)
                    SaveManager.Instance.SaveGamePreferencesData(gameInstance, location);
                
                //LoadSave
                m_resourcelow_ui.GetComponent<Canvas>().sortingOrder = 19;
                _loadingSceneSystem.LoadScene("Menu");
                _element.animators[0].SetBool("IsStart", false);
                
                Destroy(FindObjectOfType<LoadSceneWithSaveData>().gameObject);
            }));

            _element.buttons[1].onClick.AddListener(() =>
            {
                Ui_Utilities _uiUtilities;
                if (_element.GetComponent<Ui_Utilities>() == null)
                {
                    _uiUtilities = _element.AddComponent<Ui_Utilities>();
                }
                else
                {
                    _uiUtilities = _element.GetComponent<Ui_Utilities>();
                }

                _uiUtilities.canvasUI = reourceLow_Ui;
                _uiUtilities.RemoveUI();
            });
        }

        private void Update()
        {
            //print("Building Count " + gameInstance.buildingSaveDatas.Count);
        }

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