using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class People_Script : Building_System_Script
    {
        [SerializeField] private People_Preset m_peoplePreset;
        private People_SaveData _peopleSaveData = new People_SaveData();

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
        }

        public override void EndStart()
        {
            
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

        protected override void OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && !is_cant_use_power, null, m_Preset.m_building_status[0].text));
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<People_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _peopleSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _peopleSaveData;
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