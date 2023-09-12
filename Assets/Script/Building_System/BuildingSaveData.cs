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
        public bool WorkOverTime;
        public int people;
        public int worker;
        public float construction_In_Progress;
        public float efficiency = 1;
        public float re_userate_hour = 0;
        public bool building_is_placed = false;
        public bool construction_is_in_progress;
        public bool building_is_remove;
        
        //For other child class
        public object saveDataObject;
    }
}