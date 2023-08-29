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
        private GameManager GM;
        private GameInstance GI;
        private List<GameObject> m_time_measurement_line_lists = new List<GameObject>();
        private int day = 0;

        private int olddyaa = 0;

        private void OnEnable()
        {
            TM = TimeManager.Instance;
            GM = GameManager.Instance;
            GI = GM.gameInstance;
        }

        private void Start()
        {
            m_time_measurement_line_lists.Add(Instantiate(m_prefab_time_measurement_line, m_area_Time.transform));
            m_time_measurement_line_lists[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            m_time_measurement_line_lists[0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            SetDayText(m_time_measurement_line_lists[0]);
            
            m_time_measurement_line_lists.Add(Instantiate(m_prefab_time_measurement_line, m_area_Time.transform));
            m_time_measurement_line_lists[1].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            m_time_measurement_line_lists[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(174, 0);
            SetDayText(m_time_measurement_line_lists[1]);
            
            print("OnLoad : " + TM.To_TotalSecond(GI.getSaveGameDateTime));
            if(TM.To_TotalSecond(GI.getSaveGameDateTime) > 0)
                OnLoadSave();
        }

        private void Update()
        {
            if(TM.getTotalDay != olddyaa)
                print("Day Move : " + m_time_measurement_line_lists[0].GetComponent<RectTransform>().anchoredPosition.x);
            olddyaa = TM.getTotalDay;
            
            //print("Scale : " + ((m_time_measurement_line_lists[0].GetComponent<RectTransform>().rect.width / 675) * 100) + "%");
            foreach (var time_measurement_line in m_time_measurement_line_lists)
            {
                RectTransform _rectTransform = time_measurement_line.GetComponent<RectTransform>();

                float moved = (3.50000000f * (725.00000000f / 700.00000000f) / 2.00000000f) / 3600.00000000f;
                _rectTransform.anchoredPosition -= new Vector2(moved * TM.deltaTime, 0);
                
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

        private void OnLoadSave()
        {
            if (GI.gameDateTime != null)
            {
                float moved = (3.50000000f * (725.00000000f / 700.00000000f) / 2.00000000f);
                float totalmoved = moved * (TM.To_TotalSecond(GI.getSaveGameDateTime) / 3600);
                Debug.LogWarning("Toal Line Moved from time is : " + (totalmoved - Time.deltaTime));
                Debug.LogWarning("This time : " + TM.To_TotalSecond(TM.get_DateTime) + " | Min : " + TM.get_DateTime.Minute + " | Sec : " + TM.get_DateTime.Second + " | MilliSec : " + TM.get_DateTime.Millisecond);

                for(int i = 0; i < m_time_measurement_line_lists.Count; i++)
                {
                    RectTransform _rectTransform = m_time_measurement_line_lists[i].GetComponent<RectTransform>();
                    
                    if (i > 0)
                        totalmoved -= 174.00000000f;
                    
                    _rectTransform.anchoredPosition = new Vector2(-totalmoved, 0);
                }
            }
        }

        private void OnDisable()
        {
            
        }
    }
}