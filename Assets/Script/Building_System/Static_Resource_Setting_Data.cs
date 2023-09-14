using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [Serializable]
    public struct Static_Resource_Setting_Data
    {
        [SerializeField] public string setting_name;
        [SerializeField] public Building_Setting_Type buildingSettingType;
        [SerializeField] public string text_enable;
        [SerializeField] public string text_disable;
        [SerializeField] public Sprite icon_enable;
        [SerializeField] public Sprite icon_disable;
        [SerializeField] public Color light_Color;
        [SerializeField] public Color dark_Color;
        
        public Static_Resource_Setting_Data(string name, Building_Setting_Type _buildingSettingType, Sprite _iconEnable, Sprite _icon_disable, string _textEnable, string _textDisable, Color _lightColor, Color _darkColor)
        {
            setting_name = name;
            buildingSettingType = _buildingSettingType;
            icon_enable = _iconEnable;
            icon_disable = _icon_disable;
            text_enable = _textEnable;
            text_disable = _textDisable;
            light_Color = _lightColor;
            dark_Color = _darkColor;
        }
    }

    [Serializable]
    public struct Static_Resource_Information_Preset
    {
        [SerializeField] public string title;
        [SerializeField][TextArea] public string text;
        
        public Static_Resource_Information_Preset(string _title, string _text)
        {
            title = _title;
            text = _text;
        }
    }
    
    public struct Static_Resource_info_struct
    {
        public List<Static_Resource_Information_Data> status;
        public List<Static_Resource_Information_Data> informations;

        public Static_Resource_info_struct(List<Static_Resource_Information_Data> _status, List<Static_Resource_Information_Data> _informations)
        {
            status = _status;
            informations = _informations;
        }
    }

    public struct Static_Resource_Information_Data
    {
        public string title;
        public string text;
        public Building_Information_Type buildingInformationType;
        
        public Static_Resource_Information_Data(string _title, string _text, Building_Information_Type _buildingInformationType)
        {
            title = _title;
            text = _text;
            buildingInformationType = _buildingInformationType;
        }
    }
}