using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Generator Building Preset",
        menuName = "GDD/Building/Generator", order = 0)]
    public class Building_Preset : ScriptableObject
    {
        public string name = "Generator";

        public float power = 10;
    }
}