using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    [CreateAssetMenu(fileName = "Health Building Preset",
        menuName = "GDD/Building/Health", order = 2)]
    public class Health_Preset : ScriptableObject
    {
        [Header("People Receiving Treatment")]
        public int patient = 10;
    }
}