using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Generator Building Preset",
        menuName = "GDD/Building/Generator", order = 1)]
    public class Generator_Preset : Building_Preset
    {
        public float power = 10;
    }
}