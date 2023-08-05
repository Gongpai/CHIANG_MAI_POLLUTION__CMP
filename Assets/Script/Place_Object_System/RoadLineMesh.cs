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
        [SerializeField] private GameObject Road_Layer;
        [SerializeField] private List<Vector3> positions = new List<Vector3>();
        private List<GameObject> roads;
        private GameObject Navigation_point;
        private List<GameObject> NavPoint;
        private Material _material_check;

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

        public List<Vector3> Positions
        {
            get { return positions; }
            set { positions = value; }
        }
        // Start is called before the first frame update
        void Start()
        {

            //GenerateRoad();

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void GenerateRoad(Mesh meshRoad = null)
        {
            if (meshRoad != null)
            {
                _mesh = meshRoad;
            }
            
            if (Navigation_point == null)
            {
                Navigation_point = new GameObject("Navigation point");
                Navigation_point.transform.parent = Road_Layer.transform;
            }

            roads = new List<GameObject>();
            LineElement _element = new LineElement();
            
            for (int i = 0; i < positions.Count - 1; i++)
            {
                _element.Start = positions[i];
                _element.End = positions[i + 1];
                
                GameObject road_element = new GameObject("Road Element " + i);
                road_element.transform.parent = Road_Layer.transform;
                road_element.transform.position = _element.Start;
                road_element.layer = LayerMask.NameToLayer("Road_Ojbect");
                road_element.AddComponent<MeshFilter>().mesh = mesh;
                road_element.AddComponent<MeshRenderer>().sharedMaterial = _material_check;
                road_element.AddComponent<BoxCollider>();
                
                SetTransformRoad(road_element, i, _element.Start, _element.End);
                roads.Add(road_element);
            }
        }

        public void PlaceRoad()
        {
            if (roads != null && roads.Count > 0)
            {
                foreach (var road in roads)
                {
                    road.GetComponent<Renderer>().sharedMaterial = _material;
                }
            }

            roads = new List<GameObject>();
        }

        public void ClearRoad()
        {
            if (roads != null && roads.Count > 0)
            {
                foreach (var road in roads)
                {
                    Destroy(road);
                }
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
            LineElement _element = new LineElement();
            
            for (int i = 0; i < positions.Count - 1; i++)
            {
                _element.Start = positions[i];
                _element.End = positions[i + 1];
                
                roads[i].transform.position = _element.Start;
                SetTransformRoad(roads[i], i, _element.Start, _element.End);
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

            NavPoint[index].transform.parent = Navigation_point.transform;
            NavPoint[index].transform.position = endPos;
            
            road.transform.LookAt(NavPoint[index].transform);
            
            float distance = Mathf.Abs(Vector3.Distance(startPos, endPos));
            Vector3 r_forward = road.transform.forward;
            road.transform.position += r_forward * (distance / 2);
            road.transform.localScale = new Vector3(1, 1, distance + 0.2f);
        }
    }
}