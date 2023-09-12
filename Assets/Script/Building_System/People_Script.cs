using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class People_Script : Building_System_Script
    {
        [SerializeField] private People_Preset m_peoplePreset;
        private People_SaveData _peopleSaveData = new People_SaveData();

        public override void Resource_usage()
        {
            
        }

        public override void BeginStart()
        {
           
        }

        public override void EndStart()
        {
            
        }
        
        public override void OnEnableBuilding()
        {
            print("ONNNNN");
            RM.Set_Resources_Power_Use(m_peoplePreset.power_use);
        }

        public override void OnDisableBuilding()
        {
            print("OFFFFFF");
            RM.Set_Resources_Power_Use(-m_peoplePreset.power_use);
        }

        protected override void OnUpdateSettingValue()
        {
            
        }

        protected override void OnUpdateInformationValue()
        {
            
        }

        public override void OnBeginPlace()
        {
            if(_buildingSaveData.saveDataObject != null)
            {
                var savedata = JsonConvert.SerializeObject(_buildingSaveData.saveDataObject);
                var a = JsonConvert.DeserializeObject<People_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _peopleSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _peopleSaveData;
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