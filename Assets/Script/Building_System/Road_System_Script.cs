using System;
using UnityEngine;

namespace GDD
{
    public abstract class Road_System_Script : MonoBehaviour, IConstruction_System
    {
        private RoadSaveData _roadSaveData = new RoadSaveData();
        protected GameManager GM;

        public RoadSaveData roadSaveData
        {
            get => _roadSaveData;
            set => _roadSaveData = value;
        }
        
        public string name
        {
            get
            {
                if(_roadSaveData == null)
                    _roadSaveData = new RoadSaveData();
                
                return _roadSaveData.name;
            }
            set
            {
                if(_roadSaveData == null)
                    _roadSaveData = new RoadSaveData();
                
                _roadSaveData.name = value;
            }
        }

        public string path
        {
            get
            {
                if(_roadSaveData == null)
                    _roadSaveData = new RoadSaveData();
                
                return _roadSaveData.path;
            }
            set
            {
                if(_roadSaveData == null)
                    _roadSaveData = new RoadSaveData();
                
                _roadSaveData.path = value;
            }
        }

        public void OnGameLoad()
        {
            GM = GameManager.Instance;
            OnBeginPlace();
            OnEndPlace();
        }

        public void OnPlaceRoad(Vector2 StartPos, Vector2 EndPos)
        {
            GM = GameManager.Instance;
            OnBeginPlace();
            _roadSaveData.Start_Position = new Vector2D(StartPos.x, StartPos.y);
            _roadSaveData.End_Position = new Vector2D(EndPos.x, EndPos.y);
            OnEndPlace();
            GM.gameInstance.RoadSaveDatas.Add(_roadSaveData);
        }

        public abstract void Resource_usage();
        public abstract void Power_usage();

        public abstract void BeginStart();
        public abstract void EndStart();

        public abstract void OnEnableBuilding();
        public abstract void OnDisableBuilding();

        public abstract void OnBeginPlace();
        public abstract void OnEndPlace();
        public abstract void OnBeginRemove();
        public abstract void OnEndRemove();

        private void OnDestroy()
        {
            OnDestroyBuilding();
            
            GM.gameInstance.RoadSaveDatas.Remove(roadSaveData);
        }

        public abstract void OnDestroyBuilding();
    }
}