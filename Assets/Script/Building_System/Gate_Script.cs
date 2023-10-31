using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GDD
{
    public class Gate_Script : Static_Object_Resource_System_Script
    {
        [SerializeField] private Gate_Preset m_gatePreset;
        [SerializeField] private GameObject InputUI;

        private int add_survivor_villager_count;
        private int add_survivor_worker_count;
        private Gate_SaveData _gateSaveData = new Gate_SaveData();
        
        public override void Resource_product()
        {
            RM.Set_Resources_Raw_Food(Mathf.CeilToInt(m_Resource_Preset.product_output_resource * efficiency));
        }

        public override void BeginStart()
        {
            BI_datas.Add(new (m_Resource_Preset.m_static_resource_status[0].title, m_Resource_Preset.m_static_resource_status[0].text + " " + efficiency, Building_Information_Type.ShowStatus));
            BI_datas.Add(new (m_Resource_Preset.m_static_resource_status[1].title, m_Resource_Preset.m_static_resource_status[1].text + _gateSaveData.survivors_villager + "/" + m_gatePreset.max_survivor + " คน", Building_Information_Type.ShowStatus));
            BI_datas.Add(new (m_Resource_Preset.m_static_resource_status[2].title, m_Resource_Preset.m_static_resource_status[2].text + _gateSaveData.survivors_worker + "/" + m_gatePreset.max_survivor + " คน", Building_Information_Type.ShowStatus));
            add_action.Add(arg0 =>
            {
                if(_gateSaveData.survivors_villager > 0)
                    CreateUI_AddVillager(arg0);
            });
            add_action.Add(arg0 =>
            {
                if(_gateSaveData.survivors_worker > 0)
                    CreateUI_AddWorker(arg0);
            });
            
            if(_staticResourceSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_staticResourceSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<Gate_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _gateSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _staticResourceSaveData.saveDataObject = _gateSaveData;
        }

        public override void EndStart()
        {
            add_action.Add(arg => { GM.OnReourceLow(); });
        }

        protected override void Update()
        {
            base.Update();
            
            RandomPeoplePerTenDay();
        }

        private void RandomPeoplePerTenDay()
        {
            if (TM.To_Totalday(TM.get_DateTime) > 1 && (int)TM.To_Totalday(TM.get_DateTime) % 10 == 0 && TM.getGameTimeHour == 7 && (int)TM.To_Totalday(TM.get_DateTime) <= 90 && !_gateSaveData.is_set_time)
            {
                float max_sur = m_gatePreset.max_survivor;
                float villager_random = Random.Range(0, max_sur);
                max_sur -= villager_random;
                float worker_ramdom = Random.Range(0, max_sur);

                _gateSaveData.survivors_villager = (int)villager_random;
                _gateSaveData.survivors_worker = (int)worker_ramdom;

                _gateSaveData.is_set_time = true;
            } else if (TM.getGameTimeHour > 8)
            {
                _gateSaveData.is_set_time = false;
            }
        }

        public void CreateUI_AddVillager(object _object)
        {
            Ui_Utilities _uiUtilities;
            
            if (GetComponent<Ui_Utilities>() == null)
            {
                _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            }
            else
            {
                _uiUtilities = GetComponent<Ui_Utilities>();
            }

            _uiUtilities.canvasUI = InputUI;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.order_in_layer = 12;
            GameObject m_input_button = _uiUtilities.CreateInputSliderUI(arg0 =>
                {
                    AddVillagerSurvivor(arg0);
                }, 
                () =>
                {
                    AcceptVillages();
                }, (() => 
                {
                    add_survivor_villager_count = 0;
                }),0, _gateSaveData.survivors_villager, "เพิ่มชาวบ้านเข้าไปยังเมือง", "จำนวนชาวบ้าน : ", "Add People", "Cancel");
            
            m_input_button.GetComponent<Animator>().SetBool("IsStart", true);
        }
        
        public void CreateUI_AddWorker(object _object)
        {
            Ui_Utilities _uiUtilities;
            
            if (GetComponent<Ui_Utilities>() == null)
            {
                _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            }
            else
            {
                _uiUtilities = GetComponent<Ui_Utilities>();
            }

            _uiUtilities.canvasUI = InputUI;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.order_in_layer = 12;
            GameObject m_input_button = _uiUtilities.CreateInputSliderUI(arg0 =>
                {
                    AddWorkerSurvivor(arg0);
                }, 
                () =>
                {
                    AcceptWorkers();
                }, (() => 
                {
                    add_survivor_worker_count = 0;
                }),0, _gateSaveData.survivors_worker, "เพิ่มคนงานเข้าไปยังเมือง", "จำนวนคนงาน : ", "Add People", "Cancel");
            
            m_input_button.GetComponent<Animator>().SetBool("IsStart", true);
        }

        public void AddVillagerSurvivor(float number)
        {
            add_survivor_villager_count = Mathf.FloorToInt(number);
        }
        
        public void AddWorkerSurvivor(float number)
        {
            add_survivor_worker_count = Mathf.FloorToInt(number);
        }

        public void AcceptVillages()
        {
            HRM.OnSpawnPeople(add_survivor_villager_count, 0);
            _gateSaveData.survivors_villager -= add_survivor_villager_count;
            add_survivor_villager_count = 0;
        }
        
        public void AcceptWorkers()
        {
            HRM.OnSpawnPeople(0, add_survivor_worker_count);
            _gateSaveData.survivors_worker -= add_survivor_worker_count;
            add_survivor_worker_count = 0;
        }
        
        public override void OnEnableBuilding()
        {
            
        }

        public override void OnDisableBuilding()
        {
            
        }

        protected override void OnUpdateSettingValue()
        {
            List<object> list_gate_setting_values = new List<object>();
            list_gate_setting_values.Add((_gateSaveData.survivors_villager > 0));
            list_gate_setting_values.Add((_gateSaveData.survivors_worker > 0));
            list_gate_setting_values.Add(list_setting_values[0]);
            list_gate_setting_values.Add(list_setting_values[1]);
            list_gate_setting_values.Add(list_setting_values[2]);
            list_gate_setting_values.Add(true);
            list_setting_values = list_gate_setting_values;
        }

        protected override void OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(efficiency, 1.0f, m_Resource_Preset.m_static_resource_status[0].text + " " + (efficiency * 100) + "%"));
            list_information_values.Add(new Tuple<object, object, string>((float)_gateSaveData.survivors_villager, (float)m_gatePreset.max_survivor, m_Resource_Preset.m_static_resource_status[1].text + _gateSaveData.survivors_villager + "/" + m_gatePreset.max_survivor + " คน"));
            list_information_values.Add(new Tuple<object, object, string>((float)_gateSaveData.survivors_worker, (float)m_gatePreset.max_survivor, m_Resource_Preset.m_static_resource_status[2].text + _gateSaveData.survivors_worker + "/" + m_gatePreset.max_survivor + " คน"));
        }

        public override void OnBeginPlace()
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
            
        }
    }
}