using System;
using UnityEngine;

namespace GDD
{
    public class TimeManager : Sinagleton_CanDestroy<TimeManager>
    {
        private GameManager GM;
        private GameInstance GI;
        private DateTime date_Time;
        private int oldyear = 1970;
        private int totalDay = 0;
        private bool isSetTIme = false;
        private int day = 0;
        private float _timescale = 1f;
        private float _deltaTime;
        
        public float timeScale
        {
            get => _timescale;
            set => _timescale = value;
        }

        public float deltaTime
        {
            get => _deltaTime;
        }

        public int getTotalSecond
        {
            get => Mathf.FloorToInt(To_TotalSecond(date_Time));
        }

        public DateTime get_DateTime
        {
            get => date_Time;
        }
        
        public int getGameTimeMinute
        {
            get => date_Time.Minute;
        }

        public int getGameTimeHour
        {
            get => date_Time.Hour;
        }

        public int getTotalDay
        {
            get => day;
        }

        private void OnEnable()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            
            if (GI.gameDateTime == null)
            {
                NewDateTime();
            }
            else
            {
                LoadDateTime();
            }
        }

        private void Update()
        {
            gameTimeSystem();
        }

        private void gameTimeSystem()
        {
            _deltaTime = Time.deltaTime * Mathf.Pow(1000, _timescale);
            date_Time = date_Time.AddSeconds(_deltaTime);

            if ((date_Time.DayOfYear == 365 && !DateTime.IsLeapYear(date_Time.Year)) || date_Time.DayOfYear == 366)
            {
                if (!isSetTIme)
                {
                    isSetTIme = true;
                    oldyear = date_Time.Year;
                    totalDay += date_Time.DayOfYear;
                }
            }
            else
            {
                isSetTIme = false;
            }

            day = Mathf.FloorToInt(To_Totalday(date_Time));
            
            GI.gameDateTime = new GameDateTime(date_Time.Year, date_Time.Month, date_Time.Day, date_Time.Hour, date_Time.Minute, date_Time.Second, date_Time.Millisecond);
            //print("Old Year : " + date_Time.Minute + " :: " + date_Time.DayOfYear);
        }

        public void NewDateTime()
        {
            date_Time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        }
        
        public void LoadDateTime()
        {
            Debug.LogWarning("Time New Scene : " + date_Time.ToUniversalTime());
            print("D" + GI.gameDateTime.day + " H" + GI.gameDateTime.hour + " M" + GI.gameDateTime.minute + " Sec" + GI.gameDateTime.second + " MiSec" + GI.gameDateTime.millisecond);
            
            date_Time = GI.getSaveGameDateTime();
        }

        public float To_TotalSecond(DateTime _dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = _dateTime - origin;
            return (float)diff.TotalSeconds;
        }

        public float To_Totalday(DateTime _dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = _dateTime - origin;
            return (float)diff.TotalDays;
        }
        
        public float To_TotalHour(DateTime _dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = _dateTime - origin;
            return (float)diff.TotalHours;
        }
    }
}