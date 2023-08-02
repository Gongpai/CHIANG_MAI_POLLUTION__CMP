using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class HomeBar_Button_Script : MonoBehaviour
    {
        private Button homeBar_button;
        private Button_Control_Script BC_Script;
        // Start is called before the first frame update

        private void Awake()
        {
            BC_Script = gameObject.AddComponent<Button_Control_Script>();
            BC_Script.GameUI_ACS = FindObjectOfType<GameUI_Anim_Controller_Script>();
        }

        void Start()
        {
            homeBar_button = GetComponent<Button>();
            homeBar_button.onClick.AddListener(() =>
            {
                BC_Script.GameUI_ACS.PlayAnimToolBar();
                BC_Script.GameUI_ACS.PlayAnimHomeBar(false);
                BC_Script.GameUI_ACS.PanelUIScriptToolPanel.ActionMode = PanelActionMode.Auto_Hide;
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
