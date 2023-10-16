using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RandomNameGen;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = System.Random;

namespace GDD
{
    public abstract class People_System_Script : MonoBehaviour
    {
        [SerializeField] public string name_view;
        protected PeopleSystemSaveData _peopleSaveData = new PeopleSystemSaveData();
        protected ResourcesManager RM;
        protected TimeManager TM;
        protected GameManager GM;
        protected HumanResourceManager HRM;
        protected float efficiency_boot = 1;
        protected float pm25_max = 800;
        protected float hunger_value = 0.025f;
        protected float hunger_damage = 0.05f;
        
        public Health_Script _current_healthScript { get; set; }
        public People_Script _current_residentScript { get; set; }
        public IConstruction_System _constructionSystem { get; set; }
        public IObjectPool<People_System_Script> Pool { get; set; }

        public PeopleSystemSaveData saveData
        {
            get => _peopleSaveData;
            set => _peopleSaveData = value;
        }

        public string name
        {
            get => _peopleSaveData.name;
            set => _peopleSaveData.name = value;
        }
        
        public bool is_still_working
        {
            get => _peopleSaveData.is_still_working;
            set => _peopleSaveData.is_still_working = value;
        }
        
        public float efficiency
        {
            get
            {
                if (_constructionSystem.Get_WrokOverTime())
                {
                    if(TM.get_DateTime.Hour >= 6 && TM.get_DateTime.Hour < 18)
                        return (((hunger * 1) + (content * 1)) / 2) * efficiency_boot;
                    else
                        return 0;
                } else if (_constructionSystem.Get_Wrok24H())
                {
                    return (((hunger * 1) + (content * 1)) / 2) * efficiency_boot;
                }
                else
                {
                    if(TM.get_DateTime.Hour >= 8 && TM.get_DateTime.Hour < 16)
                        return (((hunger * 1) + (content * 1)) / 2) * efficiency_boot;
                    else
                        return 0;
                }
            }
        }

        public float content
        {
            get => _peopleSaveData.content;
            set => _peopleSaveData.content = value;
        }
        
        public float health
        {
            get => _peopleSaveData.health;
            set => _peopleSaveData.health = value;
        }
        
        public float hunger
        {
            get => _peopleSaveData.hunger;
            set => _peopleSaveData.hunger = value;
        }

        protected int dust_pm_25
        {
            get
            {
                if (_constructionSystem != null)
                    return GM.PM_25 - _constructionSystem.Get_Air_Filtration_Ability();
                else
                    return GM.PM_25;
            }
        }

        public PeopleDailyLife dailyLife
        {
            get => (PeopleDailyLife)_peopleSaveData.dailyLife;
            set => _peopleSaveData.dailyLife = (byte)value;
        }

        public PeopleJob peopleJob
        {
            get => (PeopleJob)_peopleSaveData.job;
            set => _peopleSaveData.job = (byte)value;
        }

        private IPeople_State _sickState, _noworkingState, _workingState;

        private People_Context_Script _peopleContextScript;

        protected virtual void Awake()
        {
            GM = GameManager.Instance;
            RM = ResourcesManager.Instance;
            TM = TimeManager.Instance;
            HRM = HumanResourceManager.Instance;
        }

        protected virtual void Start()
        {
            dailyLife = PeopleDailyLife.Working_State;
            _peopleContextScript = new People_Context_Script(this);
            _peopleSaveData.current_hour_update = TM.To_TotalHour(TM.get_DateTime) + 1;
            
            _sickState = gameObject.AddComponent<People_Sick_State>();
            _noworkingState = gameObject.AddComponent<People_NoWorking_State>();
            _workingState = gameObject.AddComponent<People_Working_State>();
            
            Random r = new Random(); 
            RandomName rn = new RandomName(r);
            name = rn.Generate(Sex.Male);
            name_view = name;
            print(" name is : " + name);
        }

        protected virtual void Update()
        {
            FindResident();
            Update_per_hour();
        }

        protected virtual void FindResident()
        {
            if (_current_residentScript == null)
            {
                List<People_Script> _peopleScripts =
                    FindObjectsByType<People_Script>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

                People_Script peopleScript = null;
                
                Parallel.ForEach(_peopleScripts, (script, state) =>
                {
                    if (script.get_people_count < script.get_people_max && script.building_is_active)
                    {
                        peopleScript = script;
                        
                        state.Break();
                    }
                });

                if (peopleScript != null)
                {
                    _current_residentScript = peopleScript;
                    _current_residentScript.OnAddPeople(_peopleSaveData);
                    HRM.residence.Add(this);
                }
            }
        }

        public virtual void SetPeopleDatatoSavaData()
        {
            Random r = new Random(); 
            RandomName rn = new RandomName(r);
            name = rn.Generate(Sex.Male);
            name_view = name;
            print(" name is : " + name);
        }
        
