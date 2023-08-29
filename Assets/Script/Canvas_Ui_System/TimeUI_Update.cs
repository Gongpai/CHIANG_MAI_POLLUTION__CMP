using System;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class TimeUI_Update : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_worktime;
        [SerializeField] private TextMeshProUGUI m_day;

        private TimeManager TM;

        private void Start()
        {
            TM = TimeManager.Instance;
        }

        private void Update()
        {
            m_worktime.text = "Work TIme " + FixTimeText(TM.getGameTimeHour.ToString()) + ":" + FixTimeText(TM.getGameTimeMinute.ToString());
            m_day.text = "Day " + TM.getTotalDay;
        }

        private string FixTimeText(string time_text)
        {
            if (time_text.Length <= 1)
            {
                return "0" + time_text;
            }
            else
            {
                return time_text;
            }
        }
    }
}