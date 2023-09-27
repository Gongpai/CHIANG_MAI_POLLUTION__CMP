using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public abstract class Road_System_Script : MonoBehaviour, IConstruction_System
    {
        [SerializeField] private Road_Preset m_Preset;
        [SerializeField] private Material m_road_material;
        [SerializeField] private Material m_construction_Road_material;
        [SerializeField] private Material m_construction_progress_material;
        [SerializeField] private Material m_remove_progress_material;
        private RoadSaveData _roadSaveData = new RoadSaveData();
        protected TimeManager TM;
        protected GameManager GM;
        private ResourcesManager RM;
        private bool is_place_road;

        public bool is_remove_road
        {
            get => _roadSaveData.road_is_remove;
            set => _roadSaveData.road_is_remove = value;
        }
        
        public RoadSaveData roadSaveData
        {
            get => _roadSaveData;
            set => _roadSaveData = value;
        }

        public Material construction_Road_material
        {
            get => m_construction_Road_material;
            set => m_construction_Road_material = value;
        }

        public Material construction_Progress_Material
        {
            get => m_construction_progress_material;
            set => m_construction_progress_material = value;
        }

        public Material remove_progress_material
        {
            get => m_remove_progress_material;
            set => m_remove_progress_material = value;
        }

        public Road_Preset road_Preset
        {
            get => m_Preset;
            set => m_Preset = value;
        }

        public Material road_material
        {
            get => m_road_material;
            set => m_road_material = value;
        }
        
        protected float construction_progess_percent
        {
            get
            {
                if (roadSaveData == null || m_Preset == null)
                {
                    return 0;
                }
                else
                {
                    return (roadSaveData.construction_In_Progress / 3600) / m_Preset.time_construction;
                }
            } 
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

        private void Start()
        {
            TM = TimeManager.Instance;
            GM = GameManager.Instance;
            RM = ResourcesManager.Instance;
        }

        private void Update()
        {
            OnProgress();
        }

        public void OnGameLoad()
        {
            TM = TimeManager.Instance;
            GM = GameManager.Instance;
            RM = ResourcesManager.Instance;
            if (!_roadSaveData.is_complete)
            {
                is_place_road = true;
            }
            else
            {
                Material[] m_roads = new Material[1];
                m_roads[0] = m_road_material;
                GetComponent<Renderer>().sharedMaterials = m_roads;
            }

            OnBeginPlace();
            OnEndPlace();
        }

        public void OnPlaceRoad()
        {
            TM = TimeManager.Instance;
            GM = GameManager.Instance;
            RM = ResourcesManager.Instance;
            Renderer renderer = GetComponent<Renderer>();
            m_construction_Road_material = renderer.sharedMaterial;
            
            Material[] m_roads = new Material[2];
            m_roads[0] = renderer.sharedMaterial;
            m_roads[1] = m_construction_progress_material;
            GetComponent<Renderer>().sharedMaterials = m_roads;
            
            _roadSaveData.is_complete = false;
            is_place_road = true;
            is_remove_road = false;

            Resources_Build();
            OnBeginPlace();
            
            OnEndPlace();
            GM.gameInstance.RoadSaveDatas.Add(_roadSaveData);
        }

        public virtual void Resources_Build()
        {
            float road_length_origin = Mathf.Abs(_roadSaveData.Start_Position.X -_roadSaveData.End_Position.X);
            print("Road Data Origin : " + " X : " + _roadSaveData.Start_Position.X + " Y : " + _roadSaveData.Start_Position.Y);
            print("Road Lenght Origin : " + road_length_origin);
            float road_length_end = Mathf.Abs(_roadSaveData.Start_Position.Y - _roadSaveData.End_Position.Y);
            print("Road Data End : " + " X : " + _roadSaveData.End_Position.X + " Y : " + _roadSaveData.End_Position.Y);
            print("Road Lenght End : " + road_length_end);
            float road_length = Mathf.Abs(road_length_origin + road_length_end);
            print("Road Lenght Sum : " + road_length);
            
            RM.Set_Resources_Tree(-(int)road_length);
            RM.Set_Resources_Rock(-(int)road_length);
        }

        public virtual void OnProgress()
        {
            float progess = _roadSaveData.construction_In_Progress / 3600;
            if ((is_place_road && m_Preset.time_construction > progess && !_roadSaveData.is_complete) || (is_remove_road && progess >= 0))
            {
                if (is_remove_road)
                {
                    roadSaveData.construction_In_Progress += TM.deltaTime * -1;
                }
                else
                {
                    roadSaveData.construction_In_Progress += TM.deltaTime;
                }

                _roadSaveData.is_complete = false;
                switch_material_construction_progress(true);

                //print("Time Con : " + (roadSaveData.construction_In_Progress / 3600));
                //print("Construction_In_Progress : " + (int)((roadSaveData.construction_In_Progress / 3600) / m_Preset.time_construction * 100) + "%");
            }
            else
            {
                if (is_place_road && !is_remove_road)
                {
                    _roadSaveData.is_complete = true;
                    switch_material_construction_progress(false);
                }
            }
            
            if (construction_progess_percent <= 0 && is_remove_road)
            {
                Destroy(gameObject);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }

        protected void switch_material_construction_progress(bool is_progress)
        {
            if (is_progress)
            {
                Material[] m_roads = new Material[2];
                m_roads[0] = m_construction_Road_material;
                m_roads[1] = m_construction_progress_material;
                GetComponent<Renderer>().sharedMaterials = m_roads;
            }
            else
            {
                Material[] m_roads = new Material[1];
                m_roads[0] = road_material;
                GetComponent<Renderer>().sharedMaterials = m_roads;
            }
        }

        public void Resource_product()
        {
            
        }

        public abstract void Resource_usage();
        public abstract void Power_usage();
        
        public virtual void Check_Surround_Road()
        {
            
        }

        public abstract void BeginStart();
        public abstract void EndStart();

        public abstract void OnEnableBuilding();
        public abstract void OnDisableBuilding();

        public abstract void OnBeginPlace();
        public abstract void OnEndPlace();

        public virtual void OnRemoveRoad()
        {
            is_remove_road = true;
            is_place_road = true;
            m_construction_progress_material = m_remove_progress_material;
            OnBeginRemove();
            
            //System script
            //-----------------------------------
            
            //-----------------------------------
            
            OnEndRemove();
        }
        
        public abstract void OnBeginRemove();
        public abstract void OnEndRemove();

        private void OnDestroy()
        {
            OnDestroyBuilding();
            
            GM.gameInstance.RoadSaveDatas.Remove(roadSaveData);
        }

        public void OnRemovePeople<T>(People_System_Script _peopleSystemScript, PeopleJob job)
        {
            
        }

        public abstract void OnDestroyBuilding();
    }
}