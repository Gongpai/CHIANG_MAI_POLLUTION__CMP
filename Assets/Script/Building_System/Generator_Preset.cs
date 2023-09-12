using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Generator Building Preset",
        menuName = "GDD/Building/Generator", order = 1)]
    public class Generator_Preset : ScriptableObject
    {
        [Header("Power produced")]
        public float power = 10;

        [Header("Resources Use")] 
        public int wood_use;
    }
}