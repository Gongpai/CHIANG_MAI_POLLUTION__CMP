using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class RoadLineMesh : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField] private Material m_construction_road_material;
        [SerializeField] private GameObject m_road_Layer;
        [SerializeField] private List<Vector3> positions = new List<Vector3>();

        private Spawner_Road_Grid _spawnerRoadGrid;
        private List<GameObject> roads;
        private GameObject Navigation_point;
        private List<GameObject> NavPoint;
        private Material _material_check;
        private List<LineElement> _element;

        public Mesh mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }
        
        public Material material_Check
        {
            get { return _material_check; }
            set { _material_check = value; }
        }

        public Material defaultMaterial
        {
            get => _material;
        }

        public List<Vector3> Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        public GameObject RoadLayer
        {
            get => m_road_Layer;
            set => m_road_Layer = value;
        }

        public float Hight_Light_Alpha
        {
            set
            {
                print("Alpha : " + value);
                _material_check.SetFloat("_HighLightAlpha", value);
            } 
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _spawnerRoadGrid = GetComponent<Spawner_Road_Grid>();
            //GenerateRoad();

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public GameObject GenerateRoad(Mesh meshRoad = null, LayerMask layerMask = default, Vector3 posStart = default, Vector3 posEnd = default)
        {
            if (meshRoad != null)
            {
                _mesh = meshRoad;
            }
            
            if (Navigation_point == null)
            {
                Navigation_point = new GameObject("Navigation point");
                Navigation_point.transform.parent = m_road_Layer.transform;
            }

            _element = new List<LineElement>();
            roads = new List<GameObject>();
            GameObject roadGroup = new GameObject("RoadGroup");
            roadGroup.transform.parent = m_road_Layer.transform;
            roadGroup.layer = LayerMask.NameToLayer("Road_Object");

            if (posStart == default || posEnd == default)
            {
                for (int i = 0; i < positions.Count - 1; i++)
                {
                    _element.Add(new LineElement());
                    _element[i].Start = positions[i];
                    _element[i].End = positions[i + 1];
                    
                    GameObject road_element = CreateRoad(("Road Element " + i), roadGroup, _element[i], layerMask);
                    SetTransformRoad(road_element, i, _element[i].Start, _element[i].End);
                    roads.Add(road_element);
                }
            }
            else
            {
                _element.Add(new LineElement());
                _element[0].Start = posStart;
                _element[0].End = posEnd;
                GameObject road_element = CreateRoad(("Road Element "), roadGroup, _element[0], layerMask, false);
                print("start : " + _element[0].Start + " end : " + _element[0].End);
                SetTransformRoad(road_element, 0, _element[0].Start, _element[0].End);
                return road_element;
            }

            return null;
        }

        private GameObject CreateRoad(string name, GameObject group, LineElement _element, LayerMask layerMask, bool isOnPlace = true)
        {
            GameObject road_element = new GameObject(name);
            road_element.transform.parent = group.transform;
            road_element.transform.position = _element.Start;
            road_element.layer = layerMask;
            road_element.AddComponent<MeshFilter>().mesh = mesh;
            road_element.AddComponent<MeshRenderer>().sharedMaterial = _material_check;
            road_element.AddComponent<BoxCollider>();
            road_element.AddComponent<Non_asphalt_Road_Script>();
            BoxCollider boxCollider = road_element.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(boxCollider.size.x, 1, boxCollider.size.z);
            boxCollider.isTrigger = true;

            if (isOnPlace)
            {
                Road_System_Script _roadSystemScript = road_element.GetComponent<Road_System_Script>();
                _roadSystemScript.roadSaveData.Start_Position = new Vector2D(_element.Start.x, _element.Start.z);
                _roadSystemScript.roadSaveData.End_Position = new Vector2D(_element.End.x, _element.End.z);
            }

            return road_element;
        }
        
        public void PlaceRoad()
        {
            if (roads != null && roads.Count > 0)
            {
                for (int i = 0; i < roads.Count; i++)
                {
                    Road_System_Script roadSystemScript = roads[i].GetComponent<Road_System_Script>();
                    Road_System_Script _roadSystemScript_prefab_obj = _spawnerRoadGrid.road_prefab.GetComponent<Road_System_Script>();
                    roadSystemScript.road_Preset = _roadSystemScript_prefab_obj.road_Preset;
                    roadSystemScript.road_material = _roadSystemScript_prefab_obj.road_material;
                    roadSystemScript.construction_Road_material = _roadSystemScript_prefab_obj.construction_Road_material;
                    roadSystemScript.construction_Progress_Material = _roadSystemScript_prefab_obj.construction_Progress_Material;
                    roadSystemScript.remove_progress_material = _roadSystemScript_prefab_obj.remove_progress_material;
                    
                    roads[i].GetComponent<Renderer>().sharedMaterial = _material;
                    print("Start : " + _element[i].Start);
                    print("End : " + _element[i].End);
                    roadSystemScript.roadSaveData.Start_Position = new Vector2D(_element[i].Start.x, _element[i].Start.z);
                    roadSystemScript.roadSaveData.End_Position = new Vector2D(_element[i].End.x, _element[i].End.z);
                    bool can_place = roadSystemScript.OnPlaceRoad();
                    roadSystemScript.roadSaveData.name = _spawnerRoadGrid.objectData[0];
                    roadSystemScript.roadSaveData.path = _spawnerRoadGrid.objectData[1];

                    if (!can_place)
                        ClearRoad();
                }
            }

            _element = new List<LineElement>();
            roads = new List<GameObject>();
        }

        public void ClearRoad()
        {
            if (roads != null && roads.Count > 0)
            {
                Destroy(roads[0].transform.parent.gameObject);
                roads = new List<GameObject>();
            }
        }
        
        public void UpdateColorMaterial(Color color)
        {
            if (roads != null && roads.Count > 0)
            {
                foreach (var road in roads)
                {
                    road.GetComponent<Renderer>().sharedMaterial.SetColor("_ColorHighLight", color);
                }
            }
        }

        public void UpdatePositionRoad()
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                _element[i].Start = positions[i];
                _element[i].End = positions[i + 1];
                
                roads[i].transform.position = _element[i].Start;
                SetTransformRoad(roads[i], i, _element[i].Start, _element[i].End);
            }
        }
        
        private void SetTransformRoad(GameObject road, int index, Vector3 startPos, Vector3 endPos)
        {
            if (NavPoint == null)
            {
                NavPoint = new List<GameObject>();
            }

            if ((NavPoint.Count - 1) < index)
            {
                GameObject nav_p = new GameObject("Nav : " + road.name);
                NavPoint.Add(nav_p);
            }
            //print("start : " + _element[0].Start + " end : " + _element[0].End);
            NavPoint[index].transform.parent = Navigation_point.transform;
            NavPoint[index].transform.position = endPos;
            
            road.transform.LookAt(NavPoint[index].transform);
            
            float distance = Mathf.Abs(Vector3.Distance(startPos, endPos));
            Vector3 r_forward = road.transform.forward;
            road.transform.position += r_forward * (distance / 2);
            road.transform.localScale = new Vector3(1, 1, distance + 0.2f);
            
            BoxCollider boxCollider = road.GetComponent<BoxCollider>();
            float colliderZsize = (distance - ((int)((boxCollider.size.x * 2) * 10f) / 10f)) / distance;
            //print("Coll Size : " + colliderZsize);
            if (colliderZsize <= 0)
            {
                colliderZsize = 0.1f;
            }
            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, colliderZsize);
        }
    }
}