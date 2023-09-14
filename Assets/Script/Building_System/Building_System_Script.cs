using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public abstract class Building_System_Script : MonoBehaviour, IConstruction_System, IMenuInteractable
    {
        [SerializeField] protected BuildingType enum_buildingType;
        [SerializeField] protected Building_Preset m_Preset;
        [SerializeField] protected GameObject m_construction_zone;
        [SerializeField] protected GameObject m_construction_progress;
        
        protected BuildingSaveData _buildingSaveData = new BuildingSaveData();
        protected GameManager GM;
        protected TimeManager TM;
        protected ResourcesManager RM;
        protected Dictionary<UnityAction<object>, Building_Setting_Data> _actions;
        protected List<UnityAction<object>> add_action = new List<UnityAction<object>>();
        protected List<Button_Action_Data> _buttonActionDatas = new List<Button_Action_Data>();
        protected Building_info_struct _information_Datas = new Building_info_struct();
        protected List<object> list_setting_values = new List<object>();
        protected List<Tuple<object, object, string>> list_information_values = new List<Tuple<object, object, string>>();
        protected List<Building_Information_Data> BI_datas = new List<Building_Information_Data>();
        protected GameObject construction_zone_pivot;
        protected bool is_addSettingother = true;
        protected bool is_cant_use_resource;
        
        private bool is_set_enable = false;
        private bool is_set_disable = false;
        
        public BuildingType _buildingType
        {
            get { return enum_buildingType; }
            set { enum_buildingType = value; }
        }

        public bool active
        {
            get => _buildingSaveData.Building_active;
            set => _buildingSaveData.Building_active = value;
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

        public bool is_Remove_Building
        {
            get => is_remove_building;
            set => is_remove_building = value;
        }

        public int people
        {
            get => _buildingSaveData.people;
            set => _buildingSaveData.people = value;
        }

        public int worker
        {
            get => _buildingSaveData.worker;
            set => _buildingSaveData.worker = value;
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
        

        public BuildingSaveData buildingSaveData
        {
            get => _buildingSaveData;
            set => _buildingSaveData = value;
        }

        public string name
        {
            get
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                return _buildingSaveData.nameObject;
            }
            set
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                _buildingSaveData.nameObject = value;
            }
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
            List<Button_Action_Data> menuDatas = new List<Button_Action_Data>();
            
            menuDatas.Add(new Button_Action_Data("Interact", Resources.Load<Sprite>("Icon/build_"), Interact));
            menuDatas.Add(new Button_Action_Data("Change Enable Building", Resources.Load<Sprite>("Icon/construction"), ChangeEnableBuilding));

            if (!is_remove_building)
            {
                menuDatas.Add(new Button_Action_Data("Remove", Resources.Load<Sprite>("Icon/account_tree"), () =>
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

        private void Start()
        {
            GM = GameManager.Instance;
            TM = TimeManager.Instance;
            RM = ResourcesManager.Instance; 
            
            Create_button_action_data_for_building();
        }

        protected virtual void Update()
        {
            //print("Is Active : " + _buildingSaveData.Building_active);
            
            OnProgress();

            buildingSaveData.efficiency = ((float)buildingSaveData.people + (float)buildingSaveData.worker) / ((float)m_Preset.max_people + (float)m_Preset.max_worker);

            ResourceUsageRate();

            Building_active();
        }

        private void Create_button_action_data_for_building()
        {
            //Building info
            BI_datas = new List<Building_Information_Data>();

            foreach (var BI in m_Preset.m_building_information)
            {
                BI_datas.Add(
                    new Building_Information_Data(BI.title, BI.text, Building_Information_Type.ShowInformation));
            }

            _information_Datas.informations = BI_datas;

            //Building setting
            add_action = new List<UnityAction<object>>();
            add_action.Add(SetActiveBuilding);



            //Building Status
            BI_datas = new List<Building_Information_Data>();
            BI_datas.Add(new Building_Information_Data("สิ่งก่อสร้างไม่พร้อมใช้งาน", "กำลังก่อสร้างเสร็จสิ้นแล้ว 0%",
                Building_Information_Type.ShowStatus));


            //Building_setting_danger
            _buttonActionDatas = new List<Button_Action_Data>();
            _buttonActionDatas.Add(new Button_Action_Data("remove", Resources.Load<Sprite>("Icon/remove_home"),
                () => { OnRemoveBuilding(); }));
            _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                () => { print("24HHHHHH"); }));


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
            print("Is NoooottttNuuuuulllllllllll : " + m_Preset.m_building_setting != null);
            int i = 0;
            foreach (var buildingSetting in m_Preset.m_building_setting)
            {
                _actions.Add(add_action[i], buildingSetting);
                i++;
            }
        }

        private void Building_active()
        {
            if (construction_zone_pivot != null && !construction_zone_pivot.activeSelf)
            {
                if (_buildingSaveData.Building_active)
                {
                    if (!is_set_enable)
                    {
                        is_set_disable = false;
                        is_set_enable = true;
                        OnEnableBuilding();
                    }
                }
                else
                {
                    if (!is_set_disable)
                    {
                        is_set_disable = true;
                        is_set_enable = false;
                        OnDisableBuilding();
                    }
                }
            }
            else
            {
                if (!is_set_disable && is_remove_building && !_buildingSaveData.Building_active)
                {
                    is_set_disable = true;
                    is_set_enable = false;
                    OnDisableBuilding();
                }
            }
        }

        public abstract void OnEnableBuilding();
        public abstract void OnDisableBuilding();

        protected virtual void ResourceUsageRate()
        {
            if (m_Preset.resources_use_rate > 0 && _buildingSaveData.Building_active)
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

        public virtual void Interact()
        {
            print("Innnnteerrraaacccttt : " + name);
        }

        public virtual void ChangeEnableBuilding()
        {
            print("OnChangeEnableBuilding : " + name);
        }

        public void SetActiveBuilding(object obj)
        {
            active = !active;
        }

        public void SetAirPurifierSpeedUp(object obj)
        {
            _buildingSaveData.Air_purifier_Speed_Up = !_buildingSaveData.Air_purifier_Speed_Up;
        }

        public void SetWorkOverTime(object obj)
        {
            _buildingSaveData.WorkOverTime = !_buildingSaveData.WorkOverTime;
        }
        
        public void RemoveAndAddPeople(object number)
        {
            print("Busssssssssssssssssssssssssssssssssss : " + (int)number);
            
            if((people + (int)number) <= people_Max && (people + (int)number) >= 0)
                people += (int)number;
        }
        
        public void RemoveAndAddWorker(object number)
        {
            print("Busssssssssssssssssssssssssssssssssss : " + (int)number);
            
            if((worker + (int)number) <= worker_Max && (worker + (int)number) >= 0)
                worker += (int)number;
        }

        public virtual void OnRemoveBuilding(bool is_Cancel_Remove = false)
        {
            bool can_remove_now = false;
            if (!is_remove_building)
            {
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

            //Over Time 24H
            _colorBlock.normalColor = new Color(0, 0, 0, 200);
            _colorBlock.highlightedColor = new Color(175, 175, 0, 240);
            _colorBlock.pressedColor = new Color(255, 255, 0, 175);
            _colorBlock.selectedColor = new Color(175, 175, 0, 240);
            _colorBlock.disabledColor = new Color(0, 0, 0, 0);
            _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                () => { print("24HHHHHH"); }, _colorBlock));

            return _buttonActionDatas[index];
        }

        public object GetValueBuilingSetting(int index)
        {
            list_setting_values = new List<object>();
            list_setting_values.Add(active);

            OnUpdateSettingValue();

            if (is_addSettingother)
            {
                list_setting_values.Add(_buildingSaveData.Air_purifier_Speed_Up);
                list_setting_values.Add(_buildingSaveData.WorkOverTime);
                list_setting_values.Add(new Tuple<float, float>(_buildingSaveData.people, m_Preset.max_people));
                list_setting_values.Add(new Tuple<float, float>(_buildingSaveData.worker, m_Preset.max_worker));
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
                    switch_mesh_construction_progress(false);

                if (is_remove_building)
                    OnRemoveBuilding();
            }
        }

        public Tuple<object, object, string> GetValueBuildingInformation(int index)
        {
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
            GM = FindObjectOfType<GameManager>();
            
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
        }

        public bool OnPlaceBuilding()
        {
            if (!RM.Set_Resources_Tree(-m_Preset.wood_build) || !RM.Set_Resources_Rock(-m_Preset.rock_build))
            {
                print("Cont Place ");
                return false;
            }
            else
            {
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
                GM.gameInstance.buildingSaveDatas.Add(_buildingSaveData);
                OnEndPlace();

                return true;
            }
        }

        private void switch_mesh_construction_progress(bool is_progress, bool is_set_building_active = true)
        {
            if (is_progress)
            {
                if (construction_zone_pivot == null)
                {
                    construction_zone_pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    construction_zone_pivot.layer = LayerMask.NameToLayer("Place_Object");
                    construction_zone_pivot.transform.parent = transform;
                    construction_zone_pivot.transform.localPosition = new Vector3(0, -0.5f, 0);
                    Destroy(construction_zone_pivot.GetComponent<BoxCollider>());
                    Outliner _outliner = construction_zone_pivot.AddComponent<Outliner>();
                    _outliner.OutlineWidth = 1.05f;
                    _outliner.enabled = false;

                    construction_zone_pivot.GetComponent<MeshFilter>().sharedMesh = m_construction_zone.GetComponent<MeshFilter>().sharedMesh;
                }

                if(is_set_building_active)
                    buildingSaveData.Building_active = false;
                
                GetComponent<MeshRenderer>().enabled = false;
                construction_zone_pivot.SetActive(true);
            }
            else
            {
                if(is_set_building_active && construction_progess_percent > 0.5)
                    buildingSaveData.Building_active = true;
                
                GetComponent<MeshRenderer>().enabled = true;
                construction_zone_pivot.SetActive(false);
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
        }

        public abstract void OnDestroyBuilding();
    }
}
