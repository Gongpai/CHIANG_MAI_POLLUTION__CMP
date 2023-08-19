using System;
using UnityEngine;

namespace GDD
{
    [Serializable]
    public struct Building_Setting_Data
    {
        [SerializeField] public string setting_name;
        [SerializeField] public Building_Setting_Type buildingSettingType;
        [SerializeField] public string text_enable;
        [SerializeField] public string text_disable;
        [SerializeField] public Sprite icon_enable;
        [SerializeField] public Sprite icon_disable;
        [SerializeField] public Color light_Color;
        [SerializeField] public Color dark_Color;
        
        public Building_Setting_Data(string name, Building_Setting_Type _buildingSettingType, Sprite _iconEnable, Sprite _icon_disable, string _textEnable, string _textDisable, Color _lightColor, Color _darkColor)
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
}