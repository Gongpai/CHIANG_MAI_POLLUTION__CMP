using System;
using System.Collections.Generic;
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
        protected Dictionary<UnityAction<object>, Static_Resource_Setting_Data> _actions = new ();
        protected List<UnityAction<object>> add_action = new List<UnityAction<object>>();
        protected List<Button_Action_Data> _buttonActionDatas = new List<Button_Action_Data>();
        protected Static_Resource_info_struct _information_Datas = new ();
        protected List<object> list_setting_values = new List<object>();
        protected List<Tuple<object, object, string>> list_information_values = new List<Tuple<object, object, string>>();
        protected List<Static_Resource_Information_Data> BI_datas = new();

        public int people
        {
            get => _staticResourceSaveData.people;
            set => _staticResourceSaveData.people = value;
        }

        public int worker
        {
            get => _staticResourceSaveData.worker;
            set => _staticResourceSaveData.worker = value;
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

        public virtual List<Button_Action_Data> GetInteractAction()
        {
            List<Button_Action_Data> menuDatas = new List<Button_Action_Data>();
            
            menuDatas.Add(new Button_Action_Data("Interact", Resources.Load<Sprite>("Icon/build_"), Interact));
            menuDatas.Add(new Button_Action_Data("Change Enable Building", Resources.Load<Sprite>("Icon/construction"), ChangeEnableBuilding));

            return menuDatas;
        }

        private void Awake()
        {
            GM = GameManager.Instance;
            TM = TimeManager.Instance;
            RM = ResourcesManager.Instance;

            if (!GM.gameInstance.check_id_ResourceSaveData(m_id_Resource))
            {
                print("Non Save");
                _staticResourceSaveData.id = m_id_Resource;
                GM.gameInstance.staticResourceSaveDatas.Add(_staticResourceSaveData);
            }
            else
            {
                print("Load Save");
                _staticResourceSaveData = GM.gameInstance.Get_Static_Resource_SaveData(m_id_Resource);
            }
        }

        private void Start()
        {
            Create_button_action_data_for_building();
        }

        protected virtual void Update()
        {
            _staticResourceSaveData.efficiency = ((float)_staticResourceSaveData.people + (float)_staticResourceSaveData.worker) / ((float)m_Resource_Preset.max_people + (float)m_Resource_Preset.max_worker);

            ResourceProductRate();
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
                () => { print("24HHHHHH"); }));

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
            _staticResourceSaveData.WorkOverTime = !_staticResourceSaveData.WorkOverTime;
        }
        
        public void RemoveAndAddPeople(object number)
        {
            if((people + (int)number) <= people_Max && (people + (int)number) >= 0)
                people += (int)number;
        }
        
        public void RemoveAndAddWorker(object number)
        {
            if((worker + (int)number) <= worker_Max && (worker + (int)number) >= 0)
                worker += (int)number;
        }

        public Button_Action_Data GetUpdateButtonAction(int index)
        {
            _buttonActionDatas = new List<Button_Action_Data>();

            //Over Time 24H
            ColorBlock _colorBlock = new ColorBlock();
            _colorBlock.normalColor = new Color(0, 0, 0, 200);
            _colorBlock.highlightedColor = new Color(175, 175, 0, 240);
            _colorBlock.pressedColor = new Color(255, 255, 0, 175);
            _colorBlock.selectedColor = new Color(175, 175, 0, 240);
            _colorBlock.disabledColor = new Color(0, 0, 0, 0);
            _colorBlock.colorMultiplier = 1;
            _colorBlock.fadeDuration = 0.1f;
            _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                () => { print("24HHHHHH"); }, _colorBlock));

            return _buttonActionDatas[index];
        }

        public object GetValueBuilingSetting(int index)
        {
            list_setting_values = new List<object>();
            list_setting_values.Add(new Tuple<float, float>(_staticResourceSaveData.people, m_Resource_Preset.max_people));
            list_setting_values.Add(new Tuple<float, float>(_staticResourceSaveData.worker, m_Resource_Preset.max_worker));
            list_setting_values.Add(_staticResourceSaveData.WorkOverTime);
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
            GM = FindObjectOfType<GameManager>();

            TM = TimeManager.Instance;
            if(_staticResourceSaveData.re_userate_hour <= 0)
                _staticResourceSaveData.re_userate_hour = TM.To_TotalHour(TM.get_DateTime) + m_Resource_Preset.product_output_use_rate;

            OnBeginPlace();
            OnBeginPlace();
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

        public abstract void OnDestroyBuilding();
    }
}