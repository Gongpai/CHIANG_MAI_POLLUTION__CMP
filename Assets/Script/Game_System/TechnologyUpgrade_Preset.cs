using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Generator Building Preset",
        menuName = "GDD/TechnologyUpgrade", order = 3)]
    public class TechnologyUpgrade_Preset : ScriptableObject
    {
        [Header("Level 1 Token Use")]
        public int generator_leveltwo;
        public int resident_leveltwo;
        public int wood_leveltwo;
        public int rock_leveltwo;
        public int food_leveltwo;
        
        [Header("Level 2 Token Use")] 
        public int level_two;
        public int resident_levelthree;
        public int gate_leveltwo;
        public int wood_levelthree;
        public int rock_levelthree;
        public int food_levelthree;
        public int rawfood_leveltwo;
        public int air_purifier_leveltwo;
        
        [Header("Level 3 Token Use")] 
        public int level_three;
        public int infirmary_unlock;
        public int gate_levelthree;
        public int wood_levelfour;
        public int rock_levelfour;
        public int food_levelfour;
        public int rawfood_levelthree;
        public int token_leveltwo;
        
        [Header("Level 4 Token Use")] 
        public int level_four;
        public int rawfood_levelfour;
        public int token_levelthree;
        public int air_purifier_levelthree;
    }
}