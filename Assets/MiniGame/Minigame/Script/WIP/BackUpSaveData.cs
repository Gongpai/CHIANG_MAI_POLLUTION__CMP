using System;
using System.Collections.Generic;

namespace GDD
{
    public class BackUpSaveData : Singleton_With_DontDestroy<BackUpSaveData>
    {
        public List<BuildingSaveData> buildingSaveDatas = new List<BuildingSaveData>();
        public List<RoadSaveData> RoadSaveDatas = new List<RoadSaveData>();
        public List<Static_Resource_SaveData> staticResourceSaveDatas = new List<Static_Resource_SaveData>();
        public List<PeopleSystemSaveData> villagerSaveDatas = new List<PeopleSystemSaveData>();
        public List<PeopleSystemSaveData> workerSaveDatas = new List<PeopleSystemSaveData>();

        private void Update()
        {
            if(buildingSaveDatas != null)
                print("Building Count " + buildingSaveDatas.Count);
        }
    }
}