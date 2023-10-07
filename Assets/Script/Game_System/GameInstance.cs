using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GDD
{
    public class GameInstance
    {
        public List<BuildingSaveData> buildingSaveDatas = new List<BuildingSaveData>();
        public List<RoadSaveData> RoadSaveDatas = new List<RoadSaveData>();
        public List<Static_Resource_SaveData> staticResourceSaveDatas = new List<Static_Resource_SaveData>();
        public List<PeopleSystemSaveData> villagerSaveDatas = new List<PeopleSystemSaveData>();
        public List<PeopleSystemSaveData> workerSaveDatas = new List<PeopleSystemSaveData>();
        public Resources_Data resources = new Resources_Data(250, 250, 0, 0, 0, 2000);
        public Resources_Data max_resources = new Resources_Data(500, 500, 500, 500, 0, 100);
        public TechnologyUpgrade_DataSave TUDataSave = new TechnologyUpgrade_DataSave();
        public int pm_25;
        
        public GameDateTime gameDateTime;

        public bool IsObjectEmpty()
        {
            bool isEmpty = false;
            isEmpty = buildingSaveDatas.Count <= 0 && RoadSaveDatas.Count <= 0;
            
            return isEmpty;
        }

        public bool check_id_ResourceSaveData(int id)
        {
            bool is_check_id = false;
            Parallel.ForEach(staticResourceSaveDatas, resourceSaveData =>
            {
                if(resourceSaveData.id == id)
                    is_check_id = true;
            });

            return is_check_id;
        }

        public Static_Resource_SaveData Get_Static_Resource_SaveData(int id)
        {
            int i = 0;
            Parallel.ForEach(staticResourceSaveDatas, (resourceSaveData, state, index) =>
            {
                if (resourceSaveData.id == id)
                    i = (int)index;
            });

            return staticResourceSaveDatas[i];
        }

        public int get_villager_count()
        {
            return villagerSaveDatas.Count;
        }

        public int get_worker_count()
        {
            return workerSaveDatas.Count;
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
        
        public void set_raw_food_resource(int resource)
        {
            resources.raw_food = resource;
        }
        
        public int get_raw_food_resource()
        {
            return resources.raw_food;
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

        public void set_token_resource(int resource)
        {
            resources.token = resource;
        }
        
        public int get_token_resource()
        {
            return resources.token;
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
        public int raw_food = 0;
        public int food = 0;
        public float power = 0;
        public int token = 0;

        public Resources_Data(int _rock, int _tree, int _raw_food, int _food, float _power, int _token)
        {
            rock = _rock;
            tree = _tree;
            raw_food = _raw_food;
            food = _food;
            power = _power;
            token = _token;
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