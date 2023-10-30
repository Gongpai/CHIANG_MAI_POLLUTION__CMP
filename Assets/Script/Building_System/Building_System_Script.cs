using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Outline.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GDD
{
    public abstract class Building_System_Script : MonoBehaviour, IConstruction_System, IMenuInteractable
    {
        [SerializeField] protected BuildingType enum_buildingType;
        [SerializeField] protected Building_Preset m_Preset;
        [SerializeField] protected GameObject m_construction_zone;
        [SerializeField] protected GameObject m_construction_progress;
        [SerializeField] protected GameObject m_warning_noti;
        [SerializeField] protected List<GameObject> m_building_Objects = new List<GameObject>();
        [SerializeField] private Sprite m_icon;
        [SerializeField] private Sprite m_bg_card;
        [SerializeField] private GameObject m_waypoint;
        [SerializeField] protected GameObject m_human_boy;
        [SerializeField] protected GameObject m_human_girl;
        [SerializeField] private string m_unlock_code = "0/0";
        [SerializeField] protected AudioSource m_audio_building_sfx;
        [SerializeField] protected AudioSource m_audio_remove_building;
        
        protected BuildingSaveData _buildingSaveData = new BuildingSaveData();
        protected GameManager GM;
        protected TimeManager TM;
        protected ResourcesManager RM;
        protected HumanResourceManager HRM;
        protected Dictionary<UnityAction<object>, Building_Setting_Data> _actions;
        protected List<UnityAction<object>> add_action = new List<UnityAction<object>>();
        protected List<Button_Action_Data> _buttonActionDatas = new List<Button_Action_Data>();
        protected Building_info_struct _information_Datas = new Building_info_struct();
        protected List<object> list_setting_values = new List<object>();
        protected List<Tuple<object, object, string>> list_information_values = new List<Tuple<object, object, string>>();
        protected List<Building_Information_Data> BI_datas = new List<Building_Information_Data>();
        protected List<Tuple<Villager_System_Script, PeopleJob>> villagers = new();
        protected List<Tuple<Worker_System_Script, PeopleJob>> workers = new();
        protected List<Button_Action_Data> menuDatas = new List<Button_Action_Data>();
        private List<Transform> _ai_pos_back_homes = new ();
        protected Building_Active _buildingActive;
        protected GameObject construction_zone_pivot;
        protected bool is_addSettingother = true;
        protected bool is_cant_use_resource = false;
        protected bool is_cant_use_power;
        protected bool is_road_found;
        protected bool is_spawn;
        private bool is_set_play_sound = false;
        private LayerMask L_Road;
        private LayerMask L_Default;
        private LayerMask L_Building;
        private LayerMask L_Obstacle;
        
        private bool is_set_enable = false;
        private bool is_set_disable = false;
        
        //New System
        protected Tuple<Vector3, Vector3, float> raycast_data;

        public Building_Preset building_preset
        {
            get => m_Preset;
        }
        
        public BuildingType _buildingType
        {
            get { return enum_buildingType; }
            set { enum_buildingType = value; }
        }

        public List<GameObject> building_Objects
        {
            get => m_building_Objects;
        }

        public Sprite icon
        {
            get => m_icon;
        }

        public Sprite bg_card
        {
            get => m_bg_card;
        }
        
        public bool active
        {
            get => _buildingSaveData.Building_active;
            set => _buildingSaveData.Building_active = value;
        }
        
        public string unlock_code
        {
            get => m_unlock_code;
        }

        public bool building_is_active
        {
            get => active && !disable && is_road_found;
        }

        protected bool auto_disable
        {
            get => _buildingSaveData.is_auto_disable;
            set => _buildingSaveData.is_auto_disable = value;
        }
        
        protected bool disable
        {
            get => _buildingSaveData.Building_Disable;
            set => _buildingSaveData.Building_Disable = value;
        }

        public bool cant_use_power
        {
            get => is_cant_use_power;
            set => is_cant_use_power = value;
        }

        protected bool is_placed
        {
            get => _buildingSaveData.building_is_placed;
            set => _buildingSaveData.building_is_placed = value;
        }

        protected bool is_construction_in_progress
        {
            get => _buildingSaveData.construction_is_in_progress;
            set => _buildingSaveData.construction_is_in_progress = value;
        }

        protected bool is_remove_building
        {
            get => _buildingSaveData.building_is_remove;
            set => _buildingSaveData.building_is_remove = value;
        }

        public bool construction_in_progress
        {
            get => is_construction_in_progress;
        }

        public float get_power_use
        {
            get
            {
                if(_buildingSaveData.Air_purifier_Speed_Up)
                    return building_preset.power_use + 1;
                else
                    return building_preset.power_use;
            }
        }
        
        public int villager_count
        {
            get => villagers.Count;
        }

        public int villager_data_count
        {
            get => _buildingSaveData.villagers.Count;
        }

        public int worker_count
        {
            get => workers.Count;
        }

        public int worker_data_count
        {
            get => _buildingSaveData.workers.Count;
        }

        public int people_Max
        {
            get => m_Preset.max_people;
        }
        
        public int worker_Max
        {
            get => m_Preset.max_worker;
        }

        protected float construction_progess_percent
        {
            get => (buildingSaveData.construction_In_Progress / 3600) / m_Preset.time_construction;
        }

        public float efficiency
        {
            get => (Get_Villager_Efficiency() + Get_Worker_Efficiency()) / 2;
        }

        public BuildingSaveData buildingSaveData
        {
            get => _buildingSaveData;
            set => _buildingSaveData = value;
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

        public List<Transform> ai_pos_back_homes
        {
            get => _ai_pos_back_homes;
            set => _ai_pos_back_homes = value;
        }
        
        public PeopleJob job
        {
            get => m_Preset.job;
            private set => m_Preset.job = value;
        }

        public Vector3 waypoint_pos
        {
            get => m_waypoint.transform.position;
        }

        public Transform waypoint
        {
            get => m_waypoint.transform;
        }
        
        public string name
        {
            get => m_Preset.name;
        }

        public string path
        {
            get
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                return _buildingSaveData.pathObject;
            }
            set
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                _buildingSaveData.pathObject = value;
            }
        }

        public Building_info_struct buildingInfoStruct
        {
            get
            {
                if(_information_Datas.informations == null)
                    _information_Datas.informations = new List<Building_Information_Data>();
                
                if(_information_Datas.status == null)
                    _information_Datas.status = new List<Building_Information_Data>();
                
                return _information_Datas;
            }
        }
        
        public List<Button_Action_Data> buildingButtonActionDatas
        {
            get => _buttonActionDatas;
        }

        public Dictionary<UnityAction<object>, Building_Setting_Data> actionsBuilding
        {
            get => _actions;
        }

        public GameObject Construction_Progress_Object
        {
            get => construction_zone_pivot;
        }

        public virtual List<Button_Action_Data> GetInteractAction()
        {
            menuDatas = new List<Button_Action_Data>();
            
            //On/Off Building
            if(active)
                menuDatas.Add(new Button_Action_Data("Turn Off", Resources.Load<Sprite>("Icon/lightbulb_Off"), () => { SetActiveBuilding(0);}));
            else
                menuDatas.Add(new Button_Action_Data("Turn On", Resources.Load<Sprite>("Icon/lightbulb_On"), () => { SetActiveBuilding(0);}));
            
            //Air Purifier Speed
            if(_buildingSaveData.Air_purifier_Speed_Up)
                menuDatas.Add(new Button_Action_Data("Speed Down", Resources.Load<Sprite>("Icon/air_purifier_icon"), () => { SetAirPurifierSpeedUp(0);}));
            else
                menuDatas.Add(new Button_Action_Data("Speed Up", Resources.Load<Sprite>("Icon/fan_Speed"), () => { SetAirPurifierSpeedUp(0);}));

            //Add Other Interaction
            AddInteractAction();
            
            //Remove Building
            if (!is_remove_building)
            {
                menuDatas.Add(new Button_Action_Data("Remove", Resources.Load<Sprite>("Icon/construction"), () =>
                    {
                        OnRemoveBuilding();
                    }));
            }
            else
            {
                menuDatas.Add(new Button_Action_Data("Cancel remove", Resources.Load<Sprite>("Icon/close_icon"), (() =>
                {
                    OnRemoveBuilding(true);
                })));
            }

            return menuDatas;
        }

        private void Awake()
        {
            GM = GameManager.Instance;
            TM = TimeManager.Instance;
            RM = ResourcesManager.Instance;
            HRM = HumanResourceManager.Instance;
            
            if(enum_buildingType != BuildingType.Generator)
                RM.Set_Resources_Power_Use(this);
        }

        private void Start()
        {
            L_Road = LayerMask.NameToLayer("Road_Object");
            L_Default = LayerMask.NameToLayer("Default");
            L_Building = LayerMask.NameToLayer("Place_Object");
            L_Obstacle = LayerMask.NameToLayer("Obstacle_Ojbect");
            
            _buildingActive = GetComponent<Building_Active>();
            
            Create_button_action_data_for_building();
            
            /*
            villagers.Add(new Tuple<Villager_System_Script, PeopleJob>(FindObjectOfType<Villager_System_Script>(), PeopleJob.Mechanic));
            workers.Add(new Tuple<Worker_System_Script, PeopleJob>(FindObjectOfType<Worker_System_Script>(), PeopleJob.Mechanic));
            print("EFF VILL : " + Get_Villager_Efficiency());
            print("EFF WORK : " + Get_Worker_Efficiency());
            */
        }

        protected virtual void Update()
        {
            //print("Is Active : " + _buildingSaveData.Building_active);
            if (raycast_data != null)
            {
                Single_Raycast_Check_Surround_Road();
            }
            else
            {
                Check_Surround_Road();
            }

            OnProgress();

            ResourceUsageRate();

            Building_active();

            Check_power_resource();

            GetValueBuildingInformation(0);
            
            float timehour = 16;
            float timehour_out = 17;
            if (_buildingSaveData.is_work_overtime)
            {
                timehour = 18;
                timehour_out = 19;
            }
            
            if (TM.getGameTimeHour == timehour && !is_spawn && building_is_active && !_buildingSaveData.is_work_24h)
            {
                SpawnPeopleBackToHome();
                is_spawn = true;
            }
            else if(TM.getGameTimeHour > timehour_out)
            {
                is_spawn = false;
            }
        }

        protected void SpawnPeopleBackToHome()
        {
            for (int i = 0; i < _ai_pos_back_homes.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        m_human_boy.transform.position = waypoint.position;
                        m_human_girl.transform.position = waypoint.position;
                        break;
                    case 1:
                        m_human_boy.transform.position = waypoint.position + (waypoint.right * 0.1f);
                        m_human_girl.transform.position = waypoint.position + (waypoint.right * 0.1f);
                        break;
                    case 2:
                        m_human_boy.transform.position = waypoint.position - (waypoint.right * 0.1f);
                        m_human_girl.transform.position = waypoint.position - (waypoint.right * 0.1f);
                        break;
                }

                float random_type = Random.Range(0, 1);
                if (random_type == 0)
                {
                    if (_ai_pos_back_homes != null)
                    {
                        float random_gender = Random.Range(0, 1);
                        GameObject spawn = null;

                        if (random_gender == 0)
                            spawn = Instantiate(m_human_boy);
                        else
                            spawn = Instantiate(m_human_girl);

                        spawn.transform.position = waypoint.position;
                        WaypointReachingState waypointReachingState = spawn.GetComponent<WaypointReachingState>();
                        waypointReachingState.waypoints.Add(_ai_pos_back_homes[i].GetComponent<Building_System_Script>().waypoint);
                        waypointReachingState.SetWaypointIndex = 0;
                        waypointReachingState.EnterState();
                        waypointReachingState.is_Start = true;

                        print("Spawn PP Building");
                    }
                }
            }

            _ai_pos_back_homes = new List<Transform>();
        }

        private void Create_button_action_data_for_building()
        {
            //Building info
            BI_datas = new List<Building_Information_Data>();

            foreach (var BI in m_Preset.m_building_information)
            {
                BI_datas.Add(new Building_Information_Data(BI.title, BI.text, Building_Information_Type.ShowInformation, Building_Show_mode.TextOnly));
            }

            _information_Datas.informations = BI_datas;

            //Building setting
            add_action = new List<UnityAction<object>>();
            add_action.Add(SetActiveBuilding);

            //Building Status
            BI_datas = new List<Building_Information_Data>();
            BI_datas.Add(new Building_Information_Data("สิ่งก่อสร้างไม่พร้อมใช้งาน", "กำลังก่อสร้างเสร็จสิ้นแล้ว 0%", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[0].title, m_Preset.m_building_status[0].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            
            //Building_setting_danger
            _buttonActionDatas = new List<Button_Action_Data>();
            _buttonActionDatas.Add(new Button_Action_Data("remove", Resources.Load<Sprite>("Icon/remove_home"),
                () => { OnRemoveBuilding(); }));

            RectTransform _rectTransform_warning = m_warning_noti.GetComponent<RectTransform>();
            _rectTransform_warning.sizeDelta = Vector2.zero;
            _rectTransform_warning.anchoredPosition = Vector2.zero;
            Canvas _warningCanvas = m_warning_noti.GetComponent<Canvas>();
            _warningCanvas.sortingOrder = 0;
            _warningCanvas.planeDistance = 1f;
            _warningCanvas.worldCamera = Camera.main;
            
            BeginStart();
            _information_Datas.status = BI_datas;

            if (is_addSettingother)
            {
                add_action.Add(SetAirPurifierSpeedUp);
                add_action.Add(SetWorkOverTime);
                add_action.Add(RemoveAndAddPeople);
                add_action.Add(RemoveAndAddWorker);
            }

            EndStart();

            _actions = new();
            //print("Is NoooottttNuuuuulllllllllll : " + m_Preset.m_building_setting != null);
            int i = 0;
            foreach (var buildingSetting in m_Preset.m_building_setting)
            {
                _actions.Add(add_action[i], buildingSetting);
                i++;
            }
        }

        protected float Get_Villager_Efficiency()
        {
            float average_efficiency = 0;
            Parallel.ForEach(villagers, vill_g =>
            {
                average_efficiency += vill_g.Item1.efficiency;
            });

            return average_efficiency / m_Preset.max_people;
        }

        protected float Get_Worker_Efficiency()
        {
            float average_efficiency = 0;
            Parallel.ForEach(workers, wor_ker =>
            {
                average_efficiency += wor_ker.Item1.efficiency;
            });

            return average_efficiency / m_Preset.max_worker;
        }

        public virtual bool Get_WrokOverTime()
        {
            return _buildingSaveData.is_work_overtime;
        }

        public virtual bool Get_Wrok24H()
        {
            return _buildingSaveData.is_work_24h;
        }

        public int Get_Air_Filtration_Ability()
        {
            int air_speed = building_preset.air_filtration_ability;

            if (GM.gameInstance.TUDataSave.air_purifier_leveltwo && !GM.gameInstance.TUDataSave.air_purifier_levelthree)
                air_speed += building_preset.air_filtration_ability + 100;
            if (!GM.gameInstance.TUDataSave.air_purifier_leveltwo && GM.gameInstance.TUDataSave.air_purifier_levelthree)
                air_speed += building_preset.air_filtration_ability + 100;
            if (GM.gameInstance.TUDataSave.air_purifier_leveltwo && GM.gameInstance.TUDataSave.air_purifier_levelthree)
                air_speed += building_preset.air_filtration_ability + 200;
            
            if (_buildingSaveData.Air_purifier_Speed_Up)
                return air_speed + 100;
            else
                return air_speed;
        }

        private void Check_power_resource()
        {
            if (enum_buildingType != BuildingType.Generator)
            {
                if (!is_cant_use_power)
                {
                    is_cant_use_power = true;
                    disable = false;
                }
                else if(m_Preset.power_use > 0)
                {
                    is_cant_use_power = false;
                    disable = true;
                }
            }
        }

        private void Building_active()
        {
            if (construction_zone_pivot != null && !construction_zone_pivot.activeSelf)
            {
                if (building_is_active)
                {
                    if (!is_set_enable)
                    {
                        is_set_disable = false;
                        is_set_enable = true;
                        OnEnableBuilding();

                        if (!is_set_play_sound)
                        {
                            m_audio_building_sfx.Play();
                            is_set_play_sound = true;
                        }
                    }
                }
                else
                {
                    if (!is_set_disable)
                    {
                        is_set_disable = true;
                        is_set_enable = false;
                        OnDisableBuilding();
                        if (is_set_play_sound)
                        {
                            m_audio_building_sfx.Stop();
                            is_set_play_sound = false;
                        }
                    }
                }
            }
            else
            {
                if (!is_set_disable && is_remove_building && !building_is_active)
                {
                    is_set_disable = true;
                    is_set_enable = false;
                    OnDisableBuilding();
                    if (is_set_play_sound)
                    {
                        m_audio_building_sfx.Stop();
                        is_set_play_sound = false;
                    }
                }
            }
        }

        public bool Get_Construction_Active()
        {
            return building_is_active;
        }

        public abstract void OnEnableBuilding();
        public abstract void OnDisableBuilding();

        protected virtual void ResourceUsageRate()
        {
            if (m_Preset.resources_use_rate > 0 && building_is_active && !disable)
            {
                if (buildingSaveData.re_userate_hour <= TM.To_TotalHour(TM.get_DateTime))
                {
                    //print("Re Use Rate/Hour : " + buildingSaveData.re_userate_hour + " Hour Now : " + TM.To_TotalHour(TM.get_DateTime));
                    buildingSaveData.re_userate_hour = TM.To_TotalHour(TM.get_DateTime) + m_Preset.resources_use_rate;
                    //print("----Resources :::::::::: " + buildingSaveData.re_userate_hour);
                    Resource_usage();
                }
            }
        }

        public abstract void Resource_usage();

        public void Resource_product()
        {
            
        }

        public virtual void Interact()
        {
            //print("Innnnteerrraaacccttt : " + name);
        }

        public virtual void ChangeEnableBuilding()
        {
            //print("OnChangeEnableBuilding : " + name);
        }

        public void SetActiveBuilding(object obj)
        {
            active = !active;
            if(active)
                _buildingActive.Play();
            else
                _buildingActive.Stop();
            
            auto_disable = active;
        }

        public void SetAirPurifierSpeedUp(object obj)
        {
            _buildingSaveData.Air_purifier_Speed_Up = !_buildingSaveData.Air_purifier_Speed_Up;
        }

        public void SetWorkOverTime(object obj)
        {
            _buildingSaveData.is_work_overtime = !_buildingSaveData.is_work_overtime;
            if (_buildingSaveData.is_work_overtime)
                _buildingSaveData.is_work_24h = false;
        }

        public void SetWork24h()
        {
            _buildingSaveData.is_work_24h = !_buildingSaveData.is_work_24h;
            if (_buildingSaveData.is_work_24h)
                _buildingSaveData.is_work_overtime = false;
        }
        
        public void RemoveAndAddPeople(object number)
        {
            print("Busssssssssssssssssssssssssssssssssss : " + (int)number);

            if ((villager_count + (int)number) <= people_Max && (villager_count + (int)number) >= 0)
            {
                print("Villager Add");
                if ((int)number > 0 && HRM.CanGetVillager())
                {
                    Tuple<Villager_System_Script, PeopleJob> villager = HRM.GetVillager();
                    villager.Item1.saveData.job = (byte)job;
                    HRM.VillagerGotoWorkBuilding(villager, this);

                    _buildingSaveData.villagers.Add(villager.Item1.saveData);
                }
                if ((int)number < 0 && villagers.Count > 0)
                {
                    HRM.VillagerNotWorking(
                        new Tuple<Villager_System_Script, PeopleJob>(
                            villagers[0].Item1, 
                            PeopleJob.Unemployed));
                    
                    _buildingSaveData.villagers.Remove(villagers[0].Item1.saveData);
                    villagers.Remove(villagers[0]);
                }
            }
        }
        
        public void RemoveAndAddWorker(object number)
        {
            print("Busssssssssssssssssssssssssssssssssss : " + (int)number);
            
            if((worker_count + (int)number) <= worker_Max && (worker_count + (int)number) >= 0)
            {
                print("Worker Add");
                if ((int)number > 0 && HRM.CanGetWorker())
                {
                    Tuple<Worker_System_Script, PeopleJob> worker = HRM.GetWorker();
                    worker.Item1.saveData.job = (byte)job;
                    HRM.WorkerGotoWorkBuilding(worker, this);
                    
                    _buildingSaveData.workers.Add(worker.Item1.saveData);
                }
                if ((int)number < 0 && workers.Count > 0)
                {
                    HRM.WorkerNotWorking(
                        new Tuple<Worker_System_Script, PeopleJob>(
                            workers[0].Item1, 
                            PeopleJob.Unemployed));
                    
                    _buildingSaveData.workers.Remove(workers[0].Item1.saveData);
                    workers.Remove(workers[0]);
                }
            }
        }

        public virtual void OnRemoveBuilding(bool is_Cancel_Remove = false)
        {
            bool can_remove_now = false;
            if (!is_remove_building)
            {
                GameObject audio_remove = new GameObject();
                audio_remove.name = "Building_Remove_Audio";
                audio_remove.AddComponent<AudioSource>().clip = m_audio_remove_building.clip;
                audio_remove.GetComponent<AudioSource>().Play();
                
                Destroy(audio_remove, 1.0f);
                can_remove_now = true;
            }
            else
            {
                can_remove_now = false;
            }
            
            if (is_Cancel_Remove && construction_progess_percent <= 0.5f)
            {
                can_remove_now = false;
                is_remove_building = true;
            }
            else
            {
                is_remove_building = !is_Cancel_Remove;
            }
            
            OnBeginRemove();
            
            //System script
            //-----------------------------------
            
            //-----------------------------------
            
            OnEndRemove();

            if (construction_progess_percent < 0.1f && can_remove_now)
            {
                RM.Set_Resources_Tree(m_Preset.wood_build);
                RM.Set_Resources_Rock(m_Preset.rock_build);
                Destroy(gameObject);
            }

            if (construction_progess_percent <= 0)
            {
                Destroy(gameObject);
            }
        }

        public Button_Action_Data GetUpdateButtonAction(int index)
        {
            _buttonActionDatas = new List<Button_Action_Data>();

            //Remove
            ColorBlock _colorBlock = new ColorBlock();
            _colorBlock.normalColor = new Color(0, 0, 0, 200);
            _colorBlock.highlightedColor = new Color(150, 0, 0, 240);
            _colorBlock.pressedColor = new Color(100, 0, 0, 200);
            _colorBlock.selectedColor = new Color(150, 0, 0, 240);
            _colorBlock.disabledColor = new Color(0, 0, 0, 0);
            _colorBlock.colorMultiplier = 1;
            _colorBlock.fadeDuration = 0.1f;
            if (!is_remove_building)
            {
                _buttonActionDatas.Add(new Button_Action_Data("remove", Resources.Load<Sprite>("Icon/remove_home"),
                    () => { OnRemoveBuilding(); }, _colorBlock));
            }
            else
            {
                _buttonActionDatas.Add(new Button_Action_Data("cancel_remove",
                    Resources.Load<Sprite>("Icon/construction"), () => { OnRemoveBuilding(true); }, _colorBlock));
            }

            AddUpdateButtonAction();
            
            return _buttonActionDatas[index];
        }

        public virtual void AddUpdateButtonAction()
        {
            
        }

        public object GetValueBuilingSetting(int index)
        {
            list_setting_values = new List<object>();
            list_setting_values.Add((active));

            OnUpdateSettingValue();

            if (is_addSettingother)
            {
                list_setting_values.Add(_buildingSaveData.Air_purifier_Speed_Up);
                list_setting_values.Add(_buildingSaveData.is_work_overtime);
                list_setting_values.Add(new Tuple<float, float>(villager_count, m_Preset.max_people));
                list_setting_values.Add(new Tuple<float, float>(worker_count, m_Preset.max_worker));
            }

            return list_setting_values[index];
        }

        public virtual void OnProgress()
        {
            float progess = buildingSaveData.construction_In_Progress / 3600;
            
            if ((m_Preset.time_construction > progess && !is_remove_building && is_placed) || (is_remove_building && progess >= 0))
            {
                is_construction_in_progress = true;
                if (is_remove_building)
                {
                    m_warning_noti.SetActive(true);
                    m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[0].SetActive(false);
                    m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[1].SetActive(true);
                    buildingSaveData.construction_In_Progress += TM.deltaTime * -1;
                }
                else
                {
                    buildingSaveData.construction_In_Progress += TM.deltaTime;
                }

                switch_mesh_construction_progress(true);
                m_construction_progress.SetActive(true);
                RectTransform _rectTransform_progress = m_construction_progress.GetComponent<RectTransform>();
                _rectTransform_progress.sizeDelta = Vector2.zero;
                _rectTransform_progress.anchoredPosition = Vector2.zero;
                Canvas _progressCanvas = m_construction_progress.GetComponent<Canvas>();
                _progressCanvas.planeDistance = 1f;
                _progressCanvas.worldCamera = Camera.main;
                Canvas_Element_List _progressElement = m_construction_progress.GetComponent<Canvas_Element_List>();
                _progressElement.image[0].fillAmount = construction_progess_percent;

                //print("Time Con : " + (buildingSaveData.construction_In_Progress / 3600));
                //print("Construction_In_Progress : " + (int)((buildingSaveData.construction_In_Progress / 3600) / m_Preset.time_construction * 100) + "%");
            }
            else
            {
                m_construction_progress.SetActive(false);
                is_construction_in_progress = false;

                if (construction_zone_pivot != null && construction_zone_pivot.activeSelf)
                {
                    switch_mesh_construction_progress(false);
                }

                if (is_remove_building)
                    OnRemoveBuilding();
            }
        }

        public Tuple<object, object, string> GetValueBuildingInformation(int index)
        {
            bool is_problem = false;
            list_information_values = new List<Tuple<object, object, string>>();
            
            if (is_construction_in_progress)
            {
                string construction_in_progress_text;
                if (!is_remove_building)
                {
                    construction_in_progress_text = "กำลังก่อสร้างเสร็จสิ้นแล้ว " +
                                                    (int)(construction_progess_percent * 100) + "%";
                }
                else
                {
                    construction_in_progress_text = "กำลังรื้อถอนเสร็จสิ้นแล้ว " +
                                                    (int)(100 - (construction_progess_percent * 100)) + "%";
                }

                list_information_values.Add(new Tuple<object, object, string>(
                    (buildingSaveData.construction_In_Progress / 3600), m_Preset.time_construction,
                    construction_in_progress_text));
            }
            else
            {
                list_information_values.Add(new Tuple<object, object, string>(0, 0, null));
            }

            list_information_values.Add(new Tuple<object, object, string>(active && !is_road_found, null, m_Preset.m_building_status[0].text));
            is_problem = OnUpdateInformationValue() || active && !is_road_found;

            if (is_problem)
            {
                m_warning_noti.SetActive(true);
                
                if (is_remove_building)
                {
                    m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[0].SetActive(false);
                    m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[1].SetActive(true);
                }
                else
                {
                    m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[1].SetActive(false);
                    m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[0].SetActive(true);
                }
            }
            else if (!is_remove_building)
            {
                m_warning_noti.SetActive(false);
                m_warning_noti.GetComponent<Canvas_Element_List>().canvas_gameObjects[0].SetActive(false);
            }
            
            //print("List Info Value : " + list_information_values[index]);
            return list_information_values[index];

        }

        public virtual void AddInteractAction()
        {
            
        }
        
        protected abstract void OnUpdateSettingValue();
        protected abstract bool OnUpdateInformationValue();
        
        public abstract void BeginStart();
        public abstract void EndStart();

        public void OnGameLoad()
        {
            GM = GameManager.Instance;
            RM = ResourcesManager.Instance;
            
            switch_mesh_construction_progress(true, false);
            if (!is_placed || construction_progess_percent < 1)
            {
                is_placed = true;
            }

            TM = TimeManager.Instance;
            if(_buildingSaveData.re_userate_hour <= 0)
                buildingSaveData.re_userate_hour = TM.To_TotalHour(TM.get_DateTime) + m_Preset.resources_use_rate;
            
            OnBeginPlace();
            transform.position = new Vector3(_buildingSaveData.Position.X, _buildingSaveData.Position.Y, _buildingSaveData.Position.Z);
            transform.eulerAngles = new Vector3(_buildingSaveData.Rotation.X, _buildingSaveData.Rotation.Y,_buildingSaveData.Rotation.Z);
            OnBeginPlace();
            
            SetPeopleDatasOnGaneLoad();
        }

        public bool OnPlaceBuilding()
        {
            if (!RM.Can_Set_Resources_Tree(-m_Preset.wood_build) || !RM.Can_Set_Resources_Rock(-m_Preset.rock_build))
            {
                print("Cont Place ");
                
                Notification notification = new Notification();
                notification.text = "Not enough resources";
                notification.icon = Resources.Load<Sprite>("Icon/warning_icon");
                notification.iconColor = Color.white;
                notification.duration = 5.0f;
                notification.isWaitTrigger = false;
                NotificationManager.Instance.AddNotification(notification);
                
                return false;
            }
            else
            {
                RM.Set_Resources_Rock(-m_Preset.rock_build);
                RM.Set_Resources_Tree(-m_Preset.wood_build);
                print("OnPlaceeeee ");
                is_placed = true;
                is_remove_building = false;
                switch_mesh_construction_progress(true);
                buildingSaveData.re_userate_hour = TM.To_TotalHour(TM.get_DateTime) + m_Preset.resources_use_rate;
                
                OnBeginPlace();
                _buildingSaveData.Position =
                    new Vector3D(transform.position.x, transform.position.y, transform.position.z);
                _buildingSaveData.Rotation = new Vector3D(transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                _buildingSaveData.b_buildingtype = (byte)enum_buildingType;
                GI_AddBuildingSavaData();
                OnEndPlace();

                return true;
            }
        }

        public void GI_AddBuildingSavaData()
        {
            GM.gameInstance.buildingSaveDatas.Add(_buildingSaveData);
        }

        public void SetPeopleDatasOnGaneLoad()
        {
            Villager_Object_Pool_Script _villagerObjectPool = FindObjectOfType<Villager_Object_Pool_Script>();
            Worker_Object_Pool_Script _workerObjectPool = FindObjectOfType<Worker_Object_Pool_Script>();
            
            foreach (var villager in _buildingSaveData.villagers)
            {
                People_System_Script _peopleSystemScript = _villagerObjectPool.Spawn(villager);
                _peopleSystemScript._constructionSystem = this;
                villagers.Add(new Tuple<Villager_System_Script, PeopleJob>((Villager_System_Script)_peopleSystemScript, job));
                GM.gameInstance.villagerSaveDatas.Add(villager);
            }

            foreach (var worker in _buildingSaveData.workers)
            {
                People_System_Script _peopleSystemScript = _workerObjectPool.Spawn(worker);
                _peopleSystemScript._constructionSystem = this;
                workers.Add(new Tuple<Worker_System_Script, PeopleJob>((Worker_System_Script)_peopleSystemScript, job));
                GM.gameInstance.workerSaveDatas.Add(worker);
            }
        }

        private void switch_mesh_construction_progress(bool is_progress, bool is_set_building_active = true)
        {
            if (is_progress)
            {
                if (construction_zone_pivot == null)
                {
                    construction_zone_pivot = Instantiate(m_construction_zone);
                    
                    for (int i = 0; i < construction_zone_pivot.transform.childCount; i++)
                    {
                        construction_zone_pivot.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Place_Object");
                    }
                    
                    construction_zone_pivot.layer = LayerMask.NameToLayer("Place_Object");
                    construction_zone_pivot.transform.parent = transform;
                    construction_zone_pivot.transform.localPosition = new Vector3(0, -0.5f, 0);
                    construction_zone_pivot.transform.rotation = transform.rotation;
                    Destroy(construction_zone_pivot.GetComponent<BoxCollider>());
                    construction_zone_pivot.AddComponent<AutoAddOutlinerGameObjects>();
                }

                if (is_set_building_active)
                {
                    active = false;
                }

                foreach (var building_Object in m_building_Objects)
                {
                    if (building_Object.GetComponent<Joint>() != null)
                    {
                        if (building_Object.GetComponent<MeshRenderer>() != null)
                            building_Object.GetComponent<MeshRenderer>().enabled = false;
                        else
                            building_Object.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        building_Object.SetActive(false);
                    }
                }
                construction_zone_pivot.SetActive(true);
            }
            else
            {
                if (is_set_building_active && construction_progess_percent > 0.5 && ((is_cant_use_power && enum_buildingType != BuildingType.Generator) || Check_Resource()))
                {
                    //print("Is Enable : " + disable + " Can Tree : " + Check_Resource() + " TREE CURRENT : " + GM.gameInstance.get_tree_resource());
                    active = true;
                } else if ((!is_cant_use_power && enum_buildingType != BuildingType.Generator) || !Check_Resource())
                {
                    //print("Is Disable : " + disable + " Can Tree : " + is_cant_use_resource);
                    active = true;
                    Resource_usage();
                    disable = true;
                }
                
                foreach (var building_Object in m_building_Objects)
                {
                    if (building_Object.GetComponent<Joint>() != null)
                    {
                        if (building_Object.GetComponent<MeshRenderer>() != null)
                            building_Object.GetComponent<MeshRenderer>().enabled = true;
                        else
                            building_Object.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        building_Object.SetActive(true);
                    }
                }
                construction_zone_pivot.SetActive(false);
            }
        }

        protected abstract bool Check_Resource();

        public virtual void Check_Surround_Road()
        {
            List<RaycastHit> hit_floorraycasthits = new List<RaycastHit>(4);

            List<Vector3> directions = new List<Vector3>();
            directions.Add(transform.forward);
            directions.Add(-transform.forward);
            directions.Add(transform.right);
            directions.Add(-transform.right);

            //TopLeft
            directions.Add(transform.forward);
            directions.Add(-transform.right);
            //TopRight
            directions.Add(transform.forward);
            directions.Add(transform.right);
            //BottomLeft
            directions.Add(-transform.forward);
            directions.Add(-transform.right);
            //BottomRight
            directions.Add(-transform.forward);
            directions.Add(transform.right);

            float object_size = GetComponent<Renderer>().bounds.size.z / 2;
            bool is_found_road = false;

            List<Vector3> start_point = new List<Vector3>();
            start_point.Add(new Vector3(-object_size, -0.25f, object_size));
            start_point.Add(new Vector3(object_size, -0.25f, object_size));
            start_point.Add(new Vector3(-object_size, -0.25f, -object_size));
            start_point.Add(new Vector3(object_size, -0.25f, -object_size));

            for (int i = 0; i < 4; i++)
            {
                bool is_hit = Physics.Raycast(new Ray(transform.position - new Vector3(0, 0.25f, 0), directions[i]), out RaycastHit raycasthit, object_size + 0.25f, 1 << L_Road | 0 << L_Default | 0 << L_Building | 0 << L_Obstacle);
                hit_floorraycasthits.Add(raycasthit);

                if (is_hit && raycasthit.transform.gameObject.layer == L_Road)
                {
                    raycast_data = new Tuple<Vector3, Vector3, float>(-new Vector3(0, 0.25f, 0), directions[i],
                        object_size + 0.25f);

                    print("Inside Raod Detected !!!!");
                    is_found_road = true;
                    break;
                }

                Debug.DrawLine(transform.position - new Vector3(0, 0.25f, 0), raycasthit.point, Color.red);
            }

            if (!is_found_road)
            {
                int origin_index = 0;
                for (int i = 4; i < directions.Count; i++)
                {
                    if (i % 2 == 0 && i != 4 && origin_index < 4)
                    {
                        origin_index++;
                    }

                    bool is_hit =
                        Physics.Raycast(new Ray(transform.position + start_point[origin_index], directions[i]),
                            out RaycastHit raycasthit, object_size + 0.25f,
                            1 << L_Road | 0 << L_Default | 0 << L_Building | 0 << L_Obstacle);
                    hit_floorraycasthits.Add(raycasthit);

                    //print("Origin : " + origin_index + " i : " + i);

                    Debug.DrawLine(start_point[origin_index], raycasthit.point, Color.red);

                    if (is_hit && raycasthit.transform.gameObject.layer == L_Road)
                    {
                        raycast_data = new Tuple<Vector3, Vector3, float>(start_point[origin_index], directions[i],
                            object_size + 0.25f);

                        print("Outside Raod Detected !!!!");

                        break;
                    }

                    /*
                    switch (i % 3)
                    {
                        case 0:
                            Debug.DrawLine(start_point[origin_index], start_point[origin_index] + directions[i], Color.white);
                            break;
                        case 1:
                            Debug.DrawLine(start_point[origin_index], start_point[origin_index] + directions[i], Color.green);
                            break;
                        case 2:
                            Debug.DrawLine(start_point[origin_index], start_point[origin_index] + directions[i], Color.yellow);
                            break;
                        default:
                            break;
                    }
                    */
                }

                /*
                Debug.DrawLine(transform.position + new Vector3(0, 0.25f, 0), transform.position + transform.forward, Color.blue);
                Debug.DrawLine(transform.position + new Vector3(0, 0.25f, 0), transform.position - transform.forward, Color.blue);
                Debug.DrawLine(transform.position + new Vector3(0, 0.25f, 0), transform.position + transform.right, Color.blue);
                Debug.DrawLine(transform.position + new Vector3(0, 0.25f, 0), transform.position - transform.right, Color.blue);
            */
            }
        }

        public virtual void Single_Raycast_Check_Surround_Road()
        {
            Physics.Raycast(new Ray(transform.position + raycast_data.Item1, raycast_data.Item2), out RaycastHit raycasthit, raycast_data.Item3, 1<<L_Road|0<<L_Default|0<<L_Building|0<<L_Obstacle);
            //hit_floorraycasthits.Add(raycasthit);

            is_road_found = true;
            
            //print("Enable!! Single Raycast Check Surround Road");
            
            if (raycasthit.transform == null)
            {
                //print("Raod Not Found !!!!");
                raycast_data = null;
                is_road_found = false;
            }
            else
            {
                Debug.DrawLine(transform.position + raycast_data.Item1, raycasthit.point, Color.red);
            }
        }

        public abstract void OnBeginPlace();
        public abstract void OnEndPlace();

        public abstract void OnBeginRemove();
        public abstract void OnEndRemove();

        private void OnDestroy()
        {
            OnDestroyBuilding();
            
            if(GM != null && _buildingSaveData != null)
                GM.gameInstance.buildingSaveDatas.Remove(_buildingSaveData);
            
            if(enum_buildingType != BuildingType.Generator)
                RM.Set_Resources_Power_Use(this, true);
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
