using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Building_Object_Script : MonoBehaviour
    {
        [SerializeField] private BuildingType _buildingType = BuildingType.Home_Small;

        public BuildingType BuildingType
        {
            get => _buildingType;
            set => _buildingType = value;
        }
    }
}