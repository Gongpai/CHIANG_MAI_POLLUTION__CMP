using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class Cookhouse_Script : Building_System_Script
    {
        [SerializeField] private Cookhouse_Preset m_cookhousePreset;
        private Cookhouse_SaveData _cookhouseSaveData = new Cookhouse_SaveData();
        
        protected override void ResourceUsageRate()
        {
            base.ResourceUsageRate();
            
            if (Check_Resource() && is_cant_use_resource)
            {
                disable = false;
                is_cant_use_resource = false;
            }
        }
        
        public override void Resource_usage()
        {
            if (!Check_Resource() && building_is_active)
            {
                disable = true;
                is_cant_use_resource = true;
            }
            else
            {
                RM.Set_Resources_Food(Mathf.RoundToInt(m_cookhousePreset.food * efficiency));
                RM.Set_Resources_Tree(-Mathf.CeilToInt(m_cookhousePreset.wood_use * efficiency));
                RM.Set_Resources_Raw_Food(-Mathf.CeilToInt(m_cookhousePreset.raw_food_use * efficiency));
            }
        }
        
        protected override bool Check_Resource()
        {
            bool check = RM.Can_Set_Resources_Tree(-Mathf.CeilToInt(m_cookhousePreset.wood_use * efficiency)) && RM.Can_Set_Resources_Raw_Food(-Mathf.CeilToInt(m_cookhousePreset.raw_food_use * efficiency));
            return check;
        }

        public override void BeginStart()
        {
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[1].title, m_Preset.m_building_status[1].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[2].title, m_Preset.m_building_status[2].text + " " + efficiency, Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
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
            
        }

        protected override bool OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && !is_cant_use_power, null, m_Preset.m_building_status[1].text));
            list_information_values.Add(new Tuple<object, object, string>(efficiency, 1.0f, m_Preset.m_building_status[2].text+ " " + (int)(efficiency * 100) + "%"));

            return active && !is_cant_use_power;
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<Cookhouse_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _cookhouseSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _cookhouseSaveData;
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