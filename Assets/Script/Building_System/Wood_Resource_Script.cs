using System;
using UnityEngine;

namespace GDD
{
    public class Wood_Resource_Script : Static_Object_Resource_System_Script
    {
        public override void Resource_product()
        {
            RM.Set_Resources_Tree(Mathf.CeilToInt(m_Resource_Preset.product_output_resource * efficiency));
        }

        public override void BeginStart()
        {
            BI_datas.Add(new (m_Resource_Preset.m_static_resource_status[0].title, m_Resource_Preset.m_static_resource_status[0].text + " " + efficiency, Building_Information_Type.ShowStatus));
        }

        public override void EndStart()
        {
            
        }
        
        public override void OnEnableBuilding()
        {
            
        }

        public override void OnDisableBuilding()
        {
            
        }

        protected override void OnUpdateSettingValue()
        {
            
        }

        protected override void OnUpdateInformationValue()
        {
            list_information_values.Add(new Tuple<object, object, string>(efficiency, 1.0f, m_Resource_Preset.m_static_resource_status[0].text+ " " + (efficiency * 100) + "%"));
        }

        public override void OnBeginPlace()
        {
            
        }

        public override void OnEndPlace()
        {
            
        }

        public override void OnBeginRemove()
        {
            
        }

        public override void OnEndRemove()
        {
            
        }

        public override void OnDestroyBuilding()
        {
            
        }
    }
}