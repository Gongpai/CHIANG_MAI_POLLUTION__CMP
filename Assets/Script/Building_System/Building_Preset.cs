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

        [Header("Jab Type")] 
        public PeopleJob job;
        
        [Header("Building Requirement")] 
        public int max_people = 10;
        public int max_worker = 10;
        
        [Header("Resources Build")] 
        public int wood_build;
        public int rock_build;
        public float time_construction = 12;
        
        [Header("Building Power Use")] 
        public float power_use;
        
        [Header("Resources Use Rate / Hour")] 
        public int resources_use_rate;
        
        [Header("Building Ability")] 
        public int air_filtration_ability = 50;
        
        [Header("Setting Data")] 
        public List<Building_Setting_Data> m_building_setting = new List<Building_Setting_Data>();
        
        [Header("Status & Information Data")] 
        public List<Building_Information_Preset> m_building_status = new List<Building_Information_Preset>();
        public List<Building_Information_Preset> m_building_information = new List<Building_Information_Preset>();
    }
}