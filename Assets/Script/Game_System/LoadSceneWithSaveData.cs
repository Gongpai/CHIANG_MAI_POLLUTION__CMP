using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class LoadSceneWithSaveData : MonoBehaviour
    {
        private GameManager GM;
        private GameInstance GI;
        private TimeManager TM;
        private HumanResourceManager HRM;
        private Spawner_Object_Grid spawnerObjectGrid;
        private Spawner_Road_Grid spawnerRoadGrid;
        private Villager_Object_Pool_Script _villagerObjectPool;
        private Worker_Object_Pool_Script _workerObjectPool;

        private float time = 0;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 0 && this != null)
            {
                this.enabled = true;
                time = 0;
                print("OnSceneLoadddddd");
                GM = GameManager.Instance;
                GI = GM.gameInstance;
                TM = TimeManager.Instance;
                HRM = HumanResourceManager.Instance;
                spawnerObjectGrid = FindObjectOfType<Spawner_Object_Grid>();
                spawnerRoadGrid = FindObjectOfType<Spawner_Road_Grid>();
                _villagerObjectPool = FindObjectOfType<Villager_Object_Pool_Script>();
                _workerObjectPool = FindObjectOfType<Worker_Object_Pool_Script>();

                if (!GI.IsObjectEmpty())
                {
                    print("Gotoloaddddd");
                    StartCoroutine(LoadGameSave());
                }

                this.enabled = false;

                if (spawnerObjectGrid != null)
                    spawnerObjectGrid.enabled = false;
                if (spawnerRoadGrid != null)
                    spawnerRoadGrid.enabled = false;
                
                Notification notification = new Notification();
                notification.text = "Successfully loaded saved data.";
                notification.icon = Resources.Load<Sprite>("Icon/save_icon");
                notification.iconColor = Color.white;
                notification.duration = 5.0f;
                notification.isWaitTrigger = false;
                NotificationManager.Instance.AddNotification(notification); 
                
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        public void OnSceneUnLoaded(Scene current)
        {
            
        }

        IEnumerator LoadGameSave()
        {
            //print("Run" + GI.villagerSaveDatas.Count + "nnnnnnnnnnnnnnnnnnnnnnnnn");

            //People Save Datas
            int villager_count = GI.villagerSaveDatas.Count;
            for (int index = 0, i_remove = 0; index < villager_count; index++)
            {
                if (GI.villagerSaveDatas[i_remove].job != Decimal.Zero)
                {
                    //print(i_remove + ". Remove Job : " + (PeopleJob)GI.villagerSaveDatas[i_remove].job);
                    GI.villagerSaveDatas.RemoveAt(i_remove);
                }
                else
                {
                    //print(i_remove + ". Notaaaaaa Job : " + (PeopleJob)GI.villagerSaveDatas[i_remove].job);
                    i_remove++;
                }
            }

            int worker_count = GI.workerSaveDatas.Count;
            for (int index = 0, i_remove = 0; index < worker_count; index++)
            {
                if (GI.workerSaveDatas[i_remove].job != Decimal.Zero)
                {
                    //print(i_remove + ". Remove Job : " + (PeopleJob)GI.workerSaveDatas[i_remove].job);
                    GI.workerSaveDatas.RemoveAt(i_remove);
                }
                else
                {
                    //print(i_remove + ". Not Job : " + (PeopleJob)GI.workerSaveDatas[i_remove].job);
                    i_remove++;
                }
            }
            
            for (int index = 0; index < GI.villagerSaveDatas.Count; index++)
            {
                People_System_Script peopleSystemScript = _villagerObjectPool.Spawn(GI.villagerSaveDatas[index]);
                HRM.AddPeople<Villager_System_Script>(peopleSystemScript);
                
            }
            
            for (int index = 0; index < GI.workerSaveDatas.Count; index++)
            {
                People_System_Script peopleSystemScript = _workerObjectPool.Spawn(GI.workerSaveDatas[index]);
                HRM.AddPeople<Worker_System_Script>(peopleSystemScript);
            }
            
            //Building Save Datas
            for(int index = 0; index < GI.buildingSaveDatas.Count; index++)
            {
                spawnerObjectGrid.enabled = true;
                GameObject buildingobject = Resources.Load(GI.buildingSaveDatas[index].pathObject).GameObject();

                //print("Building OBJ : " + buildingobject.name + " | Spawner OBJ : " + (spawnerObjectGrid == null) + " | Building SS : " + GI.buildingSystemScript.Count);
                spawnerObjectGrid.SpawnerWithLoadScene(GI.buildingSaveDatas[index], buildingobject);
                
                time += 1;
            }
            
            //Resource Save Data
            //print("Is Sttic Save is Null : " + (GI.staticResourceSaveDatas == null));
            List<Static_Object_Resource_System_Script> resourceSystemScripts = FindObjectsOfType<Static_Object_Resource_System_Script>().ToList();
            for (int i = 0; i < resourceSystemScripts.Count; i++)
            {
                resourceSystemScripts[i].OnGameLoad();

                
                if (GI.staticResourceSaveDatas != null)
                {
                    foreach (var staticResourceSaveData in GI.staticResourceSaveDatas)
                    {
                        //print("Save ID Is : " + staticResourceSaveData.id + " ID is : " + resourceSystemScripts[i].get_resource_id);
                        if (staticResourceSaveData.id == resourceSystemScripts[i].get_resource_id)
                        {
                            resourceSystemScripts[i].set_staticResourceSaveData = staticResourceSaveData;
                            break;
                        }
                    }
                }
            }
            
            //Road Save Datas
            for(int index = 0; index < GI.RoadSaveDatas.Count; index++)
            {
                spawnerRoadGrid.enabled = true;
                GameObject roadobject = Resources.Load(GI.RoadSaveDatas[index].path).GameObject();
                
                //print(roadobject.GetComponent<MeshFilter>().sharedMesh + " Index : " + index + " / count : " + (GI.RoadSaveDatas.Count - 1));
                spawnerRoadGrid.SpawnerWithLoadScene(GI.RoadSaveDatas[index], roadobject);
            }
            
            yield return 0;
        }
        
        /*
        private void OnGUI()
        {
            GUI.Label(new Rect(new Vector2(5, 5), new Vector2(250, 30)), "Time : " + time);
        }
        */
    }
}
