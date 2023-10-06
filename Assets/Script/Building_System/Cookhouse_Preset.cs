using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Cookhouse Building Preset",
        menuName = "GDD/Building/Resources/Cookhouse", order = 0)]
    public class Cookhouse_Preset : ScriptableObject
    {
        [Header("Food produced")]
        public int food = 10;
        
        [Header("Resources Use")] 
        public int wood_use;
        public int raw_food_use;
    }
}