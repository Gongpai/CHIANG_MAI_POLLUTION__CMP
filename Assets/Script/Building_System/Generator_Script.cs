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
        private Generator_SaveData _generatorSaveData = new Generator_SaveData();

        public void SetEnableOverDrive(object obj)
        {
            _generatorSaveData.IsOverdrive = !_generatorSaveData.IsOverdrive;
        }
        
        public override void BeginStart()
        {
            add_action.Add(SetEnableOverDrive);
        }

        public override void EndStart()
        {
            
        }

        protected override void OnUpdateValue()
        {
            list_setting_value.Add(_generatorSaveData.IsOverdrive);
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