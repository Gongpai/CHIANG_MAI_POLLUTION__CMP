using System.Collections.Generic;
using System.Linq;

namespace GDD
{
    public class GameInstance
    {
        public List<BuildingSaveData> buildingSystemScript = new List<BuildingSaveData>();
        public List<RoadSaveData> roadSystemScripts = new List<RoadSaveData>();

        public bool IsObjectEmpty()
        {
            bool isEmpty = false;
            isEmpty = buildingSystemScript.Count <= 0 && roadSystemScripts.Count <= 0;

            return isEmpty;
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