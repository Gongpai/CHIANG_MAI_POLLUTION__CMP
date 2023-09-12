using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

namespace GDD
{
    public class Resources_Script : Building_System_Script
    {
        private Resources_SaveData _resourcesSaveData = new Resources_SaveData();

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
                var a = JsonConvert.DeserializeObject<Resources_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _resourcesSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _resourcesSaveData;
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