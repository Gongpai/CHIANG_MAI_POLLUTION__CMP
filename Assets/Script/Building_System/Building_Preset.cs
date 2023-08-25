using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Building Preset",
        menuName = "GDD/Building/Building", order = 0)]
    public class Building_Preset : ScriptableObject
    {
        public string name = "Generator";
        public int level = 1;
        public int max_people = 10;
        public int max_worker = 10;
        public List<Building_Setting_Data> m_building_setting = new List<Building_Setting_Data>();
        public List<Building_Information_Preset> m_building_status = new List<Building_Information_Preset>();
        public List<Building_Information_Preset> m_building_information = new List<Building_Information_Preset>();
    }
}