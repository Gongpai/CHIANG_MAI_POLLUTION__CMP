using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public enum ButtonActionMode
    {
        Can_Play,
        Dont_Play
    }

    public enum PanelActionMode
    {
        Dont_Hide,
        Auto_Hide
    }

    public enum Building_Setting_Type
    {
        Centor_Button_only,
        Centor_Button_with_progress,
        Bottom_Button_only
    }

    [Serializable] public enum Building_Information_Type
    {
        ShowStatus,
        ShowInformation
    }

    [Serializable]
    public enum Building_Show_mode
    {
        TextOnly,
        TextWith_ProgressBar
    }
}
