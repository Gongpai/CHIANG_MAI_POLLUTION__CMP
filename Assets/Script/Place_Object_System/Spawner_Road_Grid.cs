using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDD
{
    public class Spawner_Road_Grid : MonoBehaviour
    {
        [SerializeField] private Mesh StartPointMesh;
        [SerializeField] private Mesh EndPointMesh;
        [SerializeField] private Material _materialCheck;
        [SerializeField] private Material _materialPoint;
        [SerializeField] private Vector2 StartLocation = new Vector2();
        [SerializeField] private Vector2 Endlocation = new Vector2();
        private SetPositionShowGirdUseMouse _setPositionShowGirdUseMouse;
        private float timer = 0;
        [SerializeField] private GameObject Start_X;
        [SerializeField] private GameObject Start_Y;
        private GameObject start_point;
        private GameObject end_point;
        private Mesh roadMesh;
        private RoadLineMesh _roadLineMesh;
        private bool IsPlaceRoad = false;
        
        private int L_Default;
        private int L_Building;
        private int L_Obstacle;

        private bool IsSelectObject = false;
        private bool IsSpawnRoad = false;
        private bool canPlaceRoad = true;
        private float landscapePos;

        List<RaycastHit> hitdata = new List<RaycastHit>();
        
        public Mesh RoadMesh
        {
            get { return roadMesh; }
            set
            {
                IsSelectObject = true;
                roadMesh = value;
            }
        }

        public RoadLineMesh Road_LineMesh
        {
            get { return _roadLineMesh; }
            set { _roadLineMesh = value; }
        }
        
        public bool isSelectObject
        {
            get { return IsSelectObject; }
            set { IsSelectObject = value; }
        }

        private void Awake()
        {
            //this.enabled = false;
            //gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _roadLineMesh.ClearRoad();
        }

        void Start()
        {
            _setPositionShowGirdUseMouse = FindObjectOfType<SetPositionShowGirdUseMouse>();
            L_Default = LayerMask.NameToLayer("Default");
            L_Building = LayerMask.NameToLayer("Place_Object");
            L_Obstacle = LayerMask.NameToLayer("Obstacle_Ojbect");
            Renderer r = _setPositionShowGirdUseMouse.GetComponent<Renderer>();
            _materialCheck.SetColor("_ColorHighLight", Color.green);
            _roadLineMesh = GetComponent<RoadLineMesh>();

            if (roadMesh != null)
                _roadLineMesh.mesh = roadMesh;
            
            _roadLineMesh.material_Check = _materialCheck;
            CreateMarkPoint();
            ShowMarker(true);
            
            hitdata.Add(new RaycastHit());
            hitdata.Add(new RaycastHit());
        }

        private void Update()
        {
            Ray Camray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Physics.Raycast(Camray, out var raycastHit, 100f, 1<<L_Default|0<<L_Building|0<<L_Obstacle);
            Vector2 mousePosGrid = new Vector2((int)raycastHit.point.x, (int)raycastHit.point.z);
            Endlocation = mousePosGrid;
            landscapePos = _setPositionShowGirdUseMouse.gameObject.transform.position.y;
            end_point.transform.position = (new Vector3(Endlocation.x, landscapePos, Endlocation.y));
            
            foreach (var hit in hitdata)
            {
                Debug.DrawLine(hit.normal, hit.point, Color.yellow);
            }

            if (!IsPointerOverUIElement())
            {
                ShowMarker(true, false);
                if (Input.GetMouseButtonUp(0) && canPlaceRoad)
                {
                    ShowMarker(true);
                    IsSpawnRoad = true;
                    
                    IsPlaceRoad = !IsPlaceRoad;
                    if (IsPlaceRoad)
                    {
                        if(!_roadLineMesh.enabled)
                            _roadLineMesh.enabled = true;
                        
                        _roadLineMesh.GenerateRoad(roadMesh);
                        StartLocation = mousePosGrid;
                    }
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    IsPlaceRoad = false;
                    _roadLineMesh.ClearRoad();
                }
            
                if (IsPlaceRoad)
                {
                    start_point.SetActive(true);
                    RayCast_RoadLine_Checker();
                }
                else if (IsSpawnRoad)
                {
                    start_point.SetActive(false);
                    _roadLineMesh.PlaceRoad();
                }
            }
            else
            {
                _roadLineMesh.ClearRoad();
                IsSpawnRoad = false;
                IsPlaceRoad = false;
                ShowMarker(false);
            }
        }
        
        private void CreateMarkPoint()
        {
            start_point = new GameObject("Start Point Mark");
            start_point.transform.parent = gameObject.transform;
            start_point.AddComponent<MeshFilter>().mesh = StartPointMesh;
            start_point.AddComponent<MeshRenderer>().sharedMaterial = _materialPoint;
            start_point.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", Color.yellow);
            
            end_point = new GameObject("End Point Mark");
            end_point.transform.parent = gameObject.transform;
            end_point.AddComponent<MeshFilter>().mesh = EndPointMesh;
            end_point.AddComponent<MeshRenderer>().sharedMaterial = _materialPoint;
            end_point.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_ColorHighLight", Color.yellow);
            
            ShowMarker(true, false);
        }
        
        public void ShowMarker(bool isHide, bool HideStartPoint = true)
        {
            if(HideStartPoint)
                start_point.SetActive(isHide);
            
            end_point.SetActive(isHide);
        }
        
        public bool IsPointerOverUIElement()
        {
            int UILayer = LayerMask.NameToLayer("UI");
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            
            for (int index = 0; index < raysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = raysastResults[index];
                //print(curRaysastResult.gameObject.layer + " : " + UILayer);
                if (curRaysastResult.gameObject.layer == UILayer)
                {
                    //print("Detect UI !!!!!!!!!!!!!");
                    return true;
                }
            }
            //print("Not Detect UI !!!!!!!!!!!!!");
            return false;
        }

        private void RayCast_RoadLine_Checker()
        {
            float x_width = Math.Abs(StartLocation.x + Endlocation.x);
            float y_width = Math.Abs(StartLocation.y + Endlocation.y);
            
            List<Vector3> pos = new List<Vector3>();
            
            if (x_width > y_width)
            {
                Vector3 StartPosX = new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosX = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 StartPosY = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosY = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y);
                //print("A");
                Debug.DrawLine(StartPosX, EndPosX, Color.red);
                Debug.DrawLine(StartPosY, EndPosY, Color.green);
                
                //road
                pos.Add(new Vector3(StartPosX.x, landscapePos, StartPosX.z));
                pos.Add(new Vector3(EndPosX.x, landscapePos, EndPosX.z));
                pos.Add(new Vector3(EndPosY.x, landscapePos, EndPosY.z));
                _roadLineMesh.Positions = pos;
                _roadLineMesh.UpdatePositionRoad();

                float x_distance = Mathf.Abs(Vector3.Distance(StartPosX, EndPosX));
                Start_X.transform.position = StartPosX;
                Start_X.transform.LookAt(EndPosX, Vector3.up);

                //point
                start_point.transform.position = new Vector3(Start_X.transform.position.x, landscapePos, Start_X.transform.position.z);
                if (EndPosY != StartPosY && StartPosX == EndPosX)
                {
                    start_point.transform.LookAt(new Vector3(EndPosY.x, landscapePos, EndPosY.z), Vector3.up);
                }
                else
                {
                    start_point.transform.LookAt(new Vector3(EndPosX.x, landscapePos, EndPosX.z), Vector3.up);
                }

                var raycastHit_X = Physics.Raycast(StartPosX, Start_X.transform.forward, out RaycastHit hit_X, x_distance, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[0] = hit_X;
                
                float y_distance = Mathf.Abs(Vector3.Distance(StartPosY, EndPosY));
                Start_Y.transform.position = StartPosY;
                Start_Y.transform.LookAt(EndPosY, Vector3.up);

                var raycastHit_Y = Physics.Raycast(StartPosY, Start_Y.transform.forward, out var hit_Y, y_distance, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[1] = hit_Y;

                if (raycastHit_X || raycastHit_Y)
                {
                    _roadLineMesh.UpdateColorMaterial(Color.red);
                }
                else
                {
                    _roadLineMesh.UpdateColorMaterial(Color.green);
                }

                canPlaceRoad = !(raycastHit_X || raycastHit_Y);
                //print("Hit X : " + raycastHit_X + " Hit Y : " + raycastHit_Y + " Axis X : " + Start_X.transform.forward + " Axis X : " + Start_Y.transform.forward);
            }
            else
            {
                Vector3 StartPosX = new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y);
                Vector3 EndPosX = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y);
                Vector3 StartPosY = new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosY = new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y);
                
                //print("B");
                Debug.DrawLine(StartPosY, EndPosY, Color.red);
                Debug.DrawLine(StartPosX, EndPosX, Color.green);
                
                //road
                pos.Add(new Vector3(StartPosY.x, landscapePos, StartPosY.z));
                pos.Add(new Vector3(EndPosY.x, landscapePos, EndPosY.z));
                pos.Add(new Vector3(EndPosX.x, landscapePos, EndPosX.z));
                _roadLineMesh.Positions = pos;
                _roadLineMesh.UpdatePositionRoad();
                
                float y_distance = Mathf.Abs(Vector3.Distance(StartPosY, EndPosY));
                Start_Y.transform.position = StartPosY;
                Start_Y.transform.LookAt(EndPosY, Vector3.up);
                
                //point
                start_point.transform.position = new Vector3(Start_Y.transform.position.x, landscapePos, Start_Y.transform.position.z);
                if (EndPosX != StartPosX && StartPosY == EndPosY)
                {
                    start_point.transform.LookAt(new Vector3(EndPosX.x, landscapePos, EndPosX.z), Vector3.up);
                }
                else
                {
                    start_point.transform.LookAt(new Vector3(EndPosY.x, landscapePos, EndPosY.z), Vector3.up);
                }

                var raycastHit_Y = Physics.Raycast(StartPosY, Start_Y.transform.forward, out RaycastHit hit_Y, y_distance, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[0] = hit_Y;
                
                float x_distance = Mathf.Abs(Vector3.Distance(StartPosX, EndPosX));
                Start_X.transform.position = StartPosX;
                Start_X.transform.LookAt(EndPosX, Vector3.up);
                
                var raycastHit_X = Physics.Raycast(StartPosX, Start_X.transform.forward, out var hit_X, x_distance, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[1] = hit_X;
                
                if (raycastHit_X || raycastHit_Y)
                {
                    _roadLineMesh.UpdateColorMaterial(Color.red);
                }
                else
                {
                    _roadLineMesh.UpdateColorMaterial(Color.green);
                }
                
                canPlaceRoad = !(raycastHit_X || raycastHit_Y);
                //print("Hit Y : " + raycastHit_Y + " Hit X : " + raycastHit_X + " Axis X : " + Start_X.transform.forward + " Axis X : " + Start_Y.transform.forward);
            }
        }
    }
}
