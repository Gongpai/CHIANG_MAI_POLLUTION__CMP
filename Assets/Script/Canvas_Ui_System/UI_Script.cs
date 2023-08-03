using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class UI_Script : MonoBehaviour
    {
        [SerializeField] private List<GameObject> list_Panel = new List<GameObject>();
        [SerializeField] private bool IsPlayPanelAnimOnStart = false;
        [SerializeField] private int Index_PanelAnimPlayOnStart = 0;
        private UI_Control_Script _uiControlScript;

        private void Awake()
        {
            _uiControlScript = GetComponent<UI_Control_Script>();
            _uiControlScript.List_Panel = list_Panel;
            _uiControlScript.Ui_Script = this;
        }

        private void Start()
        {
            if(IsPlayPanelAnimOnStart)
                _uiControlScript.PlayAnimPanel(Index_PanelAnimPlayOnStart, IsPlayPanelAnimOnStart);
        }
    }
}