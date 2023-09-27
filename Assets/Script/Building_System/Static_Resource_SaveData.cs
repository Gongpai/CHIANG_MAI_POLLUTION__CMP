using System.Collections.Generic;

namespace GDD
{
    public class Static_Resource_SaveData
    {
        //Save Sata For Building
        public string nameObject;
        public bool WorkOverTime;
        public int id;
        public List<PeopleSystemSaveData> villagers;
        public List<PeopleSystemSaveData> workers;
        public float re_userate_hour = 0;
    }
}