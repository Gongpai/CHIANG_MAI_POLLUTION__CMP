using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class LoadSceneWithSaveData : MonoBehaviour
    {
        private GameManager GM;
        private GameInstance GI;
        private Spawner_Object_Grid spawnerObjectGrid;
        private Spawner_Road_Grid spawnerRoadGrid;

        private float time = 0;
        private int index = 0;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.enabled = true;
            index = 0;
            time = 0;
            print("OnSceneLoadddddd");
            GM = FindObjectOfType<GameManager>();
            GI = GM.gameInstance;
            spawnerObjectGrid = FindObjectOfType<Spawner_Object_Grid>();
            spawnerRoadGrid = FindObjectOfType<Spawner_Road_Grid>();
        }

        private void Update()
        {
            //print(GI.buildingSystemScript.Count);
            // !GI.IsObjectEmpty() && (index < GI.buildingSystemScript.Count || index < GI.roadSystemScripts.Count)
            if (!GI.IsObjectEmpty() && (index < GI.buildingSystemScript.Count || index < GI.roadSystemScripts.Count))
            {
                if (time >= 0.5f)
                {
                    if (index < GI.buildingSystemScript.Count)
                    {
                        spawnerObjectGrid.enabled = true;
                        GameObject buildingobject = Resources.Load(GI.buildingSystemScript[index].pathObject).GameObject();

                        //print("Building OBJ : " + buildingobject.name + " | Spawner OBJ : " + (spawnerObjectGrid == null) + " | Building SS : " + GI.buildingSystemScript.Count);
                        spawnerObjectGrid.SpawnerWithLoadScene(GI.buildingSystemScript[index], buildingobject);
                    }

                    if (index < GI.roadSystemScripts.Count)
                    {
                        spawnerRoadGrid.enabled = true;
                        GameObject roadobject = Resources.Load(GI.roadSystemScripts[index].path).GameObject();
                        print(roadobject.GetComponent<MeshFilter>().sharedMesh + " Index : " + index + " / count : " + (GI.roadSystemScripts.Count - 1));
                        spawnerRoadGrid.SpawnerWithLoadScene(GI.roadSystemScripts[index], roadobject);
                    }

                    index++;
                    time = 0;
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
            else
            {
                this.enabled = false;
                spawnerObjectGrid.enabled = false;
                spawnerRoadGrid.enabled = false;
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(new Vector2(5, 5), new Vector2(250, 30)), "Time : " + time);
        }
    }
}
