using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class Tech_Script : Building_System_Script
    {
        [SerializeField] private Tech_Preset m_techPreset;
        private Tech_SaveData _techSaveData = new Tech_SaveData();
        
        public override void Resource_usage()
        {
            RM.Set_Resources_Token(Mathf.RoundToInt(m_techPreset.token * efficiency));
        }
        
        protected override bool Check_Resource()
        {
            return true;
        }

        public override void BeginStart()
        {
            _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                () => { SetWork24h(); print("24HHHHHH"); }));
            
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

        public override void AddInteractAction()
        {
            print("HFOIHHFOHOIHFOSDHOHFOS");
            
            //Work Overtime 8/12 hour
            if(_buildingSaveData.is_work_overtime)
                menuDatas.Add(new Button_Action_Data("Work 8 hour", Resources.Load<Sprite>("Icon/clock"), () => { SetWorkOverTime(0);}));
            else
                menuDatas.Add(new Button_Action_Data("Work 12 Hour", Resources.Load<Sprite>("Icon/overtime"), () => { SetWorkOverTime(0);}));
            
            //Work Overtime 24 hour
            if(_buildingSaveData.is_work_24h)
                menuDatas.Add(new Button_Action_Data("Stop work 24 hour", Resources.Load<Sprite>("Icon/clock"), () => { SetWork24h();}));
            else
                menuDatas.Add(new Button_Action_Data("Work 24 Hour", Resources.Load<Sprite>("Icon/24H"), () => { SetWork24h();}));

            
            print("COUNTTTTT : " + menuDatas.Count);
        }
        
        public override void AddUpdateButtonAction()
        {
            //Over Time 24H
            ColorBlock _colorBlock = new ColorBlock();
            _colorBlock.highlightedColor = new Color(150, 0, 0, 240);
            _colorBlock.pressedColor = new Color(100, 0, 0, 200);
            _colorBlock.selectedColor = new Color(150, 0, 0, 240);
            _colorBlock.disabledColor = new Color(0, 0, 0, 0);
            _colorBlock.colorMultiplier = 1;
            _colorBlock.fadeDuration = 0.1f;
            if (_buildingSaveData.is_work_24h)
            {
                _colorBlock.normalColor = new Color(100, 0, 0, 200);
                _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                    () => {SetWork24h(); /*print("24HHHHHH");*/ }, _colorBlock));
            }
            else
            {
                _colorBlock.normalColor = new Color(0, 0, 0, 200);
                _buttonActionDatas.Add(new Button_Action_Data("null", Resources.Load<Sprite>("Icon/24H"),
                    () => {SetWork24h(); /*print("24HHHHHH");*/ }, _colorBlock));
            }
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