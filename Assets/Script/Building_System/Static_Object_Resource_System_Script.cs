using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public abstract class Static_Object_Resource_System_Script : MonoBehaviour, IConstruction_System, IMenuInteractable
    {
        [SerializeField] protected Static_Resource_Preset m_Resource_Preset;
        [SerializeField] protected int m_id_Resource;
        
        protected Static_Resource_SaveData _staticResourceSaveData = new ();
        protected GameManager GM;
        protected TimeManager TM;
        protected ResourcesManager RM;
        protected HumanResourceManager HRM;
        protected Dictionary<UnityAction<object>, Static_Resource_Setting_Data> _actions = new ();
        protected List<UnityAction<object>> add_action = new List<UnityAction<object>>();
        protected List<Button_Action_Data> _buttonActionDatas = new List<Button_Action_Data>();
        protected Static_Resource_info_struct _information_Datas = new ();
        protected List<object> list_setting_values = new List<object>();
        protected List<Tuple<object, object, string>> list_information_values = new List<Tuple<object, object, string>>();
        protected List<Static_Resource_Information_Data> BI_datas = new();
        protected List<Tuple<Villager_System_Script, PeopleJob>> villagers = new();
        protected List<Tuple<Worker_System_Script, PeopleJob>> workers = new();

        public int villager_count
        {
            get => villagers.Count;
        }

        public int worker_count
        {
            get => workers.Count;
        }
        
        public int villager_data_count
        {
            get => _staticResourceSaveData.villagers.Count;
        }

        public int worker_data_count
        {
            get => _staticResourceSaveData.workers.Count;
        }

        public int people_Max
        {
            get => m_Resource_Preset.max_people;
        }
        
        public int worker_Max
        {
            get => m_Resource_Preset.max_worker;
        }

        public string name
        {
            get
            {
                return m_Resource_Preset.name;
            }
        }

        public PeopleJob job
        {
            get => m_Resource_Preset.job;
            private set => m_Resource_Preset.job = value;
        }
        
        public Static_Resource_info_struct buildingInfoStruct
        {
            get
            {
                if(_information_Datas.informations == null)
                    _information_Datas.informations = new ();
                
                if(_information_Datas.status == null)
                    _information_Datas.status = new ();
                
                return _information_Datas;
            }
        }
        
        public List<Button_Action_Data> buildingButtonActionDatas
        {
            get => _buttonActionDatas;
        }

        public Dictionary<UnityAction<object>, Static_Resource_Setting_Data> actionsBuilding
        {
            get => _actions;
        }
        
        public List<Tuple<Villager_System_Script, PeopleJob>> building_villagers
        {
            get => villagers;
            set => villagers = value;
        }

        public List<Tuple<Worker_System_Script, PeopleJob>> building_workers
        {
            get => workers;
            set => workers = value;
        }
        
        protected float efficiency
        {
            get => (Get_Villager_Efficiency() + Get_Worker_Efficiency()) / 2;
        }
        
        public bool Get_WrokOverTime()
        {
            return _staticResourceSaveData.is_work_overtime;
        }

        public bool Get_Wrok24H()
        {
            return _staticResourceSaveData.is_work_24h;
        }

        public virtual List<Button_Action_Data> GetInteractAction()
        {
            List<Button_Action_Data> menuDatas = new List<Button_Action_Data>();
            
            //Work Overtime 8/12 hour
            if(_staticResourceSaveData.is_work_overtime)
                menuDatas.Add(new Button_Action_Data("Work 8 hour", Resources.Load<Sprite>("Icon/clock"), () => { SetWorkOverTime(0);}));
            else
                menuDatas.Add(new Button_Action_Data("Work 12 Hour", Resources.Load<Sprite>("Icon/overtime"), () => { SetWorkOverTime(0);}));
            
            //Work Overtime 24 hour
            if(_staticResourceSaveData.is_work_24h)
                menuDatas.Add(new Button_Action_Data("Stop work 24 hour", Resources.Load<Sprite>("Icon/clock"), () => { SetWork24h();}));
            else
                menuDatas.Add(new Button_Action_Data("Work 24 Hour", Resources.Load<Sprite>("Icon/24H"), () => { SetWork24h();}));

            return menuDatas;
        }

        private void Awake()
        {
            GM = GameManager.Instance;
            TM = TimeManager.Instance;
            RM = ResourcesManager.Instance;
            HRM = HumanResourceManager.Instance;
            
            if (!GM.gameInstance.check_id_ResourceSaveData(m_id_Resource))
            {
                print("Non Save");
                _staticResourceSaveData.id = m_id_Resource;
                _staticResourceSaveData.villagers = new List<PeopleSystemSaveData>();
                _staticResourceSaveData.workers = new List<PeopleSystemSaveData>();
                GM.gameInstance.staticResourceSaveDatas.Add(_staticResourceSaveData);
            }
        }

        private void Start()
        {
            Create_button_action_data_for_building();
        }

        protected virtual void Update()
        {
            ResourceProductRate();
        }
        
        public bool Get_Construction_Active()
        {
            return true;
        }

        private void Create_button_action_data_for_building()
        {
            //Static_Resource info
            BI_datas = new List<Static_Resource_Information_Data>();

            foreach (var BI in m_Resource_Preset.m_static_resource_information)
            {
                BI_datas.Add(
                    new Static_Resource_Information_Data(BI.title, BI.text, Building_Information_Type.ShowInformation));
            }

            _information_Datas.informations = BI_datas;
            
            //Static_Resource Status
            BI_datas = new List<Static_Resource_Information_Data>();
            BI_datas.Add(new Static_Resource_Information_Data("สิ่งก่อสร้างไม่พร้อมใช้งาน", "กำลังก่อสร้างเสร็จสิ้นแล้ว 0%",
                Building_Information_Type.ShowStatus));


            //Static_Resource_setting_danger
            _buttonActionDatas = new List<Button_Action_Data>();
            _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                () => { SetWork24h(); print("24HHHHHH"); }));

            add_action = new List<UnityAction<object>>();
            BeginStart();
            _information_Datas.status = BI_datas;
            add_action.Add(RemoveAndAddPeople);
            add_action.Add(RemoveAndAddWorker);
            add_action.Add(SetWorkOverTime);

            EndStart();

            _actions = new();
            print("Is NoooottttNuuuuulllllllllll : " + m_Resource_Preset.m_static_resource_setting != null);
            int i = 0;
            foreach (var resourceSetting in m_Resource_Preset.m_static_resource_setting)
            {
                print("IIIIIIIIIIIIIII : " + i);
                _actions.Add(add_action[i], resourceSetting);
                i++;
            }
        }
        
        public abstract void OnEnableBuilding();
        public abstract void OnDisableBuilding();

        protected virtual void ResourceProductRate()
        {
            if (m_Resource_Preset.product_output_use_rate > 0)
            {
                if (_staticResourceSaveData.re_userate_hour <= TM.To_TotalHour(TM.get_DateTime))
                {
                    //print("Re Use Rate/Hour : " + buildingSaveData.re_userate_hour + " Hour Now : " + TM.To_TotalHour(TM.get_DateTime));
                    _staticResourceSaveData.re_userate_hour = TM.To_TotalHour(TM.get_DateTime) + m_Resource_Preset.product_output_use_rate;
                    //print("----Resources :::::::::: " + buildingSaveData.re_userate_hour);
                    Resource_product();
                }
            }
        }
        
        protected float Get_Villager_Efficiency()
        {
            float average_efficiency = 0;
            Parallel.ForEach(villagers, vill_g =>
            {
                average_efficiency += vill_g.Item1.efficiency;
            });

            return average_efficiency / m_Resource_Preset.max_people;
        }

        protected float Get_Worker_Efficiency()
        {
            float average_efficiency = 0;
            Parallel.ForEach(workers, wor_ker =>
            {
                average_efficiency += wor_ker.Item1.efficiency;
            });

            return average_efficiency / m_Resource_Preset.max_worker;
        }

        public abstract void Resource_product();

        public void Resource_usage()
        {
            
        }

        public virtual void Interact()
        {
            print("Innnnteerrraaacccttt : " + name);
        }

        public virtual void ChangeEnableBuilding()
        {
            print("OnChangeEnableBuilding : " + name);
        }

        public void OnRemoveBuilding(bool is_Cancel_Remove = false)
        {
            
        }

        public void SetWorkOverTime(object obj)
        {
            _staticResourceSaveData.is_work_overtime = !_staticResourceSaveData.is_work_overtime;
            if (_staticResourceSaveData.is_work_overtime)
                _staticResourceSaveData.is_work_24h = false;
        }

        public void SetWork24h()
        {
            _staticResourceSaveData.is_work_24h = !_staticResourceSaveData.is_work_24h;
            if (_staticResourceSaveData.is_work_24h)
                _staticResourceSaveData.is_work_overtime = false;
        }
        
        public int Get_Air_Filtration_Ability()
        {
            return 0;
        }
        
        public void RemoveAndAddPeople(object number)
        {
            //print("Busssssssssssssssssssssssssssssssssss : " + (int)number);

            if ((villager_count + (int)number) <= people_Max && (villager_count + (int)number) >= 0)
            {
                if ((int)number > 0 && HRM.CanGetVillager())
                {
                    Tuple<Villager_System_Script, PeopleJob> villager = HRM.GetVillager();
                    villager.Item1.saveData.job = (byte)m_Resource_Preset.job; 
                    HRM.VillagerGotoWorkResource(villager, this);
                    
                    _staticResourceSaveData.villagers.Add(villager.Item1.saveData);
                }
                if ((int)number < 0 && villagers.Count > 0)
                {
                    HRM.VillagerNotWorking(
                        new Tuple<Villager_System_Script, PeopleJob>(
                            villagers[0].Item1, 
                            PeopleJob.Unemployed));
                    
                    _staticResourceSaveData.villagers.Remove(villagers[0].Item1.saveData);
                    villagers.Remove(villagers[0]);
                }
            }
        }
        
        public void RemoveAndAddWorker(object number)
        {
            //print("Busssssssssssssssssssssssssssssssssss : " + (int)number);
            
            if((worker_count + (int)number) <= worker_Max && (worker_count + (int)number) >= 0)
            {
                if ((int)number > 0 && HRM.CanGetWorker())
                {
                    Tuple<Worker_System_Script, PeopleJob> worker = HRM.GetWorker();
                    worker.Item1.saveData.job = (byte)m_Resource_Preset.job;
                    HRM.WorkerGotoWorkResource(worker, this);
                    
                    _staticResourceSaveData.workers.Add(worker.Item1.saveData);
                }
                if ((int)number < 0 && workers.Count > 0)
                {
                    HRM.WorkerNotWorking(
                        new Tuple<Worker_System_Script, PeopleJob>(
                            workers[0].Item1, 
                            PeopleJob.Unemployed));
                    
                    _staticResourceSaveData.workers.Remove(workers[0].Item1.saveData);
                    workers.Remove(workers[0]);
                }
            }
        }
        
        public void SetPeopleDatasOnGaneLoad()
        {
            Villager_Object_Pool_Script _villagerObjectPool = FindObjectOfType<Villager_Object_Pool_Script>();
            Worker_Object_Pool_Script _workerObjectPool = FindObjectOfType<Worker_Object_Pool_Script>();
            
            foreach (var villager in _staticResourceSaveData.villagers)
            {
                People_System_Script _peopleSystemScript = _villagerObjectPool.Spawn(villager);
                _peopleSystemScript._constructionSystem = this;
                villagers.Add(new Tuple<Villager_System_Script, PeopleJob>((Villager_System_Script)_peopleSystemScript, job));
                GM.gameInstance.villagerSaveDatas.Add(villager);
            }

            foreach (var worker in _staticResourceSaveData.workers)
            {
                People_System_Script _peopleSystemScript = _workerObjectPool.Spawn(worker);
                _peopleSystemScript._constructionSystem = this;
                workers.Add(new Tuple<Worker_System_Script, PeopleJob>((Worker_System_Script)_peopleSystemScript, job));
                GM.gameInstance.workerSaveDatas.Add(worker);
            }
        }
        public Button_Action_Data GetUpdateButtonAction(int index)
        {
            _buttonActionDatas = new List<Button_Action_Data>();

            //Over Time 24H
            //Over Time 24H
            ColorBlock _colorBlock = new ColorBlock();
            _colorBlock.highlightedColor = new Color(150, 0, 0, 240);
            _colorBlock.pressedColor = new Color(100, 0, 0, 200);
            _colorBlock.selectedColor = new Color(150, 0, 0, 240);
            _colorBlock.disabledColor = new Color(0, 0, 0, 0);
            _colorBlock.colorMultiplier = 1;
            _colorBlock.fadeDuration = 0.1f;
            if (_staticResourceSaveData.is_work_24h)
            {
                _colorBlock.normalColor = new Color(100, 0, 0, 200);
                _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                    () => {SetWork24h(); /*print("24HHHHHH");*/ }, _colorBlock));
            }
            else
            {
                _colorBlock.normalColor = new Color(0, 0, 0, 200);
                _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                    () => {SetWork24h(); /*print("24HHHHHH");*/ }, _colorBlock));
            }

            return _buttonActionDatas[index];
        }

        public object GetValueBuilingSetting(int index)
        {
            list_setting_values = new List<object>();
            list_setting_values.Add(new Tuple<float, float>(villager_count, m_Resource_Preset.max_people));
            list_setting_values.Add(new Tuple<float, float>(worker_count, m_Resource_Preset.max_worker));
            list_setting_values.Add(_staticResourceSaveData.is_work_overtime);
            OnUpdateSettingValue();
            
            return list_setting_values[index];
        }

        public Tuple<object, object, string> GetValueBuildingInformation(int index)
        {
            list_information_values = new List<Tuple<object, object, string>>();
            list_information_values.Add(new Tuple<object, object, string>(0, 0, null));
            
            OnUpdateInformationValue();
            //print("List Info Value : " + list_information_values[index]);
            return list_information_values[index];

        }

        protected abstract void OnUpdateSettingValue();
        protected abstract void OnUpdateInformationValue();
        
        public abstract void BeginStart();
        public abstract void EndStart();

        public void OnGameLoad()
        {
            if (GM.gameInstance.check_id_ResourceSaveData(m_id_Resource))
            {
                print("Load Save");
                _staticResourceSaveData = GM.gameInstance.Get_Static_Resource_SaveData(m_id_Resource);
            }
            
            if(_staticResourceSaveData.re_userate_hour <= 0)
                _staticResourceSaveData.re_userate_hour = TM.To_TotalHour(TM.get_DateTime) + m_Resource_Preset.product_output_use_rate;

            OnBeginPlace();
            OnBeginPlace();
            
            SetPeopleDatasOnGaneLoad();
        }
        
        public virtual void Check_Surround_Road()
        {
            
        }

        public abstract void OnBeginPlace();
        public abstract void OnEndPlace();

        public abstract void OnBeginRemove();
        public abstract void OnEndRemove();

        private void OnDestroy()
        {
            OnDestroyBuilding();
            
            if(GM != null && _staticResourceSaveData != null)
                GM.gameInstance.staticResourceSaveDatas.Remove(_staticResourceSaveData);
        }
        
        public virtual void OnRemovePeople<T>(People_System_Script _peopleSystemScript, PeopleJob _job)
        {
            if (typeof(T) == typeof(Villager_System_Script))
                villagers.Remove(new Tuple<Villager_System_Script, PeopleJob>((Villager_System_Script)_peopleSystemScript, _job));
                
            if(typeof(T) == typeof(Worker_System_Script))
                workers.Remove(new Tuple<Worker_System_Script, PeopleJob>((Worker_System_Script)_peopleSystemScript, _job));
        }

        public abstract void OnDestroyBuilding();
    }
}