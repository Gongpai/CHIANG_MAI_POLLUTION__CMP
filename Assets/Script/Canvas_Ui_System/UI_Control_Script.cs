using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace GDD
{
    public class UI_Control_Script : MonoBehaviour, IGameUI
    {
        private List<GameObject> list_Panel = new List<GameObject>();
        private List<Panel_UI_Script> _panelUIScript = new List<Panel_UI_Script>();
        private List<Animator> _AnimPanal = new List<Animator>();
        private Object ui_Script;

        public List<GameObject> List_Panel
        {
            get { return list_Panel; }
            set { list_Panel = value; }
        }

        public Object Ui_Script
        {
            get { return ui_Script; }
            set { ui_Script = value; }
        }

        public List<Panel_UI_Script> PanelUIScript
        {
            get
            {
                if (_panelUIScript.Count <= 0)
                {
                    foreach (var panal in list_Panel)
                    {
                        _panelUIScript.Add(panal.GetComponent<Panel_UI_Script>());
                    }
                }
                return _panelUIScript;
            }
        }

        private void Awake()
        {
            foreach (var panal in list_Panel)
            {
                print("Add panel : " + panal); 
                if(panal != null && panal.GetComponent<Panel_UI_Script>() != null)
                    panal.GetComponent<Panel_UI_Script>().ui_cs = this;
                
                _panelUIScript.Add(panal.GetComponent<Panel_UI_Script>());
            }
            foreach (var panal_script in _panelUIScript)
            {
                if (panal_script != null && panal_script.Panel_Animator != null)
                {
                    print("Add Anim ui : " + panal_script.name);
                    _AnimPanal.Add(panal_script.Panel_Animator);
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void PlayAnimPanel(int panelIndex, bool IsPlayStart = true)
        {
            if (_AnimPanal.Count <= 0)
            {
                foreach (var panal_script in _panelUIScript)
                {
                    print("Add in play Anim ui : " + panal_script.name);
                    if(panal_script.Panel_Animator != null)
                        _AnimPanal.Add(panal_script.Panel_Animator);
                }
            }
            
            print(_AnimPanal.Count + " : " + panelIndex);
            _AnimPanal[panelIndex].SetBool("IsStart", IsPlayStart);
            
        }
    }
}
