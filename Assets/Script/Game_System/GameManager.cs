using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class GameManager:Singleton_With_DontDestroy<GameManager>
    {
        [SerializeField]private PM2_5_Preset m_pm2_5Preset;
        [SerializeField]private int villagers_count = 12;
        [SerializeField]private int workers_count = 12;
        [SerializeField] private GameObject m_gameover_ui;
        
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
            TM.timeScale = 0;
            
            GameObject gameover_Ui = Instantiate(m_gameover_ui);
            gameover_Ui.GetComponent<Canvas>().planeDistance = 1;
            gameover_Ui.GetComponent<Canvas>().worldCamera = Camera.main;
            Canvas_Element_List _element = gameover_Ui.GetComponent<Canvas_Element_List>();
            _element.animators[0].SetBool("IsStart", true);
            
            _element.buttons[0].onClick.AddListener((() =>
            {
                gameover_Ui.GetComponent<Canvas>().sortingOrder = 0;
                _loadingSceneSystem.LoadScene("MainMenu");
                _element.animators[0].SetBool("IsStart", false);
                
                Reset();
                Destroy(HumanResourceManager.Instance.gameObject);
                Destroy(ResourcesManager.Instance.gameObject);
            }));
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