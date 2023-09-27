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
        
        private float pm2_5_delta = 1;
        private bool is_add_value = false;

        public int pm2_5_value
        {
            get => GM.PM_25;
            set => GM.PM_25 = 300;
        }
        
        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void Update()
        {
            if (pm2_5_value > 310)
            {
                is_add_value = false;
            }else if (pm2_5_value <= 1 )
            {
                is_add_value = true;
            }

            if (is_add_value)
            {
                pm2_5_delta += Time.deltaTime * 25.0f;
            }
            else
            {
                pm2_5_delta -= Time.deltaTime * 25.0f;
            }

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

            pm2_5_value = (int)pm2_5_delta;
            m_canvasElementList.texts[0].text = pm2_5_value.ToString();
            m_canvasElementList.animators[0].SetInteger("Value", pm2_5_value);
        }
    }
}