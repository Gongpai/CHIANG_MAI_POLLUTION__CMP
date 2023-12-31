﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class HumanResourceManager : Sinagleton_CanDestroy<HumanResourceManager>
    {
        Villager_Object_Pool_Script _villagerObjectPool;
        Worker_Object_Pool_Script _workerObjectPool;
        private List<Tuple<Villager_System_Script, PeopleJob>> villagers = new();
        private List<Tuple<Worker_System_Script, PeopleJob>> workers = new();
        private List<People_System_Script> m_residences = new();
        private List<People_System_Script> m_healings = new();
        private List<People_System_Script> m_patients = new();

        private List<People_System_Script> _peopleDeaths = new();
        private List<Notification> _notifications = new List<Notification>();
        private bool is_people_death = false;
        private int old_hour = 0;

        private TimeManager TM;

        public List<People_System_Script> peopleDeaths
        {
            get => _peopleDeaths;
            set => _peopleDeaths = value;
        }
        
        public int villagers_count
        {
            get => villagers.Count;
        }

        public int worker_count
        {
            get => workers.Count;
        }
        
        public List<Tuple<Villager_System_Script, PeopleJob>> get_villager_datas
        {
            get => villagers;
        }

        public List<Tuple<Worker_System_Script, PeopleJob>> get_worker_datas
        {
            get => workers;
        }

        public List<People_System_Script> residence
        {
            get => m_residences;
            set => m_residences = value;
        }

        public List<People_System_Script> healing
        {
            get => m_healings;
            set => m_healings = value;
        }

        public List<People_System_Script> patients
        {
            get => m_patients;
            set => m_patients = value;
        }

        private void Awake()
        {
            _villagerObjectPool = FindObjectOfType<Villager_Object_Pool_Script>();
            _workerObjectPool = FindObjectOfType<Worker_Object_Pool_Script>();

            TM = TimeManager.Instance;
        }

        private void LateUpdate()
        {
            if (peopleDeaths.Count > 0)
            {
                Notification notification = new Notification();
                notification.text = "People Death : " + peopleDeaths.Count;
                notification.icon = Resources.Load<Sprite>("Icon/warning_icon");
                notification.iconColor = Color.white;
                notification.soundSFX = Resources.Load<AudioClip>("Sound/UI_Sound/bell-people-death");
                notification.duration = 10.0f;
                notification.isWaitTrigger = false;

                NotificationManager NM = NotificationManager.Instance;
                NM.AddNotification(notification);

                peopleDeaths = new();
            }

            if (patients.Count > 0 && old_hour == TimeManager.Instance.getGameTimeHour)
            {
                Notification notification = new Notification();
                notification.text = "People are sick";
                notification.icon = Resources.Load<Sprite>("Icon/emergency_icon");
                notification.iconColor = Color.white;
                notification.duration = 10.0f;
                notification.isWaitTrigger = false;
                _notifications.Add(notification);
            }
            
            if (((worker_count + villagers_count) - residence.Count) > 0 && old_hour == TimeManager.Instance.getGameTimeHour)
            {
                Notification notification = new Notification();
                notification.text = "People need housing";
                notification.icon = Resources.Load<Sprite>("Icon/remove_home");
                notification.iconColor = Color.white;
                notification.duration = 10.0f;
                notification.isWaitTrigger = false;
                _notifications.Add(notification);
            }

            if (_notifications.Count > 0 && old_hour == TimeManager.Instance.getGameTimeHour)
            {
                foreach (var notification in _notifications)
                {
                    NotificationManager.Instance.AddNotification(notification);
                }
                
                Set_Old_hour_Noti_Showing();
                _notifications = new List<Notification>();
            }

            if (GameManager.Instance.gameInstance.villagerSaveDatas.Count == 0 && GameManager.Instance.gameInstance.workerSaveDatas.Count == 0 && !is_people_death && SceneManager.GetActiveScene().buildIndex != 1 && TimeManager.Instance.getGameTimeHour > 0)
            {
                GameManager.Instance.OnGameOver();
                print("GAME OVERRR!!!!!!!!!!!!!!");
                is_people_death = true;
                
                Notification notification = new Notification();
                notification.text = "All People Death";
                notification.icon = Resources.Load<Sprite>("Icon/person");
                notification.iconColor = Color.white;
                notification.duration = 5.0f;
                notification.isWaitTrigger = false;
                NotificationManager.Instance.AddNotification(notification);
            }
        }

        private void Set_Old_hour_Noti_Showing()
        {
            if (TM.getGameTimeHour >= 0)
            {
                old_hour = 6;
            }
            if (TM.getGameTimeHour >= 6)
            {
                old_hour = 12;
            }
            if (TM.getGameTimeHour >= 12)
            {
                old_hour = 18;
            }
            if (TM.getGameTimeHour >= 18)
            {
                old_hour = 0;
            }
        }
        
        public void OnSpawnPeople(int villager_count, int worker_count)
        {
            Villager_Object_Pool_Script _villagerObjectPool = FindObjectOfType<Villager_Object_Pool_Script>();
            Worker_Object_Pool_Script _workerObjectPool = FindObjectOfType<Worker_Object_Pool_Script>();

            for (int i = 0; i < villager_count; i++)
            {
                People_System_Script peopleSystemScript = _villagerObjectPool.Spawn();
                print("Is Vill Nullll : " + (peopleSystemScript == null));
                AddPeople<Villager_System_Script>(peopleSystemScript);
            }

            for (int i = 0; i < worker_count; i++)
            {
                People_System_Script peopleSystemScript = _workerObjectPool.Spawn();
                print("Is Wok Nullll : " + (peopleSystemScript == null));
                AddPeople<Worker_System_Script>(peopleSystemScript);
            }
        }

        public void AddPeople<T>(People_System_Script _peopleSystemScript)
        {
            if(typeof(T) == typeof(Villager_System_Script))
                villagers.Add(new Tuple<Villager_System_Script, PeopleJob>((Villager_System_Script)_peopleSystemScript, PeopleJob.Unemployed));
            
            if(typeof(T) == typeof(Worker_System_Script))
                workers.Add(new Tuple<Worker_System_Script, PeopleJob>((Worker_System_Script)_peopleSystemScript, PeopleJob.Unemployed));
        }

        public void RemovePeople<T>(People_System_Script _peopleSystemScript, PeopleJob job)
        {
            if (typeof(T) == typeof(Villager_System_Script))
                villagers.Remove(new Tuple<Villager_System_Script, PeopleJob>((Villager_System_Script)_peopleSystemScript, job));
                
            if(typeof(T) == typeof(Worker_System_Script))
                workers.Remove(new Tuple<Worker_System_Script, PeopleJob>((Worker_System_Script)_peopleSystemScript, job));
        }

        public Tuple<Villager_System_Script, PeopleJob> GetVillager()
        {
            return villagers[0];
        }

        public Tuple<Worker_System_Script, PeopleJob> GetWorker()
        {
            return workers[0];
        }
        
        public void VillagerGotoWorkBuilding(Tuple<Villager_System_Script, PeopleJob> _villagerdata, Building_System_Script _buildingSystemScript)
        {
            _villagerdata.Item1._constructionSystem = _buildingSystemScript;
            _buildingSystemScript.building_villagers.Add(new Tuple<Villager_System_Script, PeopleJob>(_villagerdata.Item1, (PeopleJob)_villagerdata.Item1.saveData.job));
            
            villagers.Remove(_villagerdata);
        }
        
        public bool CanGetVillager()
        {
            return (villagers.Count != 0);
        }
        
        public void WorkerGotoWorkBuilding(Tuple<Worker_System_Script, PeopleJob> _workerData, Building_System_Script _buildingSystemScript)
        {
            _workerData.Item1._constructionSystem = _buildingSystemScript;
            _buildingSystemScript.building_workers.Add(new Tuple<Worker_System_Script, PeopleJob>(_workerData.Item1, (PeopleJob)_workerData.Item1.saveData.job));
            
            workers.Remove(_workerData);
        }

        public bool CanGetWorker()
        {
            return (workers.Count != 0);
        }
        
        public void VillagerGotoWorkResource(Tuple<Villager_System_Script, PeopleJob> _villagerData, Static_Object_Resource_System_Script _resourceSystemScript)
        {
            _villagerData.Item1._constructionSystem = _resourceSystemScript;
            _resourceSystemScript.building_villagers.Add(new Tuple<Villager_System_Script, PeopleJob>(_villagerData.Item1, (PeopleJob)_villagerData.Item1.saveData.job));
            
            villagers.Remove(_villagerData);
        }
        
        public void WorkerGotoWorkResource(Tuple<Worker_System_Script, PeopleJob> _workerData, Static_Object_Resource_System_Script _resourceSystemScript)
        {
            _workerData.Item1._constructionSystem = _resourceSystemScript;
            _resourceSystemScript.building_workers.Add(new Tuple<Worker_System_Script, PeopleJob>(_workerData.Item1, (PeopleJob)_workerData.Item1.saveData.job));
            
            workers.Remove(_workerData);
        }
        
        public void VillagerNotWorking(Tuple<Villager_System_Script, PeopleJob> _villageDatas)
        {
            _villageDatas.Item1.peopleJob = PeopleJob.Unemployed;
            _villageDatas.Item1._constructionSystem = null;
            
            villagers.Add(new Tuple<Villager_System_Script, PeopleJob>(_villageDatas.Item1, _villageDatas.Item1.peopleJob));
            
        }
        
        public void WorkerNotWorking(Tuple<Worker_System_Script, PeopleJob> _workerDatas)
        {
            _workerDatas.Item1.peopleJob = PeopleJob.Unemployed;
            _workerDatas.Item1._constructionSystem = null;
            
            workers.Add(new Tuple<Worker_System_Script, PeopleJob>(_workerDatas.Item1, _workerDatas.Item1.peopleJob));
        }
    }
}