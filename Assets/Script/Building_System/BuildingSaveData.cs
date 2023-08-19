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
        
        //For other child class
        public object saveDataObject;
    }
}