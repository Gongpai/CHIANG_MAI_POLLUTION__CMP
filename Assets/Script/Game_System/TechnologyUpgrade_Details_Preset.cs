using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "Generator Building Preset",
        menuName = "GDD/TechnologyDetails", order = 4)]
    public class TechnologyUpgrade_Details_Preset : ScriptableObject
    {
        [Header("TechnologyUpgrade Details")]
        public List<TechnologyUpgrade_Details_List> lists;
    }

    [Serializable]
    public class TechnologyUpgrade_Details_List
    {
        public List<TechnologyUpgrade_Details> details;

        public TechnologyUpgrade_Details_List(List<TechnologyUpgrade_Details> _details)
        {
            details = _details;
        }
    }
    
    [Serializable]
    public struct TechnologyUpgrade_Details
    {
        public string name;
        public string title;
        [TextArea]public string message;
        [TextArea]public string disciption;

        public TechnologyUpgrade_Details(string _name, string _title, string _message, string _disciption)
        {
            name = _name;
            title = _title;
            message = _message;
            disciption = _disciption;
        }
    }
}