        private void Update_per_hour()
        {
            if (_peopleSaveData.current_hour_update <= TM.To_TotalHour(TM.get_DateTime))
            {
                //print("Re Use Rate/Hour : " + buildingSaveData.re_userate_hour + " Hour Now : " + TM.To_TotalHour(TM.get_DateTime));
                _peopleSaveData.current_hour_update = TM.To_TotalHour(TM.get_DateTime) + 1;
                //print("----Resources :::::::::: " + buildingSaveData.re_userate_hour);
                //print("------------------------------------");
                
                Hunger_Status_Controll();
                health_Status_Controll();
                
                ChangeState_System();
                _peopleContextScript.Update_per_hour();
                Content_Status_Controll();

                CheckHeathPeople();
                
                //print("Efficiency : " + efficiency);
            }
        }

        private void CheckHeathPeople()
        {
            if (health <= 0.000000f)
            {
                ReturnToPool();
            }
        }
        
        private void Hunger_Status_Controll()
        {
            if (hunger - hunger_value >= 0)
            {
                hunger -= hunger_value;
            }else if (hunger - hunger_value <= 0)
            {
                hunger = 0;
            }
                

            if (hunger <= 0.5f && RM.Can_Set_Resources_Food(-1))
            {
                RM.Set_Resources_Food(-1);
                hunger = 1;
            }
            
            //print("Hunger : " + hunger);
        }
        
        private void Content_Status_Controll()
        {
            content = (hunger + health) / 2;
            
            //print("Content : " + content);
        }

        private void health_Status_Controll()
        {
            if(hunger <= hunger_damage && health - hunger_damage >= 0)
            {
                health -= hunger_damage;
            }
            else if(health - hunger_damage <= 0)
            {
                health = 0;
                Debug.LogError("People Dead");
            }

            if (dust_pm_25 >= 150)
            {
                if (health - (((float)dust_pm_25 - 150.000000f) / pm25_max) <= 0)
                {
                    health = 0;
                }
                else
                {
                    //print("ddddd : " + ((float)(dust_pm_25 - 150.000000f) / pm25_max));
                    health -= ((float)(dust_pm_25 - 150.000000f) / pm25_max);
                }
            }

            //print("PM 2.5 : " + dust_pm_25);
            //print("Health : " + health);
        }

        protected virtual void ReturnToPool()
        {
            Pool.Release(this);
        }

        private void ChangeState_System()
        {
            if (content <= 0.1f && health > 0.1f && dailyLife != PeopleDailyLife.Sick_State)
            {
                dailyLife = PeopleDailyLife.NoWorking_State;
            }

            if (health <= 0.3f)
            {
                dailyLife = PeopleDailyLife.Sick_State;
            }

            if (health > 0.1f && content > 0.1f  && dailyLife != PeopleDailyLife.Sick_State)
            {
                dailyLife = PeopleDailyLife.Working_State;
            }
            
            
            switch (dailyLife)
            {
                case PeopleDailyLife.Sick_State:
                    OnSickState();
                    break;
                case PeopleDailyLife.NoWorking_State:
                    OnNoWorkingState();
                    break;
                case PeopleDailyLife.Working_State:
                    OnWorkingState();
                    break;
            }
        }

        private void OnSickState()
        {
            print("OnSickState");
            _peopleContextScript.Transition(_sickState);
        }

        private void OnNoWorkingState()
        {
            //print("OnNoWorkingState");
            _peopleContextScript.Transition(_noworkingState);
        }

        private void OnWorkingState()
        {
            //print("OnWorkingState");
            _peopleContextScript.Transition(_workingState);
        }

        protected void OnDisable()
        {
            OnDead();
        }

        protected virtual void OnDead()
        {
            HRM.peopleDeaths.Add(this);

            if(dailyLife == PeopleDailyLife.Sick_State && _current_healthScript == null)
                HRM.patients.Remove(this);
            if(dailyLife == PeopleDailyLife.Sick_State && _current_healthScript != null)
                HRM.healing.Remove(this);
            
            if (peopleJob == PeopleJob.Nurse)
            {
                if (dailyLife == PeopleDailyLife.Sick_State)
                {
                    print("Nurse Remove");
                    Building_System_Script buildingSystem = (Building_System_Script)_constructionSystem;
                    Health_Script healthScript = (Health_Script)buildingSystem;
                    healthScript.OnRecoverIllness(_peopleSaveData, true);
                }
            }
            else
            {
                if (_current_healthScript != null)
                {
                    _current_healthScript.OnRecoverIllness(_peopleSaveData);
                    _current_healthScript = null;
                }
            }

            if (_current_residentScript != null)
            {
                _current_residentScript.OnRemovePeople(_peopleSaveData);
                HRM.residence.Remove(this);
                _current_residentScript = null;
            }
            
            _peopleSaveData = new PeopleSystemSaveData();
        }
    }
}