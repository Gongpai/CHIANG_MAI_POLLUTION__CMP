using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public struct Building_Setting_UI_Data
    {
        public List<UnityAction> actions;
        public Building_System_Script buildingSystemScript;
        public Building_Setting_Type buildingSettingType;
        public string text_enable;
        public string text_disable;
        public Sprite icon_enable;
        public Sprite icon_disable;
        public Color light_Color;
        public Color dark_Color;

        public Building_Setting_UI_Data(List<UnityAction> _unityActions, Building_System_Script _buildingSystemScript, Building_Setting_Type buildingSettingType, bool _active, float _value, float _maxValue, Sprite _iconEnable, Sprite _icon_disable, string _textEnable, string _textDisable, Color _lightColor, Color _darkColor)
        {
            actions = _unityActions;
            buildingSystemScript = _buildingSystemScript;
            this.buildingSettingType = buildingSettingType;
            icon_enable = _iconEnable;
            icon_disable = _icon_disable;
            text_enable = _textEnable;
            text_disable = _textDisable;
            light_Color = _lightColor;
            dark_Color = _darkColor;
        }
    }
}