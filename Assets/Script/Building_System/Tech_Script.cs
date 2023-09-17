using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class Tech_Script : Building_System_Script
    {
        [SerializeField] private Tech_Preset m_techPreset;
        private Tech_SaveData _techSaveData = new Tech_SaveData();

        public override void Resource_usage()
        {
            
        }
        
        protected override bool Check_Resource()
        {
            return false;
        }

        public override void BeginStart()
        {
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[0].title, m_Preset.m_building_status[0].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            add_action.Add(RemoveAndAddPeople);
            is_addSettingother = false;
        }

        public override void EndStart()
        {
            
        }
        
        public override void OnEnableBuilding()
        {
            print("ONNNNN");
        }

        public override void OnDisableBuilding()
        {
            
        }

        protected override void OnUpdateSettingValue()
        {
            list_setting_values.Add(new Tuple<float, float>(_buildingSaveData.people, m_Preset.max_people));
        }

        protected override void OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && !is_cant_use_power, null, m_Preset.m_building_status[0].text));
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<Tech_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _techSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _techSaveData;
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