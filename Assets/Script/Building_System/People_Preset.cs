﻿using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "People Building Preset",
        menuName = "GDD/Building/People", order = 3)]
    public class People_Preset : ScriptableObject
    {
        [Header("Residents")]
        public float people = 10;

        [Header("Resources Use")] 
        public float power_use;
    }
}