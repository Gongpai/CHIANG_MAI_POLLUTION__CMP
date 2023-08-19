using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

namespace GDD
{
    public class People_Script : Building_System_Script
    {
        private People_SaveData _peopleSaveData = new People_SaveData();
        
        public override void BeginStart()
        {
            
        }

        public override void EndStart()
        {
            
        }

        protected override void OnUpdateValue()
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
    }
}