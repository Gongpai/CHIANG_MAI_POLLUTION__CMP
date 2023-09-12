using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Tech Building Preset",
        menuName = "GDD/Building/Tech", order = 5)]
    public class Tech_Preset : ScriptableObject
    {
        [Header("Research Speed %/h")]
        public float speed = 10;
        
        [Header("Resources Use")] 
        public float power_use;
    }
}