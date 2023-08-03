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
        private UI_Control_Script UI_CS;

        private PanelActionMode actionMode;
        private bool IsPointEnter = false;

        public UI_Control_Script ui_cs
        {
            get { return UI_CS; }
            set { UI_CS = value; }
        }
        
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
                
                print("------------------- : " + panel_Animator);
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

                foreach (var bc_script in List_BC_Script)
                {
                    bc_script.UI_CS = UI_CS;
                    bc_script.panelUIScript = this;
                }
            }
        }

        private void Update()
        {
            if (actionMode == PanelActionMode.Auto_Hide && !IsPointEnter && panel_Animator.GetCurrentAnimatorStateInfo(0).IsName("ToolPanal_Anim_In"))
            {
                //print("mode " + (actionMode == PanelActionMode.Auto_Hide));
                PlayPanelAniOut();
            }
            
        }

        public int CheckUI_StillOpen()
        {
            int OpenCanvasOrder = 0;
            foreach (var bc_script in List_BC_Script)
            {
                if (bc_script.canvasSpawn != null)
                {
                    bc_script.canvasSpawn.GetComponent<Canvas>().sortingOrder = 0;
                    OpenCanvasOrder = 1;
                    bc_script.OnDestroyCanvas(false);
                }
            }

            return OpenCanvasOrder;
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
            List_BC_Script[0].UI_CS.PlayAnimPanel(1, true);
            List_BC_Script[0].UI_CS.PlayAnimPanel(0, false);
        }
    }
}
