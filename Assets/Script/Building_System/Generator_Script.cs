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

        public float power_produce
        {
            get
            {
                if (building_is_active)
                {
                    return m_generatorPreset.power * buildingSaveData.efficiency;
                }
                else
                {
                    return 0;
                }
            }
        }

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
                RM.Set_Resources_Tree(-Mathf.CeilToInt(m_generatorPreset.wood_use * _buildingSaveData.efficiency));
            }
        }

        protected override bool Check_Resource()
        {
            //print("Is Enable : " + disable + " Can Tree : " + RM.Can_Set_Resources_Tree(-Mathf.CeilToInt(m_generatorPreset.wood_use * _buildingSaveData.efficiency)) + " TREE CURRENT : " + GM.gameInstance.get_tree_resource());
            return RM.Can_Set_Resources_Tree(-Mathf.CeilToInt(m_generatorPreset.wood_use * _buildingSaveData.efficiency));
        }

        public void SetEnableOverDrive(object obj)
        {
            _generatorSaveData.IsOverdrive = !_generatorSaveData.IsOverdrive;
        }

        public override void OnEnableBuilding()
        {
            
        }

        public override void OnDisableBuilding()
        {
            
        }

        public override void BeginStart()
        {
            add_action.Add(SetEnableOverDrive);
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[0].title, m_Preset.m_building_status[0].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[1].title, m_Preset.m_building_status[1].text + " " + power_produce + "/" + m_generatorPreset.power + " kw", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[2].title, m_Preset.m_building_status[2].text + " " + _buildingSaveData.efficiency, Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
        }

        public override void EndStart()
        {
            RM.generatorScripts.Add(this);
        }

        protected override void OnUpdateSettingValue()
        {
            list_setting_values.Add(_generatorSaveData.IsOverdrive);
        }

        protected override void OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && is_cant_use_resource, null, m_Preset.m_building_status[0].text));
            list_information_values.Add(new Tuple<object, object, string>(power_produce, m_generatorPreset.power, m_Preset.m_building_status[1].text + " " + power_produce + "/" + m_generatorPreset.power + " kw"));
            list_information_values.Add(new Tuple<object, object, string>(buildingSaveData.efficiency, 1.0f, m_Preset.m_building_status[2].text+ " " + (_buildingSaveData.efficiency * 100) + "%"));
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

        public override void OnDestroyBuilding()
        {
            if(RM != null)
                RM.generatorScripts.Remove(this);
        }
    }
}