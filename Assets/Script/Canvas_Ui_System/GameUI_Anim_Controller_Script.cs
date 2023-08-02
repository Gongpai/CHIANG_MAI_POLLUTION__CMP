using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class GameUI_Anim_Controller_Script : MonoBehaviour
    {
        [SerializeField] private GameObject ToolPanel;
        [SerializeField] private GameObject ContentBar;
        [SerializeField] private GameObject HomeBar;

        private Panel_UI_Script _panelUIScriptToolPanel;
        private Panel_UI_Script _panelUIScriptContentBar;
        private Panel_UI_Script _panelUIScriptHomeBar;
        private Animator _AnimToolPanal;
        private Animator _AnimHomeBar;

        public GameObject toolPanel
        {
            get { return ToolPanel; }
            set { ToolPanel = value; }
        }
        public GameObject contentBar
        {
            get { return ContentBar; }
            set { ContentBar = value; }
        }
        public GameObject homeBar
        {
            get { return HomeBar; }
            set { HomeBar = value; }
        }

        public Panel_UI_Script PanelUIScriptToolPanel
        {
            get
            {
                if (_panelUIScriptToolPanel == null)
                {
                    _panelUIScriptToolPanel = ToolPanel.GetComponent<Panel_UI_Script>();
                }
                return _panelUIScriptToolPanel;
            }
        }

        public Panel_UI_Script PanelUIScriptContentBar
        {
            get
            {
                _panelUIScriptContentBar = ToolPanel.GetComponent<Panel_UI_Script>();
                return _panelUIScriptContentBar;
            }
        }

        public Panel_UI_Script PanelUIScriptHomeBar
        {
            get
            {
                _panelUIScriptHomeBar = ToolPanel.GetComponent<Panel_UI_Script>();
                return _panelUIScriptHomeBar;
            }
        }

        private void Awake()
        {
            _panelUIScriptToolPanel = ToolPanel.GetComponent<Panel_UI_Script>();
            _panelUIScriptContentBar = ToolPanel.GetComponent<Panel_UI_Script>();
            _panelUIScriptHomeBar = ToolPanel.GetComponent<Panel_UI_Script>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _AnimToolPanal = ToolPanel.GetComponent<Panel_UI_Script>().Panel_Animator;
            _AnimHomeBar = HomeBar.GetComponent<Panel_UI_Script>().Panel_Animator;
            
            _AnimToolPanal.SetBool("IsStart", true);
        }

        public void PlayAnimToolBar(bool IsPlayStart = true)
        {
            _AnimToolPanal.SetBool("IsStart", IsPlayStart);
        }
        
        public void PlayAnimHomeBar(bool IsPlayStart = true)
        {
            _AnimHomeBar.SetBool("IsStart", IsPlayStart);
        }
    }
}
