using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Road Preset", menuName = "GDD/Road", order = 1)]
    public class Road_Preset : ScriptableObject
    {
        public string name = "Road";
        public int level = 1;
        
        [Header("Resources Build")] 
        public int wood_build;
        public int rock_build;
        public float time_construction = 12;
        
        [Header("Road Power Use")] 
        public float power_use = 0;

    }
}