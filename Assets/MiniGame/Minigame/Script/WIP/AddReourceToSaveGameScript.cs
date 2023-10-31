using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class AddReourceToSaveGameScript : MonoBehaviour
    {
        [SerializeField] private Button m_backtocity;
        private Ch9Inventory inventory;
        private ResourcesManager RM;
        private GameManager GM;
        private SaveManager SM;
        private TimeManager TM;
        private LoadingSceneSystem m_loadingSceneSystem;
        
        private void Start()
        {
            RM = ResourcesManager.Instance;
            GM = GameManager.Instance;
            SM = SaveManager.Instance;
            TM = TimeManager.Instance;
            m_loadingSceneSystem = FindObjectOfType<LoadingSceneSystem>();
            inventory = FindObjectOfType<Ch9Inventory>();
            
            //Debug.LogWarning("Resource Count : Wood = " + get_resource().Item1 + " || Stone = " + get_resource().Item2);
            
            m_backtocity.onClick.AddListener(() =>
            {
                //AddResource
                RM.Set_Resources_Tree((get_resource().Item1) * 10);
                RM.Set_Resources_Rock((get_resource().Item2) * 10);
                
                //SaveGame
                string defaultPath = Application.persistentDataPath;
                string location;
                var default_info = new DirectoryInfo(defaultPath);
                bool is_fount = false;

                if (GM.gameInstance.auto_save_count > 0)
                    location = Application.persistentDataPath + "/AutoSaveDay " + TimeManager.Instance.getTotalDay + " (" + GM.gameInstance.auto_save_count + ")";
                else
                    location = Application.persistentDataPath + "/AutoSaveDay " + TimeManager.Instance.getTotalDay;
                
                //LoadSaveData
                GameInstance gameInstance_load = SM.LoadGamePreferencesData(location, GM.gameInstance.GetType()).ConvertTo<GameInstance>();
                GM.gameInstance.buildingSaveDatas = gameInstance_load.buildingSaveDatas;
                GM.gameInstance.RoadSaveDatas = gameInstance_load.RoadSaveDatas;
                GM.gameInstance.staticResourceSaveDatas = gameInstance_load.staticResourceSaveDatas;
                GM.gameInstance.villagerSaveDatas = gameInstance_load.villagerSaveDatas;
                GM.gameInstance.workerSaveDatas = gameInstance_load.workerSaveDatas;

                Debug.LogWarning("load Building Count : " + gameInstance_load.buildingSaveDatas);
                Debug.LogWarning("Building Count : " + GM.gameInstance.buildingSaveDatas);
                
                //Set Location Save
                GM.gameInstance.savefile_backtocity = location;
                
                if (GM.gameInstance.day_save_count != TM.getTotalDay)
                {
                    GM.gameInstance.auto_save_count = 0;
                    GM.gameInstance.day_save_count = TM.getTotalDay;
                }

                List<FileInfo> allFiles = default_info.GetFiles("*.json*")
                    .OrderByDescending(f => f.LastWriteTime.Year <= 1601 ? f.CreationTime : f.LastWriteTime).ToList();

                foreach (var fileInfo in allFiles)
                {
                    print(fileInfo.Name + " || " + "AutoSaveDay " + TimeManager.Instance.getTotalDay + ".json");
                    if (fileInfo.Name == "AutoSaveDay " + TimeManager.Instance.getTotalDay + ".json")
                    {
                        GM.gameInstance.auto_save_count++;
                        SaveManager.Instance.SaveGamePreferencesData(GM.gameInstance, location);
                        
                        is_fount = true;
                    }
                }

                if(!is_fount)
                    SaveManager.Instance.SaveGamePreferencesData(GM.gameInstance, location);
                
                //LoadSave
                var saveData = SM.LoadGamePreferencesData(GM.gameInstance.savefile_backtocity, GM.gameInstance.GetType());
                GM.GetSetGameManager = saveData;
                GM.OnGameLoad();
                
                //LoadScene Back To City
                m_loadingSceneSystem.LoadScene("GameLevel");
            });
        }

        private Tuple<int, int> get_resource()
        {
            Tuple<int, int> resource_datas = new Tuple<int, int>(0, 0);

            if (inventory != null)
            {
                string inventoryInfo = "Inventory:\n";
                foreach (var key in inventory.GetKeys())
                {
                    switch (key)
                    {
                        case "Wood":
                            resource_datas = new Tuple<int, int>(inventory.GetItemAmount(key), resource_datas.Item2);
                            break;
                        case "Stone":
                            resource_datas = new Tuple<int, int>(resource_datas.Item1, inventory.GetItemAmount(key));
                            break;
                    }
                    /*
                    int amount = inventory.GetItemAmount(key);
                    inventoryInfo += $"{key}: {amount}\n";*/
                }
            }

            return resource_datas;
        }

        private void Update()
        {
            
        }
    }
}