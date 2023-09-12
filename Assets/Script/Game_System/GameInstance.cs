using System;
using System.Collections.Generic;
using System.Linq;

namespace GDD
{
    public class GameInstance
    {
        public List<BuildingSaveData> buildingSaveDatas = new List<BuildingSaveData>();
        public List<RoadSaveData> RoadSaveDatas = new List<RoadSaveData>();
        public Resources_Data resources = new Resources_Data(0, 0, 0, 0);
        
        public GameDateTime gameDateTime;

        public bool IsObjectEmpty()
        {
            bool isEmpty = false;
            isEmpty = buildingSaveDatas.Count <= 0 && RoadSaveDatas.Count <= 0;
            
            return isEmpty;
        }

        public void set_rock_resource(int resource)
        {
             resources.rock = resource;
        }
        
        public int get_rock_resource()
        {
            return resources.rock;
        }
        
        public void set_tree_resource(int resource)
        {
            resources.tree = resource;
        }
        
        public int get_tree_resource()
        {
            return resources.tree;
        }
        
        public void set_food_resource(int resource)
        {
            resources.food = resource;
        }
        
        public int get_food_resource()
        {
            return resources.food;
        }
        
        public void set_power_resource(float resource)
        {
            resources.power = resource;
        }
        
        public float get_power_resource()
        {
            return resources.power;
        }

        public DateTime getSaveGameDateTime()
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

    public class Resources_Data
    {
        public int rock = 0;
        public int tree = 0;
        public int food = 0;
        public float power = 0;

        public Resources_Data(int _rock, int _tree, int _food, int _power)
        {
            rock = _rock;
            tree = _tree;
            food = _food;
            power = _power;
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