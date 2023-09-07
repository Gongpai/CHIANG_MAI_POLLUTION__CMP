using System;
using UnityEngine;

namespace GDD
{
    public class Time_Controll_UI_Script : MonoBehaviour
    {
        [SerializeField] private Button_Switch_Tab_Animation_Control m_buttonSwitchTab;
        public static float timeSpeed = 1;

        private int i_tab = 1;
        private Canvas_Element_List m_canvasElementList;
        private TimeManager TM;

        private void Start()
        {
            TM = TimeManager.Instance;
            m_canvasElementList = GetComponent<Canvas_Element_List>();

            if (GetComponent<Canvas_Element_List>() != null)
            {
                addListenerTimeSpeed(0, 0);
                addListenerTimeSpeed(1, 1);
                addListenerTimeSpeed(2, 1.3f);
                addListenerTimeSpeed(3, 1.6f);
            }
            
            print("Time Speed : " + timeSpeed);
        }

        private void Update()
        {
            if (m_buttonSwitchTab != null)
            {
                if (TM.timeScale == 0)
                {
                    m_buttonSwitchTab.OnSwitchTab(0);
                }
                else if (TM.timeScale <= 1)
                {
                    m_buttonSwitchTab.OnSwitchTab(1);
                }
                else if (TM.timeScale <= 1.3f)
                {
                    m_buttonSwitchTab.OnSwitchTab(2);
                }
                else if (TM.timeScale <= 1.6f)
                {
                    m_buttonSwitchTab.OnSwitchTab(3);
                }
            }
        }

        private void addListenerTimeSpeed(int index, float Speed)
        {
            if (index < m_canvasElementList.buttons.Count)
            {
                m_canvasElementList.buttons[index].onClick.AddListener(() =>
                {
                    SetSpeed(Speed);
                    i_tab = index;
                    
                    if (m_canvasElementList.buttons[index].GetComponent<OpenUI_Button_Script>() == null)
                    {
                        timeSpeed = index;
                        print("Time Speed : " + timeSpeed);
                    }
                });
            }
        }

        public void SetSpeed(float speed)
        {
            TM.timeScale = speed;
        }

        public void auto_Resume_Time()
        {
            switch (timeSpeed)
            {
                case 0:
                    TM.timeScale = 0;
                    break;
                case 1:
                    TM.timeScale = 1;
                    break;
                case 2:
                    TM.timeScale = 1.3f;
                    break;
                case 3:
                    TM.timeScale = 1.6f;
                    break;
                default:
                    break;
            }
        }
    }
}