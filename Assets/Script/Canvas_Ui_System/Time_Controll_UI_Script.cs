using System;
using UnityEngine;

namespace GDD
{
    public class Time_Controll_UI_Script : MonoBehaviour
    {
        private Canvas_Element_List m_canvasElementList;
        private TimeManager TM;

        private void Start()
        {
            TM = TimeManager.Instance;
            m_canvasElementList = GetComponent<Canvas_Element_List>();
            m_canvasElementList.buttons[0].onClick.AddListener(() => { SetSpeed(0); });
            m_canvasElementList.buttons[1].onClick.AddListener(() => { SetSpeed(1); });
            m_canvasElementList.buttons[2].onClick.AddListener(() => { SetSpeed(1.5f); });
            m_canvasElementList.buttons[3].onClick.AddListener(() => { SetSpeed(1.75f); });
        }

        public void SetSpeed(float speed)
        {
            TM.timeScale = speed;
        }
    }
}