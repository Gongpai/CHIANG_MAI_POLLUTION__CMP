using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

namespace GDD
{
    public class Health_Script : Building_System_Script
    {
        private Health_SaveData _healthSaveData = new Health_SaveData();

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
                var a = JsonConvert.DeserializeObject<Health_SaveData>(savedata);
                //print("SaveData : " + savedata);
                _healthSaveData = a;
                
                //print("Over Is : " + _generatorSaveData.IsOverdrive);
            }
            
            _buildingSaveData.saveDataObject = _healthSaveData;
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