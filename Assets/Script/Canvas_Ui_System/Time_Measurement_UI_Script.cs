using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Time_Measurement_UI_Script : MonoBehaviour
    {
        [SerializeField] private GameObject m_area_Time;
        [SerializeField] private GameObject m_prefab_time_measurement_line;
        [SerializeField] private GameObject m_prefab_pm2_5_warning;
        
        private TimeManager TM;
        private GameManager GM;
        private GameInstance GI;
        private List<GameObject> m_time_measurement_line_lists = new List<GameObject>();
        private List<GameObject> warning_pm2_5_lists = new List<GameObject>();
        private int day = 0;

        private float pos_spawn_warning
        {
            get => GI.timeMeasurementSaveData.pos_spawn_warning;
            set => GI.timeMeasurementSaveData.pos_spawn_warning = value;
        }

        private int current_datetime
        {
            get => GI.timeMeasurementSaveData.current_datetime;
            set => GI.timeMeasurementSaveData.current_datetime = value;
        }

        private int default_hour_spawn
        {
            get => GI.timeMeasurementSaveData.default_hour_spawn;
            set => GI.timeMeasurementSaveData.default_hour_spawn = value;
        }

        private int offset_warning_spawn
        {
            get => GI.timeMeasurementSaveData.offset_warning_spawn;
            set => GI.timeMeasurementSaveData.offset_warning_spawn = value;
        }

        private List<float> pos_warning_lists
        {
            get => GI.timeMeasurementSaveData.pos_warning_lists;
            set => GI.timeMeasurementSaveData.pos_warning_lists = value;
        }

        private int spawn_number
        {
            get => GI.timeMeasurementSaveData.spawn_number;
            set => GI.timeMeasurementSaveData.spawn_number = value;
        }
        
        private int olddyaa = 0;

        private void OnEnable()
        {
            TM = TimeManager.Instance;
            GM = GameManager.Instance;
            GI = GM.gameInstance;
        }

        private void Start()
        {
            GI.current_day_mp2_5 += GI.day_before_pm2_5;
            m_time_measurement_line_lists.Add(Instantiate(m_prefab_time_measurement_line, m_area_Time.transform));
            m_time_measurement_line_lists[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            m_time_measurement_line_lists[0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            SetDayText(m_time_measurement_line_lists[0]);
            
            m_time_measurement_line_lists.Add(Instantiate(m_prefab_time_measurement_line, m_area_Time.transform));
            m_time_measurement_line_lists[1].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            m_time_measurement_line_lists[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(174, 0);
            SetDayText(m_time_measurement_line_lists[1]);
            
            print("OnLoad : " + TM.To_TotalSecond(GI.getSaveGameDateTime()));
            if(TM.To_TotalSecond(GI.getSaveGameDateTime()) > 0)
                OnLoadSave();
        }

        private void Update()
        {
            /*
            if(TM.getTotalDay != olddyaa)
                print("Day Move : " + m_time_measurement_line_lists[0].GetComponent<RectTransform>().anchoredPosition.x);
            */
            olddyaa = TM.getTotalDay;
            
            Add_PM2_5_Warning();
            
            //print("Scale : " + ((m_time_measurement_line_lists[0].GetComponent<RectTransform>().rect.width / 675) * 100) + "%");
            foreach (var time_measurement_line in m_time_measurement_line_lists)
            {
                RectTransform _rectTransform = time_measurement_line.GetComponent<RectTransform>();
                
                float moved = (3.50000000f * (725.00000000f / 700.00000000f) / 2.00000000f) / 3600.00000000f;
                _rectTransform.anchoredPosition -= new Vector2(moved * TM.deltaTime, 0);
                
                if (_rectTransform.anchoredPosition.x < -175.00000000f)
                {
                    float offset_repos = -175.00000000f - _rectTransform.anchoredPosition.x;
                    //print("Total offset : " + (174.00000000f - offset_repos));
                    _rectTransform.anchoredPosition = new Vector2(173 - offset_repos, 0);
                    SetDayText(time_measurement_line);
                }
            }

            int _i = 0;
            foreach (var warning_pm2_5 in warning_pm2_5_lists)
            {
                float moved = (3.50000000f * (725.00000000f / 700.00000000f) / 2.00000000f) / 3600.00000000f;
                //print("Current : " + pos_warning_lists.Count);
                RectTransform rectTransform = warning_pm2_5.GetComponent<RectTransform>();
                rectTransform.anchoredPosition -= new Vector2(moved * TM.deltaTime, 0);
                pos_warning_lists[_i] = rectTransform.anchoredPosition.x;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -10);
                _i++;
            }

            for (int i = 0; i < warning_pm2_5_lists.Count; i++)
            {
                if (warning_pm2_5_lists[i].GetComponent<RectTransform>().anchoredPosition.x <= -18)
                {
                    Destroy(warning_pm2_5_lists[i]);
                    warning_pm2_5_lists.Remove(warning_pm2_5_lists[i]);
                    pos_warning_lists.RemoveAt(i);
                }
            }
        }

        private void Add_PM2_5_Warning()
        {
            print("Check Day Spawn : " + (int)TM.To_Totalday(TM.get_DateTime) + " | " + (GI.current_day_mp2_5 - offset_warning_spawn));
            print("Check is 7 : " + TM.getGameTimeHour + " | " + default_hour_spawn);
            print("Check is same value : " + current_datetime  + " | " + default_hour_spawn);
            if ((int)TM.To_Totalday(TM.get_DateTime) == GI.current_day_mp2_5 - offset_warning_spawn && TM.getGameTimeHour == default_hour_spawn && current_datetime != default_hour_spawn)
            {
                print("ONLOADDDDDDDDDDDDDDDDDDDDD");
                current_datetime = default_hour_spawn;
                
                GI.current_day_mp2_5 += GI.day_before_pm2_5 + GI.day_after_pm2_5;
                pos_warning_lists = new List<float>();
                SpawnWarning(5);
                SpawnWarning(5 + GI.day_after_pm2_5);
                spawn_number++;
                
                if (spawn_number == 3 && (int)TM.To_Totalday(TM.get_DateTime) > 0 && (int)TM.To_Totalday(TM.get_DateTime) <= 90)
                {
                    spawn_number = 0;
                    offset_warning_spawn += 1;
                    GI.day_before_pm2_5 -= 1;
                    GI.day_after_pm2_5 += 1;
                }
            } else if (TM.getGameTimeHour == default_hour_spawn + 1)
            {
                current_datetime = 0;
            }
        }

        private void SpawnWarning(int day_number)
        {
            warning_pm2_5_lists.Add(Instantiate(m_prefab_pm2_5_warning, m_area_Time.transform));
            float move = (day_number) * (175 / 4);
            pos_warning_lists.Add(move);
            RectTransform warning = warning_pm2_5_lists[warning_pm2_5_lists.Count - 1].GetComponent<RectTransform>();
            warning.anchoredPosition = new Vector2(move, 0);
        }
        
        private void SetDayText(GameObject time_measurement_line)
        {
            foreach (var text in time_measurement_line.GetComponent<Canvas_Element_List>().texts)
            {
                Debug.LogWarning("DAYYYYYY IS : " + day);

                text.text = day.ToString();
                day++;
            }
        }

        private void OnLoadSave()
        {
            if (GI.gameDateTime != null)
            {
                float moved = (3.50000000f * (725.00000000f / 700.00000000f) / 2.00000000f);
                float totalmoved = moved * (TM.To_TotalSecond(GI.getSaveGameDateTime()) / 3600);
                Debug.LogWarning("Toal Line Moved from time is : " + (totalmoved - Time.deltaTime));
                Debug.LogWarning("This time : " + TM.To_TotalSecond(TM.get_DateTime) + " | Min : " + TM.get_DateTime.Minute + " | Sec : " + TM.get_DateTime.Second + " | MilliSec : " + TM.get_DateTime.Millisecond);

                for(int i = 0; i < m_time_measurement_line_lists.Count; i++)
                {
                    RectTransform _rectTransform = m_time_measurement_line_lists[i].GetComponent<RectTransform>();
                    
                    if (i > 0)
                        totalmoved -= 174.00000000f;
                    
                    _rectTransform.anchoredPosition = new Vector2(-totalmoved, 0);
                }

                for (int i = 0; i < GI.timeMeasurementSaveData.pos_warning_lists.Count; i++)
                {
                    warning_pm2_5_lists.Add(Instantiate(m_prefab_pm2_5_warning, m_area_Time.transform));
                    RectTransform warning = warning_pm2_5_lists[warning_pm2_5_lists.Count - 1].GetComponent<RectTransform>();
                    print("Pos Warning : " + GI.timeMeasurementSaveData.pos_warning_lists[i]);
                    warning.anchoredPosition = new Vector2(GI.timeMeasurementSaveData.pos_warning_lists[i], 0);
                }
            }
        }

        private void OnDisable()
        {
            
        }
    }
}