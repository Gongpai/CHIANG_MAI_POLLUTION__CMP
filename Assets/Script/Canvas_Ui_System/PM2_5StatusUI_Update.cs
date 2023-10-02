using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class PM2_5StatusUI_Update : MonoBehaviour
    {
        [SerializeField]private Canvas_Element_List m_canvasElementList;
        [SerializeField] private List<Color> m_color = new List<Color>();
        private GameManager GM;
        private PM2_5_System_Script PM25;

        private int pm2_5_value
        {
            get => PM25.pm2_5_value;
        }
        
        private void Start()
        {
            GM = GameManager.Instance;
            PM25 = PM2_5_System_Script.Instance;
        }

        private void Update()
        {
            if (pm2_5_value < 50)
            {
                m_canvasElementList.image[0].color = m_color[0];
            }
            else if(pm2_5_value < 100)
            {
                m_canvasElementList.image[0].color = m_color[1];
            }
            else if(pm2_5_value < 150)
            {
                m_canvasElementList.image[0].color = m_color[2];
            }
            else if(pm2_5_value < 200)
            {
                m_canvasElementList.image[0].color = m_color[3];
            }
            else if(pm2_5_value < 300)
            {
                m_canvasElementList.image[0].color = m_color[4];
            }
            else
            {
                m_canvasElementList.image[0].color = m_color[5];
            }
            
            m_canvasElementList.texts[0].text = pm2_5_value.ToString();
            m_canvasElementList.animators[0].SetInteger("Value", pm2_5_value);
        }
    }
}