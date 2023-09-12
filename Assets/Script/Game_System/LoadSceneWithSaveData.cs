using System;
using System.Collections;
using System.Collections.Generic;
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
        private Spawner_Object_Grid spawnerObjectGrid;
        private Spawner_Road_Grid spawnerRoadGrid;

        private float time = 0;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            //SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 0)
            {
                this.enabled = true;
                time = 0;
                print("OnSceneLoadddddd");
                GM = GameManager.Instance;
                GI = GM.gameInstance;
                TM = TimeManager.Instance;
                spawnerObjectGrid = FindObjectOfType<Spawner_Object_Grid>();
                spawnerRoadGrid = FindObjectOfType<Spawner_Road_Grid>();

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

                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        public void OnSceneUnLoaded(Scene current)
        {
            
        }

        IEnumerator LoadGameSave()
        {
            print("Runnnnnnnnnnnnnnnnnnnnnnnnnn");
            
            for(int index = 0; index < GI.buildingSaveDatas.Count; index++)
            {
                spawnerObjectGrid.enabled = true;
                GameObject buildingobject = Resources.Load(GI.buildingSaveDatas[index].pathObject).GameObject();

                //print("Building OBJ : " + buildingobject.name + " | Spawner OBJ : " + (spawnerObjectGrid == null) + " | Building SS : " + GI.buildingSystemScript.Count);
                spawnerObjectGrid.SpawnerWithLoadScene(GI.buildingSaveDatas[index], buildingobject);
                time += 1;
            }
            
            for(int index = 0; index < GI.RoadSaveDatas.Count; index++)
            {
                spawnerRoadGrid.enabled = true;
                GameObject roadobject = Resources.Load(GI.RoadSaveDatas[index].path).GameObject();
                print(roadobject.GetComponent<MeshFilter>().sharedMesh + " Index : " + index + " / count : " + (GI.RoadSaveDatas.Count - 1));
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
