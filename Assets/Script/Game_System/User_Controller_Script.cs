using System;
using UnityEngine;

namespace GDD
{
    public class User_Controller_Script : MonoBehaviour
    {
        [SerializeField] private OpenUI_Button_Script open_PauseMenu;
        private bool is_pause = false;
        private UI_Control_Script UI_Script;
        
        private void Start()
        {
            UI_Script = FindObjectOfType<UI_Control_Script>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                open_PauseMenu.button_event.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                Time_Controll_UI_Script.SetSpeed(1);
                is_pause = false;
            }
            
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                Time_Controll_UI_Script.SetSpeed(2);
                is_pause = false;
            }
            
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                Time_Controll_UI_Script.SetSpeed(3);
                is_pause = false;
            }
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!is_pause)
                {
                    Time_Controll_UI_Script.SetSpeed(0);
                    is_pause = true;
                }
                else
                {
                    Time_Controll_UI_Script.auto_Resume_Time();
                    is_pause = false;
                }
            }
        }
    }
}