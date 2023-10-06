using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Gate Preset", menuName = "GDD/Gate", order = 0)]
    public class Gate_Preset : ScriptableObject
    {
        public int max_survivor = 10;
    }
}