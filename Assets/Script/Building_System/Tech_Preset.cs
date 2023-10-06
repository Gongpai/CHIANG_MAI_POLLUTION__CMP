using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Tech Building Preset",
        menuName = "GDD/Building/Tech", order = 5)]
    public class Tech_Preset : ScriptableObject
    {
        [Header("Food produced")]
        public int token;
    }
}