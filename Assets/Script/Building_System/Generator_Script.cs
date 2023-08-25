using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class Generator_Script : Building_System_Script
    {
        [SerializeField] private Generator_Preset m_generatorPreset; 
        private Generator_SaveData _generatorSaveData = new Generator_SaveData();

        public void SetEnableOverDrive(object obj)
        {
            _generatorSaveData.IsOverdrive = !_generatorSaveData.IsOverdrive;
        }
        
        public override void BeginStart()
        {
            add_action.Add(SetEnableOverDrive);
            
            List<Building_Information_Data> BI_datas = new List<Building_Information_Data>();
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[0].title, m_Preset.m_building_status[0].text, Building_Information_Type.ShowStatus));
            _information_Datas.status = BI_datas;
        }

        public override void EndStart()
        {
            
        }

        protected override void OnUpdateSettingValue()
        {
            list_setting_value.Add(_generatorSaveData.IsOverdrive);
        }

        protected override void OnUpdateInformationValue()
        {
            list_information_value.Add(new Tuple<object, object, string>(_generatorSaveData.current_power, m_generatorPreset.power, _generatorSaveData.current_power + "/" + m_generatorPreset.power + " kw"));
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<Generator_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _generatorSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _generatorSaveData;
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
    }
}