using System.Collections.Generic;

namespace GDD
{
    public class BuildingSaveData
    {
        //Save Sata For Building
        public string nameObject;
        public string pathObject;
        public Vector3D Position;
        public Vector3D Rotation;
        public byte b_buildingtype;
        public bool Building_active;
        public bool Air_purifier_Speed_Up;
        public bool is_work_overtime;
        public float construction_In_Progress;
        public float re_userate_hour = 0;
        public List<PeopleSystemSaveData> villagers = new List<PeopleSystemSaveData>();
        public List<PeopleSystemSaveData> workers = new List<PeopleSystemSaveData>();
        public bool building_is_placed = false;
        public bool construction_is_in_progress;
        public bool building_is_remove;
        public bool is_auto_disable;
        public bool Building_Disable;
        public bool is_work_24h = false;
        
        //For other child class
        public object saveDataObject;
    }
}