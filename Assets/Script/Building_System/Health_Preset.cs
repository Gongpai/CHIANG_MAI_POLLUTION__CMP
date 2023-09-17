using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Health Building Preset",
        menuName = "GDD/Building/Health", order = 2)]
    public class Health_Preset : ScriptableObject
    {
        [Header("People Receiving Treatment")]
        public float people = 10;
    }
}