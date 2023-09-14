using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public struct Resource_Setting_UI_Data
    {
        public List<UnityAction> actions;
        public Static_Object_Resource_System_Script resourceSystemScript;
        public Building_Setting_Type buildingSettingType;
        public string text_enable;
        public string text_disable;
        public Sprite icon_enable;
        public Sprite icon_disable;
        public Color light_Color;
        public Color dark_Color;

        public Resource_Setting_UI_Data(List<UnityAction> _unityActions, Static_Object_Resource_System_Script _resourceSystemScript, Building_Setting_Type buildingSettingType, bool _active, float _value, float _maxValue, Sprite _iconEnable, Sprite _icon_disable, string _textEnable, string _textDisable, Color _lightColor, Color _darkColor)
        {
            actions = _unityActions;
            resourceSystemScript = _resourceSystemScript;
            this.buildingSettingType = buildingSettingType;
            icon_enable = _iconEnable;
            icon_disable = _icon_disable;
            text_enable = _textEnable;
            text_disable = _textDisable;
            light_Color = _lightColor;
            dark_Color = _darkColor;
        }
    }

    public struct Resource_Information_UI_Data
    {
        public string title;
        public string text;
        public float value;
        public float max_value;
        public Building_Information_Type buildingInformationType;

        public Resource_Information_UI_Data(string _title, string _text, float _value, float _max_value, Building_Information_Type _buildingInformationType)
        {
            title = _title;
            text = _text;
            value = _value;
            max_value = _max_value;
            buildingInformationType = _buildingInformationType;
        }
    }
}