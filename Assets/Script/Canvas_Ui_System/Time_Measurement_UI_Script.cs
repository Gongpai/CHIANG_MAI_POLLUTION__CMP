using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Time_Measurement_UI_Script : MonoBehaviour
    {
        [SerializeField] private GameObject m_area_Time;
        [SerializeField] private GameObject m_prefab_time_measurement_line;
        
        private TimeManager TM;
        private List<GameObject> m_time_measurement_line_lists = new List<GameObject>();
        private int day = 0;

        private int olddyaa = 0;

        private void OnEnable()
        {
            TM = TimeManager.Instance;
        }

        private void Start()
        {
            m_time_measurement_line_lists.Add(Instantiate(m_prefab_time_measurement_line, m_area_Time.transform));
            m_time_measurement_line_lists[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            SetDayText(m_time_measurement_line_lists[0]);
            
            m_time_measurement_line_lists.Add(Instantiate(m_prefab_time_measurement_line, m_area_Time.transform));
            m_time_measurement_line_lists[1].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            m_time_measurement_line_lists[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(174, 0);
            SetDayText(m_time_measurement_line_lists[1]);
        }

        private void Update()
        {
            if(TM.getDayTotal != olddyaa)
                print("Day Move : " + m_time_measurement_line_lists[0].GetComponent<RectTransform>().anchoredPosition.x);
            olddyaa = TM.getDayTotal;
            
            //print("Scale : " + ((m_time_measurement_line_lists[0].GetComponent<RectTransform>().rect.width / 675) * 100) + "%");
            foreach (var time_measurement_line in m_time_measurement_line_lists)
            {
                RectTransform _rectTransform = time_measurement_line.GetComponent<RectTransform>();

                //print("CULLLL : " + ((((14.000f / 4.000f) * (725.000f / 700.000f)) / 2.000f) / 3600.000f));
                
                _rectTransform.anchoredPosition -= new Vector2((3.50000000f * (725.00000000f / 700.00000000f) / 2.00000000f) / 3600.00000000f, 0) * TM.deltaTime;
                
                if (_rectTransform.anchoredPosition.x < -175.00000000f)
                {
                    float offset_repos = -175.00000000f - _rectTransform.anchoredPosition.x;
                    print("Total offset : " + (174.00000000f - offset_repos));
                    _rectTransform.anchoredPosition = new Vector2(173 - offset_repos, 0);
                    SetDayText(time_measurement_line);
                }
            }
        }

        private void SetDayText(GameObject time_measurement_line)
        {
            foreach (var text in time_measurement_line.GetComponent<Canvas_Element_List>().tests)
            {
                text.text = day.ToString();
                day++;
            }
        }
    }
}