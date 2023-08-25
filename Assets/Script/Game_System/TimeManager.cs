using System;
using UnityEngine;

namespace GDD
{
    public class TimeManager : Singleton<TimeManager>
    {
        private GameManager GM;
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
        
        public int getGameTimeMinute
        {
            get => date_Time.Minute;
        }

        public int getGameTimeHour
        {
            get => date_Time.Hour;
        }

        public int getDayTotal
        {
            get => day - 1;
        }
        
        private void OnEnable()
        {
            GM = GameManager.Instance;
        }

        private void Start()
        {
            date_Time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        }

        private void Update()
        {
            gameTimeSystem();
        }

        private void gameTimeSystem()
        {
            _deltaTime = Time.deltaTime * Mathf.Pow(100, _timescale);
            date_Time = date_Time.AddSeconds(_deltaTime);

            if ((date_Time.DayOfYear == 365 && !DateTime.IsLeapYear(date_Time.Year)) || date_Time.DayOfYear == 366)
            {
                if (!isSetTIme)
                {
                    isSetTIme = true;
                    oldyear = date_Time.Year;
                    totalDay += date_Time.DayOfYear;
                    day = date_Time.DayOfYear;
                }
            }
            else
            {
                isSetTIme = false;
                day = totalDay + date_Time.DayOfYear;
            }
            
            //print("Old Year : " + date_Time.Minute + " :: " + date_Time.DayOfYear);
        }
    }
}