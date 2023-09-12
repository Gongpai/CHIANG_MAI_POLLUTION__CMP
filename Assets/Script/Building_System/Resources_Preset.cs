using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Resources Building Preset",
        menuName = "GDD/Building/Resources", order = 4)]
    public class Resources_Preset : ScriptableObject
    {
        [Header("Residents")]
        public float people = 10;

        [Header("Resources Use")] 
        public float power_use;
    }
}