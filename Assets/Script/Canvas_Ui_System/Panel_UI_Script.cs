using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GDD
{
    public class Panel_UI_Script : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Animator panel_Animator;
        [SerializeField] private List<GameObject> List_Button = new List<GameObject>();
        private List<Button_Control_Script> List_BC_Script = new List<Button_Control_Script>();

        private PanelActionMode actionMode;
        private bool IsPointEnter = false;

        public PanelActionMode ActionMode
        {
            get { return actionMode; }
            set { actionMode = value; }
        }
        public List<GameObject> list_Button
        {
            get { return List_Button; }
        }

        public Animator Panel_Animator
        {
            get
            {
                if (panel_Animator == null)
                    panel_Animator = GetComponent<Animator>();
                
                return panel_Animator;
            }
            set { panel_Animator = value; }
        }
        
        public List<Button_Control_Script> list_BC_Script
        {
            get
            {
                if (List_BC_Script.Count <= 0)
                {
                    foreach (var _list_button in List_Button)
                    {
                        List_BC_Script.Add(_list_button.GetComponent<Button_Control_Script>());
                    }
                }

                return List_BC_Script;
            }
        }

        private void Awake()
        {
            if (panel_Animator == null)
                panel_Animator = GetComponent<Animator>();

        }

        private void Start()
        {
            if (List_BC_Script.Count <= 0)
            {
                foreach (var list_button in List_Button)
                {
                    List_BC_Script.Add(list_button.GetComponent<Button_Control_Script>());
                    print(list_button.GetComponent<Button_Control_Script>() == null);
                }
            }
        }

        private void Update()
        {
            if (actionMode == PanelActionMode.Auto_Hide && !IsPointEnter && panel_Animator.GetCurrentAnimatorStateInfo(0).IsName("ToolPanal_Anim_In"))
            {
                PlayPanelAniOut();
            }
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            IsPointEnter = true;
        }
        
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            IsPointEnter = false;
            if (actionMode == PanelActionMode.Auto_Hide)
            {
                PlayPanelAniOut();
            }
            print("Exit- dvggfdgdfgfdg");
        }

        public void PlayPanelAniOut()
        {
            panel_Animator.SetBool("IsStart", false);
            List_BC_Script[0].GameUI_ACS.PlayAnimHomeBar();
        }
    }
}
