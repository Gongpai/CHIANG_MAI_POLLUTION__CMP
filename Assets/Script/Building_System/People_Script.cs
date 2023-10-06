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
        private People_SaveData _peoplScriptSaveData = new People_SaveData();

        public int get_people_count
        {
            get => _peoplScriptSaveData.peoples.Count;
        }
        
        public int get_people_max
        {
            get => m_peoplePreset.people;
        }
        
        public override void Resource_usage()
        {
            
        }

        protected override bool Check_Resource()
        {
            return false;
        }
        
        public override void BeginStart()
        {
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[1].title, m_Preset.m_building_status[1].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[2].title, m_Preset.m_building_status[2].text + get_people_count + "/" + m_peoplePreset.people + " คน", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
        }

        public override void EndStart()
        {
            
        }

        public void OnAddPeople(PeopleSystemSaveData _peopleSaveData)
        {
            if (get_people_count < m_peoplePreset.people)
            {
                _peoplScriptSaveData.peoples.Add(_peopleSaveData);
            }
        }

        public void OnRemovePeople(PeopleSystemSaveData _peopleSaveData)
        {
            if (get_people_count > 0)
            {
                _peoplScriptSaveData.peoples.Remove(_peopleSaveData);
            }
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

        protected override bool OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && !is_cant_use_power, null, m_Preset.m_building_status[1].text));
            list_information_values.Add(new Tuple<object, object, string>((float)get_people_count, (float)m_peoplePreset.people, m_Preset.m_building_status[2].text + " " + get_people_count + "/" + m_peoplePreset.people + " คน"));

            return active && !is_cant_use_power;
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<People_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _peoplScriptSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _peoplScriptSaveData;
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