using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    [CreateAssetMenu(fileName = "Static_Resource Preset",
        menuName = "GDD/Static_Resource", order = 2)]
    public class Static_Resource_Preset : ScriptableObject
    {
        public string name = "Generator";

        [Header("Building Requirement")] public int max_people = 10;
        public int max_worker = 10;

        [Header("Resources Product Output Rate / Hour")]
        public int product_output_resource;
        public int product_output_use_rate;

        [Header("Setting Data")]
        public List<Static_Resource_Setting_Data> m_static_resource_setting = new();

        [Header("Status & Information Data")]
        public List<Static_Resource_Information_Preset> m_static_resource_status = new ();
        public List<Static_Resource_Information_Preset> m_static_resource_information = new ();
    }
}
