using System;
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
        
        public Health_Script _current_healthScript { get; set; }
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
                return (((hunger * 1) + (content * 1)) / 2) * efficiency_boot;
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
            get => GM.PM_25;
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
            Update_per_hour();
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
            if (hunger - 0.1f >= 0)
            {
                hunger -= 0.1f;
            }else if (hunger - 0.1f <= 0)
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
            if(hunger <= 0.1f && health - 0.1f >= 0)
            {
                health -= 0.1f;
            }
            else if(health - 0.1f <= 0)
            {
                health = 0;
                Debug.LogError("People Dead");
            }

            if (dust_pm_25 >= 150)
            {
                if (health - (((float)dust_pm_25 - 150.000000f) / 300.000000f) <= 0)
                {
                    health = 0;
                }
                else
                {
                    print("ddddd : " + ((float)(dust_pm_25 - 150.000000f) / 300.000000f));
                    health -= ((float)(dust_pm_25 - 150.000000f) / 300.000000f);
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

            if (_current_healthScript != null)
            {
                _current_healthScript.OnRecoverIllness(saveData);
                _current_healthScript = null;
            }

            _peopleSaveData = new PeopleSystemSaveData();
        }
    }
}