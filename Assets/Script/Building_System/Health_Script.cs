using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class Health_Script : Building_System_Script
    {
        [SerializeField] private Health_Preset m_healthPreset;
        private Health_SaveData _healthSaveData = new Health_SaveData();

        [SerializeField] private List<PeopleSystemSaveData> ppp = new List<PeopleSystemSaveData>();

        public int get_patient_count
        {
            get => _healthSaveData.patients.Count;
        }

        public int get_nurse_patient_count
        {
            get => _healthSaveData.nurse_patients.Count;
        }

        public int get_patient_max
        {
            get => m_healthPreset.patient;
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
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[2].title, m_Preset.m_building_status[2].text + get_patient_count + "/" + get_patient_max + " คน", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[3].title, m_Preset.m_building_status[3].text + get_nurse_patient_count + "/" + (m_Preset.max_people + m_Preset.max_worker) + " คน", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[4].title, m_Preset.m_building_status[4].text + " " + efficiency, Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
        }

        public override void EndStart()
        {
            
        }

        protected override void Update()
        {
            base.Update();
            ppp = _healthSaveData.patients;
        }

        public void OnAdmit(PeopleSystemSaveData _peopleSaveData, bool is_nurse = false)
        {
            if (is_nurse)
            {
                if (get_nurse_patient_count < m_Preset.max_people + m_Preset.max_worker)
                {
                    _healthSaveData.nurse_patients.Add(_peopleSaveData);
                }
            }
            else
            {
                if (get_patient_count < get_patient_max)
                {
                    _healthSaveData.patients.Add(_peopleSaveData);
                }
            }
        }

        public void OnRecoverIllness(PeopleSystemSaveData _peopleSaveData, bool is_nurse = false)
        {
            if (is_nurse)
            {
                if (get_nurse_patient_count > 0)
                    _healthSaveData.nurse_patients.Remove(_peopleSaveData);
            }
            else
            {
                if (get_patient_count > 0)
                {
                    _healthSaveData.patients.Remove(_peopleSaveData);
                }
            }
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
            
        }

        protected override bool OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && !is_cant_use_power, null, m_Preset.m_building_status[1].text));
            list_information_values.Add(new Tuple<object, object, string>((float)get_patient_count, (float)get_patient_max, m_Preset.m_building_status[2].text + " " + get_patient_count + "/" + get_patient_max + " คน"));
            list_information_values.Add(new Tuple<object, object, string>((float)get_nurse_patient_count, (float)(m_Preset.max_people + m_Preset.max_worker), m_Preset.m_building_status[3].text + " " + get_nurse_patient_count + "/" + (m_Preset.max_people + m_Preset.max_worker) + " คน"));
            list_information_values.Add(new Tuple<object, object, string>(efficiency, 1.0f, m_Preset.m_building_status[4].text+ " " + (int)(efficiency * 100) + "%"));
            return active && !is_cant_use_power;
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<Health_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _healthSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _healthSaveData;
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