using System.Collections.Generic;

namespace GDD
{
    public class Static_Resource_SaveData
    {
        //Save Sata For Building
        public string nameObject;
        public bool is_work_overtime;
        public int id;
        public List<PeopleSystemSaveData> villagers;
        public List<PeopleSystemSaveData> workers;
        public float re_userate_hour = 0;
        public bool is_work_24h = false;
        public bool is_unlock = false;
        
        //For other child class
        public object saveDataObject;
    }
}