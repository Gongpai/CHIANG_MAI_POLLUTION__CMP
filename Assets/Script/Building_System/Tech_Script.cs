using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

namespace GDD
{
    public class Tech_Script : Building_System_Script
    {
        private Tech_SaveData _techSaveData = new Tech_SaveData();

        public override void Resource_usage()
        {
            
        }

        public override void BeginStart()
        {
            add_action.Add(RemoveAndAddPeople);
            is_addSettingother = false;
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
            list_setting_values.Add(new Tuple<float, float>(_buildingSaveData.people, m_Preset.max_people));
        }

        protected override void OnUpdateInformationValue()
        {
            
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