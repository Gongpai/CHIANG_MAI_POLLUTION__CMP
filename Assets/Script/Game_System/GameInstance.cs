using System;
using System.Collections.Generic;
using System.Linq;

namespace GDD
{
    public class GameInstance
    {
        public List<BuildingSaveData> buildingSystemScript = new List<BuildingSaveData>();
        public List<RoadSaveData> roadSystemScripts = new List<RoadSaveData>();
        public GameDateTime gameDateTime;

        public bool IsObjectEmpty()
        {
            bool isEmpty = false;
            isEmpty = buildingSystemScript.Count <= 0 && roadSystemScripts.Count <= 0;

            return isEmpty;
        }

        public DateTime getSaveGameDateTime
        {
            get
            {
                if (gameDateTime != null)
                {
                    return new DateTime(gameDateTime.year, gameDateTime.month, gameDateTime.day,
                        gameDateTime.hour, gameDateTime.minute, gameDateTime.second, gameDateTime.millisecond);
                }
                else
                {
                    return new DateTime(1970, 1, 1, 0, 0, 0, 0);
                }
            }
        }
    }

    public class GameDateTime
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;
        public int millisecond;

        public GameDateTime(int Year, int Month, int Day, int Hour, int Minute, int Second, int Millisecond)
        {
            year = Year;
            month = Month;
            day = Day;
            hour = Hour;
            minute = Minute;
            second = Second;
            millisecond = Millisecond;
        }
    }

    public class Vector3D
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
    
    public class Vector2D
    {
        public float X;
        public float Y;

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}