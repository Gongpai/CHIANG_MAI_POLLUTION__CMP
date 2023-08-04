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
        [SerializeField] private List<Vector3> positions = new List<Vector3>();
        private List<GameObject> roads;
        private GameObject Navigation_point;
        private GameObject NavPoint;

        Mesh mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
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
        
        public void GenerateRoad()
        {
            if (Navigation_point == null)
            {
                Navigation_point = new GameObject("Navigation point");
                Navigation_point.transform.parent = gameObject.transform;
            }

            roads = new List<GameObject>();
            LineElement _element = new LineElement();
            
            for (int i = 0; i < positions.Count - 1; i++)
            {
                _element.Start = positions[i];
                _element.End = positions[i + 1];
                
                GameObject road_element = new GameObject("Road Element " + i);
                road_element.transform.parent = gameObject.transform;
                road_element.transform.position = _element.Start;
                road_element.AddComponent<MeshFilter>().mesh = mesh;
                road_element.AddComponent<MeshRenderer>().sharedMaterial = _material;
                road_element.AddComponent<MeshCollider>();
                
                NavPoint = new GameObject("Nav : " + road_element.name);
                SetTransformRoad(road_element, _element.Start, _element.End);
                
                roads.Add(road_element);
            }
        }

        public void PlaceRoad()
        {
            roads = new List<GameObject>();
        }

        public void ClearRoad()
        {
            foreach (var road in roads)
            {
                Destroy(road);
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
                SetTransformRoad(roads[i], _element.Start, _element.End);
            }
        }
        
        private void SetTransformRoad(GameObject road, Vector3 startPos, Vector3 endPos)
        {
            NavPoint.transform.parent = Navigation_point.transform;
            NavPoint.transform.position = endPos;
            
            road.transform.LookAt(NavPoint.transform);
            
            float distance = Mathf.Abs(Vector3.Distance(startPos, endPos));
            Vector3 r_forward = road.transform.forward;
            road.transform.position += r_forward * (distance / 2);
            road.transform.localScale = new Vector3(1, 1, distance);
        }
    }
}