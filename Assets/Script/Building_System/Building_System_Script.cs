using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public abstract class Building_System_Script : MonoBehaviour, IConstruction_System, IMenuInteractable
    {
        [SerializeField] protected BuildingType enum_buildingType;
        [SerializeField] protected Building_Preset m_Preset;
        protected BuildingSaveData _buildingSaveData = new BuildingSaveData();
        protected GameManager GM;
        protected Dictionary<UnityAction<object>, Building_Setting_Data> _actions;
        protected List<UnityAction<object>> add_action;

        protected Building_info_struct _information_Datas = new Building_info_struct();
        protected List<object> list_setting_value = new List<object>();
        protected List<Tuple<object, object, string>> list_information_value = new List<Tuple<object, object, string>>();
        protected bool is_addSettingother = true;
        
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

        public Dictionary<UnityAction<object>, Building_Setting_Data> actionsBuilding
        {
            get => _actions;
        }

        public virtual List<Quick_Menu_Data> GetInteractAction()
        {
            List<Quick_Menu_Data> menuDatas = new List<Quick_Menu_Data>();
            menuDatas.Add(new Quick_Menu_Data("Interact", Resources.Load<Sprite>("Icon/build_"), Interact));
            menuDatas.Add(new Quick_Menu_Data("Change Enable Building", Resources.Load<Sprite>("Icon/construction"), ChangeEnableBuilding));
            menuDatas.Add(new Quick_Menu_Data("Remove", Resources.Load<Sprite>("Icon/account_tree"), RemoveBuilding));

            return menuDatas;
        }

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

        public virtual void RemoveBuilding()
        {
            print("Reomove : " + name);
            Destroy(gameObject);
        }

        private void Start()
        {
            GM = GameManager.Instance;
            
            //Building info and status
            List<Building_Information_Data> BI_datas = new List<Building_Information_Data>();
            foreach (var BI in m_Preset.m_building_information)
            {
                BI_datas.Add(new Building_Information_Data(BI.title, BI.text, Building_Information_Type.ShowInformation));
            }
            _information_Datas.informations = BI_datas;
               
            //Building setting
            add_action = new List<UnityAction<object>>();
            add_action.Add(SetActiveBuilding);
            
            BeginStart();

            if (is_addSettingother)
            {
                add_action.Add(SetAirPurifierSpeedUp);
                add_action.Add(SetWorkOverTime);
                add_action.Add(RemoveAndAddPeople);
                add_action.Add(RemoveAndAddWorker);
            }

            EndStart();

            _actions = new();
            print( "Is NoooottttNuuuuulllllllllll : " + m_Preset.m_building_setting != null);
            int i = 0;
            foreach (var buildingSetting in m_Preset.m_building_setting)
            {
                _actions.Add(add_action[i], buildingSetting);
                i++;
            }
             
        }

        public object GetValueBuilingSetting(int index)
        {
            list_setting_value = new List<object>();
            list_setting_value.Add(active);
            OnUpdateSettingValue();

            if (is_addSettingother)
            {
                list_setting_value.Add(_buildingSaveData.Air_purifier_Speed_Up);
                list_setting_value.Add(_buildingSaveData.WorkOverTime);
                list_setting_value.Add(new Tuple<float, float>(_buildingSaveData.people, m_Preset.max_people));
                list_setting_value.Add(new Tuple<float, float>(_buildingSaveData.worker, m_Preset.max_worker));
            }

            return list_setting_value[index];
        }

        public Tuple<object, object, string> GetValueBuildingInformation(int index)
        {
            OnUpdateInformationValue();
            print("List Info Value : " + list_information_value[index]);
            return list_information_value[index];
        }

        protected abstract void OnUpdateSettingValue();
        protected abstract void OnUpdateInformationValue();
        
        public abstract void BeginStart();
        public abstract void EndStart();

        public void OnGameLoad()
        {
            GM = FindObjectOfType<GameManager>();
            OnBeginPlace();
            transform.position = new Vector3(_buildingSaveData.Position.X, _buildingSaveData.Position.Y, _buildingSaveData.Position.Z);
            transform.eulerAngles = new Vector3(_buildingSaveData.Rotation.X, _buildingSaveData.Rotation.Y,_buildingSaveData.Rotation.Z);
            OnBeginPlace();
        }

        public void OnPlaceBuilding()
        {
            print("OnPlaceeeee ");
            OnBeginPlace();
            _buildingSaveData.Position = new Vector3D(transform.position.x, transform.position.y, transform.position.z);
            _buildingSaveData.Rotation = new Vector3D(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            _buildingSaveData.b_buildingtype = (byte)enum_buildingType;
            GM.gameInstance.buildingSystemScript.Add(_buildingSaveData);
            OnEndPlace();
        }

        public abstract void OnBeginPlace();
        public abstract void OnEndPlace();

        public void OnRemoveBuilding()
        {
            OnBeginRemove();
            
            //System script
            //-----------------------------------
            
            //-----------------------------------
            
            OnEndRemove();
        }

        public abstract void OnBeginRemove();
        public abstract void OnEndRemove();

        private void OnDestroy()
        {
            if(GM != null && _buildingSaveData != null)
                GM.gameInstance.buildingSystemScript.Remove(_buildingSaveData);
        }
    }
}
