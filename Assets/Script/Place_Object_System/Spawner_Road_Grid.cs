using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDD
{
    public class Spawner_Road_Grid : MonoBehaviour
    {
        [SerializeField] private Mesh StartPointMesh;
        [SerializeField] private Mesh EndPointMesh;
        [SerializeField] private Mesh m_RemoveMesh;
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
        private Mesh m_roadMesh;
        private RoadLineMesh _roadLineMesh;
        private bool IsNewRoad = false;
        private RoadMode _roadMode;
        private LineElement m_road_line_1 = new LineElement();
        private LineElement m_road_line_2 = new LineElement();
        
        private int L_Landscape;
        private int L_Default;
        private int L_Building;
        private int L_Obstacle;
        private int L_Road;
        private int L_Remove;

        private bool IsSelectObject = false;
        private bool IsPlaceRoad = false;
        private bool canPlaceRoad = false;
        private bool canCreateNewRoad = true;
        private float landscapePos;
        private List<string> m_ObjectData = new();
        private LineElement debug_1 = new LineElement();
        private LineElement debug_2 = new LineElement();

        List<RaycastHit> hitdata = new List<RaycastHit>();
        
        public Mesh RoadMesh
        {
            get { return m_roadMesh; }
            set
            {
                IsSelectObject = true;
                m_roadMesh = value;
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
        
        public List<string> objectData
        {
            get { return m_ObjectData; }
            set { m_ObjectData = value; }
        }

        public RoadMode roadMode
        {
            get => _roadMode;
            set
            {
                if (value == RoadMode.Remove)
                {
                    m_roadMesh = m_RemoveMesh;
                }
                _roadMode = value;
            }
        }

        private void Awake()
        {
            //this.enabled = false;
            //gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            print("On Disable");
            _roadLineMesh.ClearRoad();
            
            if(start_point != null && end_point != null)
                ShowMarker(false);
        }

        void Start()
        {
            _setPositionShowGirdUseMouse = FindObjectOfType<SetPositionShowGirdUseMouse>();
            L_Landscape = LayerMask.NameToLayer("Landscape");
            L_Default = LayerMask.NameToLayer("Default");
            L_Building = LayerMask.NameToLayer("Place_Object");
            L_Obstacle = LayerMask.NameToLayer("Obstacle_Ojbect");
            L_Road = LayerMask.NameToLayer("Road_Object");
            L_Remove = LayerMask.NameToLayer("Remove");
            Renderer r = _setPositionShowGirdUseMouse.GetComponent<Renderer>();
            _materialCheck.SetColor("_ColorHighLight", Color.green);
            _roadLineMesh = GetComponent<RoadLineMesh>();

            if (m_roadMesh != null)
                _roadLineMesh.mesh = m_roadMesh;
            
            _roadLineMesh.material_Check = _materialCheck;
            CreateMarkPoint();
            ShowMarker(true);
            
            hitdata.Add(new RaycastHit());
            hitdata.Add(new RaycastHit());
            hitdata.Add(new RaycastHit());
        }

        private void Update()
        {
            Ray Camray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Physics.Raycast(Camray, out var raycastHit, 100f, 1<<L_Landscape|0<<L_Default|0<<L_Building|0<<L_Obstacle|0<<L_Road|0<<L_Remove);
            Debug.DrawLine(raycastHit.point + new Vector3(0, 10, 0), raycastHit.point, Color.red);

            //Debug Line
            Debug.DrawLine(debug_1.Start, debug_1.End, Color.magenta);
            Debug.DrawLine(debug_2.Start, debug_2.End, Color.cyan);
            
            /*
            if (raycastHit.transform != null)
            {
                print("Cam Ray : " + raycastHit.transform.name);
            }
            else
            {
                print("Cam Ray : nulllllllllllllllllll");
            }
            */

            Vector2 mousePosGrid = new Vector2((int)raycastHit.point.x, (int)raycastHit.point.z);
            Endlocation = mousePosGrid;
            landscapePos = _setPositionShowGirdUseMouse.gameObject.transform.position.y;
            end_point.transform.position = (new Vector3(Endlocation.x, landscapePos, Endlocation.y));
            
            foreach (var hit in hitdata)
            {
                Debug.DrawLine(hit.normal, hit.point, Color.yellow);
            }
            
            if (!PointerOverUIElement.OnPointerOverUIElement())
            {
                ShowMarker(true, false);
                if (Input.GetMouseButtonUp(0) && (canCreateNewRoad || _roadMode == RoadMode.Remove))
                {
                    ShowMarker(true);
                    canPlaceRoad = true;
                    
                    IsNewRoad = !IsNewRoad;
                    if (IsNewRoad)
                    {
                        if(!_roadLineMesh.enabled)
                            _roadLineMesh.enabled = true;
                        
                        if(_roadMode == RoadMode.Place)
                        {
                            _roadLineMesh.Hight_Light_Alpha = 0.4f;
                            _roadLineMesh.GenerateRoad(m_roadMesh, L_Road);
                        }
                        else
                        {
                            _roadLineMesh.Hight_Light_Alpha = 1f;
                            _roadLineMesh.GenerateRoad(m_roadMesh, L_Remove);
                        }

                        StartLocation = mousePosGrid;
                    }
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    IsNewRoad = false;
                    canCreateNewRoad = true;
                    IsPlaceRoad = false;
                    _roadLineMesh.ClearRoad();
                }

                if (IsNewRoad)
                {
                    start_point.SetActive(true);
                    IsPlaceRoad = true;
                    
                    if (_roadMode == RoadMode.Place)
                    {
                        RayCast_RoadLine_Checker(0 << L_Default|0 << L_Building|0 << L_Road|1<< L_Obstacle, Color.green, Color.red);
                    }
                    else
                    {
                        RayCast_RoadLine_Checker(0<<L_Default|0<<L_Building|0<<L_Obstacle|0<<L_Remove|1<<L_Road, Color.gray, Color.yellow);
                    }
                }
                else if (canPlaceRoad)
                {
                    start_point.SetActive(false);

                    if (IsPlaceRoad)
                    {
                        IsPlaceRoad = false;
                        if (_roadMode == RoadMode.Place)
                        {
                            _roadLineMesh.PlaceRoad();
                        }
                        else
                        {
                            _roadLineMesh.ClearRoad();
                            Road_Remover();
                        }
                    }
                }
            }
            else
            {
                _roadLineMesh.ClearRoad();
                canPlaceRoad = false;
                IsNewRoad = false;
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
        
        public void SpawnerWithLoadScene(RoadSaveData roadSaveData, GameObject roadObject)
        {
            _roadLineMesh = GetComponent<RoadLineMesh>();
            _roadLineMesh.enabled = true;
            
            if (_roadLineMesh.RoadLayer == null)
                _roadLineMesh.RoadLayer = GameObject.FindGameObjectWithTag("Road_Layer");

            if (_setPositionShowGirdUseMouse == null)
                _setPositionShowGirdUseMouse = FindObjectOfType<SetPositionShowGirdUseMouse>();

            Mesh roadMesh = roadObject.GetComponent<MeshFilter>().sharedMesh;
            landscapePos = _setPositionShowGirdUseMouse.gameObject.transform.position.y;
            Vector3 StartPos = new Vector3(roadSaveData.Start_Position.X, landscapePos, roadSaveData.Start_Position.Y);
            Vector3 EndPos = new Vector3(roadSaveData.End_Position.X, landscapePos, roadSaveData.End_Position.Y);
            
            L_Road = LayerMask.NameToLayer("Road_Object");
            GameObject spawn = _roadLineMesh.GenerateRoad(roadMesh, L_Road,  StartPos, EndPos);
            spawn.GetComponent<Renderer>().sharedMaterial = _roadLineMesh.defaultMaterial;
            Road_System_Script roadSystemScript = spawn.GetComponent<Road_System_Script>();
            
            //print(buildingSaveData.Position.X + " | " + buildingSaveData.Position.Y + " | " + buildingSaveData.Position.Z);
            roadSystemScript.roadSaveData = roadSaveData;
            roadSystemScript.OnGameLoad();
        }

        private List<Vector3> Get_Start_End_Point(bool isStartX)
        {
            List<Vector3> Road_point = new List<Vector3>();

            if (isStartX)
            {
                Road_point.Add(new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f,StartLocation.y));
                Road_point.Add(new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f,StartLocation.y));
                Road_point.Add(new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f,StartLocation.y));
                Road_point.Add(new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f,Endlocation.y));
            }
            else
            {
                Road_point.Add(new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y));
                Road_point.Add(new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y));
                Road_point.Add(new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y));
                Road_point.Add(new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y));
            }

            return Road_point;
        }

        private void Road_Remover()
        {
            Road_System_Script roadSystemScript;
            Vector2 pointstart, pointend;
            RaycastHit roadHit_1;
            RaycastHit roadHit_2 = hitdata[1];
            Vector3 roadpos_1, roadpos_2;

            if (hitdata[0].transform == hitdata[2].transform || hitdata[2].transform == null)
            {
                roadHit_1 = hitdata[0];
            }
            else if(hitdata[0].transform == hitdata[1].transform || hitdata[0].transform == null || hitdata[0].transform != hitdata[2].transform)
            {
                roadHit_1 = hitdata[2];
            }
            else
            {
                roadHit_1 = hitdata[0];
            }

            roadpos_1 = _roadLineMesh.Positions[0];
            roadpos_2 = _roadLineMesh.Positions[0];
            
            if (roadHit_1.transform != null)
            {
                roadSystemScript = roadHit_1.transform.GetComponent<Road_System_Script>();
                pointstart = new Vector2(roadSystemScript.roadSaveData.Start_Position.X, roadSystemScript.roadSaveData.Start_Position.Y);
                pointend = new Vector2(roadSystemScript.roadSaveData.End_Position.X, roadSystemScript.roadSaveData.End_Position.Y);
                //print("Road 1 Point Start : " + pointstart + " | Road 1 Point End : " + pointend);
                //print("(1) Start Across Point : " + hitdata[0].point);
                //print("(1) End Across Point : " + Endlocation);

                if (roadHit_2.transform != null && hitdata[2].transform == null)
                {
                    print("HIIIITTT TRUE");
                    OnSeparate_Road(roadSystemScript, m_road_line_1,
                        m_road_line_2);
                    debug_1.Start = m_road_line_1.Start;
                    debug_1.End = m_road_line_1.End;
                }
                else
                {
                    print("HIIIITTT FALSE");
                    OnSeparate_Road(roadSystemScript, m_road_line_2, 
                        m_road_line_1);
                    debug_1.Start = m_road_line_2.Start;
                    debug_1.End = m_road_line_2.End;
                }

                
            }

            if (roadHit_2.transform != null && (m_road_line_1.End != m_road_line_2.End))
            {
                roadSystemScript = roadHit_2.transform.GetComponent<Road_System_Script>();
                pointstart = new Vector2(roadSystemScript.roadSaveData.Start_Position.X, roadSystemScript.roadSaveData.Start_Position.Y);
                pointend = new Vector2(roadSystemScript.roadSaveData.End_Position.X, roadSystemScript.roadSaveData.End_Position.Y);
                //print("Road 2 Point Start : " + pointstart + " | Road 2 Point End : " + pointend);
                print("(2) Start Across Point : " + hitdata[1].point);
                print("(2) End Across Point : " + Endlocation);
                OnSeparate_Road(roadSystemScript, m_road_line_2, m_road_line_1);
                debug_2.Start = m_road_line_2.Start;
                debug_2.End = m_road_line_2.End;
            }
        }
        
        private void OnSeparate_Road(Road_System_Script roadSystemScript, LineElement line1, LineElement line2)
        {
            Vector2 start_roadData;
            Vector2 end_roadData;
            List<LineElement> _elements = new List<LineElement>();
            Vector2 start_cross_point;
            Vector2 end_cross_point;
            Vector2 start_cross_point_line2;
            Vector2 end_cross_point_line2;
            
            //print("Road Name : " + roadSystemScript.transform.name);
            
            Sort_Least_to_Greatest_Vector2(new Vector2(roadSystemScript.roadSaveData.Start_Position.X, roadSystemScript.roadSaveData.Start_Position.Y), 
                new Vector2(roadSystemScript.roadSaveData.End_Position.X, roadSystemScript.roadSaveData.End_Position.Y),
                out start_roadData, out end_roadData, true);
            
            Debug.LogWarning("Start X : " + roadSystemScript.roadSaveData.Start_Position.X + " Y : " + roadSystemScript.roadSaveData.Start_Position.Y);
            Debug.LogWarning("End X : " + roadSystemScript.roadSaveData.End_Position.X + " Y : " + roadSystemScript.roadSaveData.End_Position.Y);

            //Debug.LogWarning("start_roadData x : " + start_roadData.x + " | start x : " + start.x);
            Sort_Least_to_Greatest_Vector2(new Vector2(line1.Start.x, line1.Start.z), new Vector2(line1.End.x, line1.End.z), out start_cross_point, out end_cross_point, true);
            
            print("Start Cross : " + start_cross_point + " | End Cross : " + end_cross_point);
            if (start_roadData.x != end_roadData.x)
            {
                bool isDestroySeparated = false;
                bool isDestroyDistanceZero = false;
                //print("Road 1 Distance : " + (int)Mathf.Abs(start_cross_point.x- start_roadData.x));
                //print("Road 2 Distance : " + (int)Mathf.Abs(end_cross_point.x - end_roadData.x) + " | A : " + end_cross_point.x + " | B : " + end_roadData.x);
                
                //Road 1 Start/End
                RoadCutting(ref _elements, out bool isCut_start, true, true, roadSystemScript, start_cross_point, start_roadData, "X_1");
                isDestroySeparated = isCut_start;
                isDestroyDistanceZero = !isCut_start;

                //Road 2 Start/End
                RoadCutting(ref _elements, out bool isCut_end, true, false, roadSystemScript, end_cross_point, end_roadData, "X_2");
                isDestroySeparated = isCut_end;
                isDestroyDistanceZero = !isCut_end;

                if (isDestroyDistanceZero && roadSystemScript.transform.gameObject != null || isDestroySeparated)
                {
                    print("Fucking Deadddddddddddddddddddd");
                    Destroy(roadSystemScript.transform.parent.gameObject);
                }
            }

            Sort_Least_to_Greatest_Vector2(new Vector2(roadSystemScript.roadSaveData.Start_Position.X, roadSystemScript.roadSaveData.Start_Position.Y), 
                new Vector2(roadSystemScript.roadSaveData.End_Position.X, roadSystemScript.roadSaveData.End_Position.Y),
                out start_roadData, out end_roadData, false);
            Sort_Least_to_Greatest_Vector2(new Vector2(line2.Start.x, line2.Start.z), new Vector2(line2.End.x, line2.End.z), out start_cross_point, out end_cross_point, false);
            print("Start Cross : " + start_cross_point + " | End Cross : " + end_cross_point);
            if (start_roadData.y != end_roadData.y)
            {
                bool isDestroySeparated = false;
                bool isDestroyDistanceZero = false;
                //print("Road 1 Distance : " + (int)Mathf.Abs(start_cross_point.x- start_roadData.x));
                //print("Road 2 Distance : " + (int)Mathf.Abs(end_cross_point.x - end_roadData.x) + " | A : " + end_cross_point.x + " | B : " + end_roadData.x);
                
                //Road 1 Start/End
                RoadCutting(ref _elements, out bool isCut_start, false, true, roadSystemScript, start_cross_point, start_roadData, "Y_1");
                isDestroySeparated = isCut_start;
                isDestroyDistanceZero = !isCut_start;

                //Road 2 Start/End
                RoadCutting(ref _elements, out bool isCut_end, false, false, roadSystemScript, end_cross_point, end_roadData, "Y_2");
                isDestroySeparated = isCut_end;
                isDestroyDistanceZero = !isCut_end;

                if (isDestroyDistanceZero && roadSystemScript.transform.gameObject != null || isDestroySeparated)
                {
                    print("Fucking Deadddddddddddddddddddd");
                    Destroy(roadSystemScript.transform.parent.gameObject);
                }
            }
        }

        private void RoadCutting(ref List<LineElement> _elements, out bool isCut, bool isXAxis, bool isFirst, Road_System_Script roadSystemScript, Vector2 cross_point, Vector2 roadData, string debug = "")
        {
            float cross_point_check;
            float road_data_check;
            float start_road_data;
            float end_road_data;
            if (isXAxis)
            {
                cross_point_check = cross_point.x;
                road_data_check = roadData.x;
            }
            else
            {
                cross_point_check = cross_point.y;
                road_data_check = roadData.y;
            }
            
            if ((int)Mathf.Abs(cross_point_check - road_data_check) != 0)
            {
                isCut = true;
                _elements.Add(new LineElement());
                AddLineElements(ref _elements, new Vector2(roadData.x, roadData.y),
                    new Vector2(cross_point.x, cross_point.y));

                Vector3 element_start = _elements[_elements.Count - 1].Start;
                Vector3 element_end = _elements[_elements.Count - 1].End;
                print("New Road " + debug + " Start At : " + element_start + " | End At : " + element_end);

                if (isXAxis)
                {
                    start_road_data = element_start.x;
                    end_road_data = element_start.x;
                    if (cross_point_check >= start_road_data && isFirst)
                    {
                        CreateNewRoadFromSeparated(roadSystemScript, element_start, element_end);
                    }
                    else if(cross_point_check <= end_road_data && !isFirst)
                    {
                        CreateNewRoadFromSeparated(roadSystemScript, element_start, element_end);
                    }
                }
                else
                {
                    start_road_data = element_start.z;
                    end_road_data = element_start.z;
                    if (cross_point_check >= start_road_data && isFirst)
                    {
                        CreateNewRoadFromSeparated(roadSystemScript, element_start, element_end);
                    }
                    else if(cross_point_check <= end_road_data && !isFirst)
                    {
                        CreateNewRoadFromSeparated(roadSystemScript, element_start, element_end);
                    }
                }
                
                if (isFirst)
                {
                    print("1 Road " + debug + " Cross At : " + cross_point_check + " | End At : " + start_road_data);
                }
                else
                {
                    print("2 Road " + debug + " Cross At : " + cross_point_check + " | End At : " + end_road_data);
                }
            }
            else
            {
                isCut = false;
            }
        }

        private void CreateNewRoadFromSeparated(Road_System_Script roadSystemScript, Vector3 start, Vector3 end)
        {
            Mesh roadMesh = roadSystemScript.GetComponent<MeshFilter>().sharedMesh;
            //Component road_component = roadSystemScript.GetComponent(roadSystemScript.GetType());
            
            GameObject spawn = _roadLineMesh.GenerateRoad(roadMesh, L_Road,  start, end);
            spawn.GetComponent<Renderer>().sharedMaterial = _roadLineMesh.defaultMaterial;
            //spawn.AddComponent(road_component.GetType());
            spawn.GetComponent<Road_System_Script>().OnPlaceRoad(new Vector2(start.x, start.z), new Vector2(end.x, end.z));
        }

        private void Sort_Least_to_Greatest_Vector2(Vector2 a, Vector2 b, out Vector2 out_a, out Vector2 out_b, bool isAxit_x)
        {
            if (isAxit_x)
            {
                if (a.x < b.x)
                {
                    out_a = a;
                    out_b = b;
                    //print("TRUE : Out_A : " + out_a + " | Out_B : " + out_b);
                }
                else
                {
                    out_a = b;
                    out_b = a;
                    
                    //out_a = a;
                    //out_b = b;
                    //print("FALSE : Out_A : " + out_a + " | Out_B : " + out_b);
                }
            }
            else
            {
                if (a.y < b.y)
                {
                    out_a = a;
                    out_b = b;
                    print("Y TRUE : Out_A : " + out_a + " | Out_B : " + out_b);
                }
                else
                {
                    out_a = b;
                    out_b = a;
                    print("Y FALSE : Out_A : " + out_a + " | Out_B : " + out_b);
                }
            }
        }

        private void AddLineElements(ref List<LineElement> _elements, Vector2 start, Vector2 end)
        {
            _elements[_elements.Count - 1].Start = new Vector3(start.x, landscapePos, start.y);
            _elements[_elements.Count - 1].End = new Vector3(end.x, landscapePos, end.y);
        }
        
        private void RayCast_RoadLine_Checker(LayerMask layerMask, Color activate, Color deactivate, float offset_direction = -0.25f)
        {
            float x_width = Math.Abs(StartLocation.x + Endlocation.x);
            float y_width = Math.Abs(StartLocation.y + Endlocation.y);
            
            List<Vector3> pos = new List<Vector3>();
            
            if (x_width > y_width)
            {
                Vector3 StartPosX = Get_Start_End_Point(true)[0];
                Vector3 EndPosX = Get_Start_End_Point(true)[1];
                Vector3 StartPosY = Get_Start_End_Point(true)[2];
                Vector3 EndPosY = Get_Start_End_Point(true)[3];
                //print("A");
                //road
                pos.Add(new Vector3(StartPosX.x, landscapePos, StartPosX.z));
                pos.Add(new Vector3(EndPosX.x, landscapePos, EndPosX.z));
                pos.Add(new Vector3(EndPosY.x, landscapePos, EndPosY.z));
                _roadLineMesh.Positions = pos;
                _roadLineMesh.UpdatePositionRoad();

                float x_distance = Mathf.Abs(Vector3.Distance(StartPosX, EndPosX));
                Start_X.transform.position = StartPosX;
                Start_X.transform.LookAt(EndPosX, Vector3.up);
                Vector3 PointX_offset = Start_X.transform.position + Start_X.transform.forward * offset_direction;

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

                //Hit Before Start
                Vector3 point_hit_before = start_point.transform.position + (start_point.transform.forward * 0.2f) + (start_point.transform.right * (Mathf.Abs(offset_direction) + 0.1f)) + new Vector3(0, 0.4f, 0);
                Physics.Raycast(point_hit_before, start_point.transform.right * -1f, out RaycastHit hit_before, Mathf.Abs(offset_direction) + 0.1f, layerMask);
                if(hit_before.transform != null)
                    hitdata[2] = hit_before;
                
                //Hit Start
                var raycastHit_X = Physics.Raycast(PointX_offset, Start_X.transform.forward, out RaycastHit hit_X, x_distance + (offset_direction * -1), layerMask);
                hitdata[0] = hit_X;
                
                float y_distance = Mathf.Abs(Vector3.Distance(StartPosY, EndPosY));
                Start_Y.transform.position = StartPosY;
                Start_Y.transform.LookAt(EndPosY, Vector3.up);
                Vector3 PointY_offset = Start_Y.transform.position + Start_Y.transform.forward * offset_direction;

                //Hit End
                var raycastHit_Y = Physics.Raycast(PointY_offset, Start_Y.transform.forward, out var hit_Y, y_distance + (offset_direction * -1), layerMask);
                hitdata[1] = hit_Y;

                if (raycastHit_X || raycastHit_Y)
                {
                    _roadLineMesh.UpdateColorMaterial(deactivate);
                }
                else
                {
                    _roadLineMesh.UpdateColorMaterial(activate);
                }
                
                m_road_line_1.Start = Start_Y.transform.position;
                m_road_line_1.End = EndPosY;
                m_road_line_2.Start = Start_X.transform.position;
                m_road_line_2.End = EndPosX;
                
                Debug.DrawLine(point_hit_before, hitdata[2].point, Color.blue);
                Debug.DrawLine(PointX_offset, EndPosX, Color.red);
                Debug.DrawLine(PointY_offset, EndPosY, Color.green);

                canCreateNewRoad = !(raycastHit_X || raycastHit_Y);
                //print("Hit X : " + raycastHit_X + " Hit Y : " + raycastHit_Y + " Axis X : " + Start_X.transform.forward + " Axis X : " + Start_Y.transform.forward);
            }
            else
            {
                Vector3 StartPosX = Get_Start_End_Point(false)[0];
                Vector3 EndPosX = Get_Start_End_Point(false)[1];
                Vector3 StartPosY = Get_Start_End_Point(false)[2];
                Vector3 EndPosY = Get_Start_End_Point(false)[3];
                //print("B");
                //road
                pos.Add(new Vector3(StartPosY.x, landscapePos, StartPosY.z));
                pos.Add(new Vector3(EndPosY.x, landscapePos, EndPosY.z));
                pos.Add(new Vector3(EndPosX.x, landscapePos, EndPosX.z));
                _roadLineMesh.Positions = pos;
                _roadLineMesh.UpdatePositionRoad();
                
                float y_distance = Mathf.Abs(Vector3.Distance(StartPosY, EndPosY));
                Start_Y.transform.position = StartPosY;
                Start_Y.transform.LookAt(EndPosY, Vector3.up);
                Vector3 PointY_offset = Start_Y.transform.position + Start_Y.transform.forward * offset_direction;
                
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

                //Hit Before Start
                Vector3 point_hit_before = start_point.transform.position + (start_point.transform.forward * 0.2f) + (start_point.transform.right * (Mathf.Abs(offset_direction) + 0.1f)) + new Vector3(0, 0.4f, 0);
                Physics.Raycast(point_hit_before, start_point.transform.right * -1f, out RaycastHit hit_before, Mathf.Abs(offset_direction) + 0.1f, layerMask);
                if(hit_before.transform != null)
                    hitdata[2] = hit_before;
                
                //Hit Start
                var raycastHit_Y = Physics.Raycast(PointY_offset, Start_Y.transform.forward, out RaycastHit hit_Y, y_distance + (offset_direction * -1), layerMask);
                hitdata[0] = hit_Y;
                
                float x_distance = Mathf.Abs(Vector3.Distance(StartPosX, EndPosX));
                Start_X.transform.position = StartPosX;
                Start_X.transform.LookAt(EndPosX, Vector3.up);
                Vector3 PointX_offset =  Start_X.transform.position + Start_X.transform.forward * offset_direction;
                
                //Hit End
                var raycastHit_X = Physics.Raycast(PointX_offset, Start_X.transform.forward, out var hit_X, x_distance + (offset_direction * -1), layerMask);
                hitdata[1] = hit_X;
                
                if (raycastHit_X || raycastHit_Y)
                {
                    _roadLineMesh.UpdateColorMaterial(deactivate);
                }
                else
                {
                    _roadLineMesh.UpdateColorMaterial(activate);
                }
                
                m_road_line_1.Start = Start_Y.transform.position;
                m_road_line_1.End = EndPosY;
                m_road_line_2.Start = Start_X.transform.position;
                m_road_line_2.End = EndPosX;
                
                Debug.DrawLine(point_hit_before, hitdata[2].point, Color.blue);
                Debug.DrawLine(PointY_offset, EndPosY, Color.red);
                Debug.DrawLine(PointX_offset, EndPosX, Color.green);
                
                canCreateNewRoad = !(raycastHit_X || raycastHit_Y);
                //print("Hit Y : " + raycastHit_Y + " Hit X : " + raycastHit_X + " Axis X : " + Start_X.transform.forward + " Axis X : " + Start_Y.transform.forward);
            }
        }
    }
}
