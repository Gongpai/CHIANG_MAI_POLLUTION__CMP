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
                addListenerTimeSpeed(2, 2);
                addListenerTimeSpeed(3, 3);
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
        
        public static void SetSpeed(float speed)
        {
            if (speed == 0)
            {
                TimeManager.Instance.timeScale = 0;
            }

            if (speed == 1)
            {
                TimeManager.Instance.timeScale = 1;
                timeSpeed = 1;
            }

            if (speed == 2)
            {
                TimeManager.Instance.timeScale = 1.3f;
                timeSpeed = 2;
            }

            if (speed >= 3)
            {
                TimeManager.Instance.timeScale = 1.6f;
                timeSpeed = 3;
            }
        }

        public static void auto_Resume_Time()
        {
            switch (timeSpeed)
            {
                case 0:
                    TimeManager.Instance.timeScale = 0;
                    break;
                case 1:
                    TimeManager.Instance.timeScale = 1;
                    break;
                case 2:
                    TimeManager.Instance.timeScale = 1.3f;
                    break;
                case 3:
                    TimeManager.Instance.timeScale = 1.6f;
                    break;
                default:
                    break;
            }
        }
    }
}