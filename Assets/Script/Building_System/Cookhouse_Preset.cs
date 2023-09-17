using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Cookhouse Building Preset",
        menuName = "GDD/Building/Resources/Cookhouse", order = 0)]
    public class Cookhouse_Preset : ScriptableObject
    {
        [Header("Residents")]
        public float people = 10;
    }
}