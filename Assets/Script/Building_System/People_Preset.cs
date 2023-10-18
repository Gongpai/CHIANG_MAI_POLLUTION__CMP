using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "People Building Preset",
        menuName = "GDD/Building/People", order = 3)]
    public class People_Preset : ScriptableObject
    {
        [Header("Residents")]
        public int people = 10;

        [Header("AI Max")] 
        int ai_max;
    }
}