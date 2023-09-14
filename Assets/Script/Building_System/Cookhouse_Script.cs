using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class Cookhouse_Script : Building_System_Script
    {
        [FormerlySerializedAs("m_resourcesPreset")] [SerializeField] private Cookhouse_Preset mCookhousePreset;
        private Cookhouse_SaveData _cookhouseSaveData = new Cookhouse_SaveData();

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
            
        }

        public override void OnDisableBuilding()
        {
            
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