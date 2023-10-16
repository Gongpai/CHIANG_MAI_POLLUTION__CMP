using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
                    if (_generatorSaveData.IsOverdrive)
                        if(_buildingSaveData.Air_purifier_Speed_Up)
                            return ((m_generatorPreset.power * efficiency) * 2) - 1;
                        else
                            return (m_generatorPreset.power * efficiency) * 2;
                    else
                        if(_buildingSaveData.Air_purifier_Speed_Up)
                            return (m_generatorPreset.power * efficiency) - 1;
                        else
                            return m_generatorPreset.power * efficiency;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float max_power_produce
        {
            get
            {
                if (_generatorSaveData.IsOverdrive)
                    return m_generatorPreset.power * 2;
                else
                    return m_generatorPreset.power;
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
                if(_generatorSaveData.IsOverdrive)
                    RM.Set_Resources_Tree(-Mathf.CeilToInt((m_generatorPreset.wood_use * efficiency)) * 2);
                else
                    RM.Set_Resources_Tree(-Mathf.CeilToInt(m_generatorPreset.wood_use * efficiency));
            }
        }

        protected override bool Check_Resource()
        {
            //print("Is Enable : " + disable + " Can Tree : " + RM.Can_Set_Resources_Tree(-Mathf.CeilToInt(m_generatorPreset.wood_use * _buildingSaveData.efficiency)) + " TREE CURRENT : " + GM.gameInstance.get_tree_resource());
            return RM.Can_Set_Resources_Tree(-Mathf.CeilToInt(m_generatorPreset.wood_use * efficiency));
        }

        public void SetEnableOverDrive(object obj)
        {
            _generatorSaveData.IsOverdrive = !_generatorSaveData.IsOverdrive;
        }
        
        public override bool Get_WrokOverTime()
        {
            return false;
        }

        public override bool Get_Wrok24H()
        {
            return true;
        }

        public override void OnEnableBuilding()
        {
            
        }

        public override void OnDisableBuilding()
        {
            
        }

        public override void BeginStart()
        {
            is_addSettingother = false;
            add_action.Add(SetEnableOverDrive);
            add_action.Add(SetAirPurifierSpeedUp);
            add_action.Add(RemoveAndAddPeople);
            add_action.Add(RemoveAndAddWorker);
            
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[1].title, m_Preset.m_building_status[1].text, Building_Information_Type.ShowStatus, Building_Show_mode.TextOnly));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[2].title, m_Preset.m_building_status[2].text + " " + power_produce + "/" + max_power_produce + " kw", Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
            BI_datas.Add(new Building_Information_Data(m_Preset.m_building_status[3].title, m_Preset.m_building_status[3].text + " " + efficiency, Building_Information_Type.ShowStatus, Building_Show_mode.TextWith_ProgressBar));
        }

        public override void EndStart()
        {
            RM.generatorScripts.Add(this);
        }

        public override void AddInteractAction()
        {
            print("HFOIHHFOHOIHFOSDHOHFOS");
            
            //Air Purifier Speed
            if(_generatorSaveData.IsOverdrive)
                menuDatas.Add(new Button_Action_Data("OverDrive Off", Resources.Load<Sprite>("Icon/speed_Off"), () => { SetEnableOverDrive(0);}));
            else
                menuDatas.Add(new Button_Action_Data("OverDrive On", Resources.Load<Sprite>("Icon/speed_On"), () => { SetEnableOverDrive(0);}));
            
            print("COUNTTTTT : " + menuDatas.Count);
        }

        protected override void OnUpdateSettingValue()
        {
            list_setting_values.Add(_generatorSaveData.IsOverdrive);
            list_setting_values.Add(_buildingSaveData.Air_purifier_Speed_Up);
            list_setting_values.Add(new Tuple<float, float>(villager_count, m_Preset.max_people));
            list_setting_values.Add(new Tuple<float, float>(worker_count, m_Preset.max_worker));
        }

        protected override bool OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(active && is_cant_use_resource, null, m_Preset.m_building_status[1].text));
            list_information_values.Add(new Tuple<object, object, string>(power_produce, max_power_produce, m_Preset.m_building_status[2].text + " " + Mathf.FloorToInt(power_produce) + "/" + max_power_produce + " kw"));
            list_information_values.Add(new Tuple<object, object, string>(efficiency, 1.0f, m_Preset.m_building_status[3].text+ " " + (int)(efficiency * 100) + "%"));

            return (active && is_cant_use_resource);
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
        
        public override void AddUpdateButtonAction()
        {
            
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