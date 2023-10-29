using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class OpenUI_Button_Script : MonoBehaviour
    {
        [SerializeField] private GameObject button_BG;
        [SerializeField] private GameObject Canvas_to_create;
        [SerializeField] private float m_planeDistance = 2.0f;
        [SerializeField] private bool useCameraOverlay = false;
        [SerializeField] private int index = 0;
        private bool is_show_pause_menu;
        private UnityEvent _button_event = new UnityEvent();

        private Button_Control_Script BC_Script;
        
        public UnityEvent button_event
        {
            get => _button_event;
        }
        
        
        private void Awake()
        {
            BC_Script = gameObject.AddComponent<Button_Control_Script>();
            BC_Script.UI_CS = FindObjectOfType<UI_Control_Script>();
        }

        void Start()
        {
            BC_Script.button_bg = button_BG;
            BC_Script.OnClickButton(() =>
            {
                //print(gameObject.name + " Click!!!!!!!!!!!!!!!!!");
                if (Canvas_to_create != null && !BC_Script.IsCreateCanvas)
                {
                    BC_Script.canvas_to_create = Canvas_to_create;
                    BC_Script.OnCreateCanvas(m_planeDistance, useCameraOverlay, index);
                }
                else
                {
                    BC_Script.OnDestroyCanvas();
                }
            });

            _button_event.AddListener(() =>
            {
                //print(gameObject.name + " Click!!!!!!!!!!!!!!!!!");
                if (Canvas_to_create != null && !BC_Script.IsCreateCanvas)
                {
                    BC_Script.canvas_to_create = Canvas_to_create;
                    BC_Script.OnCreateCanvas(m_planeDistance, useCameraOverlay, index);
                    is_show_pause_menu = true;
                }
                else
                {
                    BC_Script.OnDestroyCanvas(); 
                    is_show_pause_menu = false;
                    Time_Controll_UI_Script.auto_Resume_Time();
                }
            });
        }

        private void Update()
        {
            if (is_show_pause_menu)
            {
                Time_Controll_UI_Script.SetSpeed(0);
            }
        }
    }
}
