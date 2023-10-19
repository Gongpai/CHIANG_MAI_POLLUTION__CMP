using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD
{
    public class People_Script : Building_System_Script
    {
        [SerializeField] private People_Preset m_peoplePreset;
        [SerializeField] private GameObject m_human_boy;
        [SerializeField] private GameObject m_human_girl;
        private People_SaveData _peoplScriptSaveData = new People_SaveData();
        private bool is_spawn;
        

        public int get_people_count
        {
            get => _peoplScriptSaveData.peoples.Count;
        }
        
        public int get_people_max
        {
            get => m_peoplePreset.people;
        }
        
        public override void Resource_usage()
        {
            
        }

        protected override bool Check_Resource()
        {
            return false;
        }
        
        public override void BeginStart()
        {
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[1].title, m_Preset.m_building_status[1].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[2].title, m_Preset.m_building_status[2].text + get_people_count + "/" + m_peoplePreset.people + " คน", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
        }

        public override void EndStart()
        {
            
        }

        protected override void Update()
        {
            base.Update();

            if (TM.getGameTimeHour == 8 && !is_spawn && building_is_active)
            {
                SpawnPeopleToWork();
                is_spawn = true;
            }
            else if(TM.getGameTimeHour > 9)
            {
                is_spawn = false;
            }
        }

        public void OnAddPeople(PeopleSystemSaveData _peopleSaveData)
        {
            if (get_people_count < m_peoplePreset.people)
            {
                _peoplScriptSaveData.peoples.Add(_peopleSaveData);
            }
        }

        public void OnRemovePeople(PeopleSystemSaveData _peopleSaveData)
        {
            if (get_people_count > 0)
            {
                _peoplScriptSaveData.peoples.Remove(_peopleSaveData);
            }
        }

        protected void SpawnPeopleToWork()
        {
            List<Building_System_Script> buildingSystems = FindObjectsByType<Building_System_Script>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
            List<Static_Object_Resource_System_Script> staticSystemScripts =
                FindObjectsByType<Static_Object_Resource_System_Script>(FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None).ToList();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        m_human_boy.transform.position =  waypoint.position;
                        m_human_girl.transform.position =  waypoint.position;
                        break;
                    case 1:
                        m_human_boy.transform.position =  waypoint.position + (waypoint.right * 0.1f);
                        m_human_girl.transform.position =  waypoint.position + (waypoint.right * 0.1f);
                        break;
                    case 2:
                        m_human_boy.transform.position =  waypoint.position - (waypoint.right * 0.1f);
                        m_human_girl.transform.position =  waypoint.position - (waypoint.right * 0.1f);
                        break;
                }
                
                float random_type = Random.Range(0, 1);
                if (random_type == 0)
                {
                    if (buildingSystems != null)
                    {
                        float random_gender = Random.Range(0, 1);
                        GameObject spawn = null;
                        Building_System_Script buildingSystemScript = null;

                        Parallel.ForEach(buildingSystems, (script, state) =>
                        {
                            if ((script.villager_count > 0 || script.worker_count > 0) && script._buildingType != BuildingType.People)
                            {
                                buildingSystemScript = script;
                                state.Stop();
                            }
                        });

                        if (buildingSystemScript != null)
                        {
                            if (random_gender == 0)
                                spawn = Instantiate(m_human_boy);
                            else
                                spawn = Instantiate(m_human_girl);

                            spawn.transform.position = waypoint.position;
                            WaypointReachingState waypointReachingState = spawn.GetComponent<WaypointReachingState>();
                            waypointReachingState.waypoints.Add(buildingSystemScript.waypoint);
                            waypointReachingState.SetWaypointIndex = 0;
                            waypointReachingState.EnterState();
                            waypointReachingState.is_Start = true;
                            
                            print("Spawn PP Building");
                        }
                    }
                }
                else
                {
                    if (staticSystemScripts != null)
                    {
                        float random_gender = Random.Range(0, 1);
                        GameObject spawn = null;
                        
                        Static_Object_Resource_System_Script staticObjectResourceSs = null;

                        Parallel.ForEach(staticSystemScripts, (script, state) =>
                        {
                            if (script.villager_count > 0 || script.worker_count > 0)
                            {
                                staticObjectResourceSs = script;
                                state.Stop();
                            }
                        });


                        if (staticObjectResourceSs != null)
                        {
                            if (random_gender == 0)
                                spawn = Instantiate(m_human_boy);
                            else
                                spawn = Instantiate(m_human_girl);

                            spawn.transform.position = waypoint.position;
                            WaypointReachingState waypointReachingState = spawn.GetComponent<WaypointReachingState>();
                            waypointReachingState.waypoints.Add(staticObjectResourceSs.waypoint);
                            waypointReachingState.SetWaypointIndex = 0;
                            waypointReachingState.EnterState();
                            waypointReachingState.is_Start = true;
                            
                            print("Spawn PP StaticObject");
                        }
                    }
                }
            }
        }

        public override void OnEnableBuilding()
        {
            
        }

        public override void OnDisableBuilding()
        {
            
        }

        protected override void OnUpdateSettingValue()
        {
            
        }

        protected override bool OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && !is_cant_use_power, null, m_Preset.m_building_status[1].text));
            list_information_values.Add(new Tuple<object, object, string>((float)get_people_count, (float)m_peoplePreset.people, m_Preset.m_building_status[2].text + " " + get_people_count + "/" + m_peoplePreset.people + " คน"));

            return active && !is_cant_use_power;
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<People_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _peoplScriptSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _peoplScriptSaveData;
        }

        public override void OnEndPlace()
        {
            
        }

        public override void OnBeginRemove()
        {
            
        }

        public override void OnEndRemove()
        {
           
        }

        public override void OnDestroyBuilding()
        {
            
        }
    }
}