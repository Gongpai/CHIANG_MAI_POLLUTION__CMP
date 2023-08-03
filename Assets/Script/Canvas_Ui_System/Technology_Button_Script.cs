using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GDD
{
    public class Technology_Button_Script : MonoBehaviour
    {
        [SerializeField] private GameObject button_BG;
        [SerializeField] private GameObject Canvas_to_create;

        private Button_Control_Script BC_Script;
        private bool IsCreateCanvas;

        private void Awake()
        {
            BC_Script = gameObject.AddComponent<Button_Control_Script>();
            BC_Script.UI_CS = FindObjectOfType<UI_Control_Script>();
        }

        private void Start()
        {
            BC_Script.button_bg = button_BG;
            BC_Script.OnClickButton(() =>
            {
                print(gameObject.name + " Click!!!!!!!!!!!!!!!!!");
                if (Canvas_to_create != null && !IsCreateCanvas)
                {
                    IsCreateCanvas = true;
                    BC_Script.canvas_to_create = Canvas_to_create;
                    BC_Script.OnCreateCanvas();
                }
                else
                {
                    IsCreateCanvas = false;
                    BC_Script.OnDestroyCanvas();
                }
            });
        }
    }
}