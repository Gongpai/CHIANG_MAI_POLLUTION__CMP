using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Spawner_Road_Grid : MonoBehaviour
    {
        [SerializeField] private GameObject RoadMesh;
        [SerializeField] private Vector2 StartLocation = new Vector2();
        [SerializeField] private Vector2 Endlocation = new Vector2();
        private SetPositionShowGirdUseMouse _setPositionShowGirdUseMouse;
        private float timer = 0;
        [SerializeField] private GameObject Start_X;
        [SerializeField] private GameObject Start_Y;
        private RoadLineMesh _roadLineMesh;
        private bool IsPlaceRoad = false;
        
        private int L_Default;
        private int L_Building;
        private int L_Obstacle;

        List<RaycastHit> hitdata = new List<RaycastHit>();

        void Start()
        {
            _setPositionShowGirdUseMouse = FindObjectOfType<SetPositionShowGirdUseMouse>();
            L_Default = LayerMask.NameToLayer("Default");
            L_Building = LayerMask.NameToLayer("Place_Object");
            L_Obstacle = LayerMask.NameToLayer("Obstacle_Ojbect");
            Renderer r = _setPositionShowGirdUseMouse.GetComponent<Renderer>();
            _roadLineMesh = GetComponent<RoadLineMesh>();

            hitdata.Add(new RaycastHit());
            hitdata.Add(new RaycastHit());
        }

        private void Update()
        {
            Ray Camray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Physics.Raycast(Camray, out var raycastHit, 100f, 1<<L_Default|0<<L_Building|0<<L_Obstacle);
            Vector2 mousePosGrid = new Vector2((int)raycastHit.point.x, (int)raycastHit.point.z);
            Endlocation = mousePosGrid;
            
            foreach (var hit in hitdata)
            {
                Debug.DrawLine(hit.normal, hit.point, Color.yellow);
            }

            if (Input.GetMouseButtonUp(0))
            {
                IsPlaceRoad = !IsPlaceRoad;
                if (IsPlaceRoad)
                {
                    if(!_roadLineMesh.enabled)
                        _roadLineMesh.enabled = true;
                    
                    StartLocation = mousePosGrid;
                    _roadLineMesh.GenerateRoad();
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                IsPlaceRoad = false;
                _roadLineMesh.ClearRoad();
            }
            
            if (IsPlaceRoad)
            {
                RayCast_RoadLine_Checker();
            }
            else
            {
                _roadLineMesh.PlaceRoad();
            }
        }

        private void RayCast_RoadLine_Checker()
        {
            float x_width = Math.Abs(StartLocation.x + Endlocation.x);
            float y_width = Math.Abs(StartLocation.y + Endlocation.y);
            List<Vector3> pos = new List<Vector3>();
            float landscapePos = _setPositionShowGirdUseMouse.gameObject.transform.position.y;
            
            if (x_width > y_width)
            {
                Vector3 StartPosX = new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosX = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 StartPosY = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosY = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y);
                //print("A");
                Debug.DrawLine(StartPosX, EndPosX, Color.red);
                Debug.DrawLine(StartPosY, EndPosY, Color.green);

                
                pos.Add(new Vector3(StartPosX.x, landscapePos, StartPosX.z));
                pos.Add(new Vector3(EndPosX.x, landscapePos, EndPosX.z));
                pos.Add(new Vector3(EndPosY.x, landscapePos, EndPosY.z));
                _roadLineMesh.Positions = pos;
                _roadLineMesh.UpdatePositionRoad();
                
                Start_X.transform.position = StartPosX;
                Start_X.transform.LookAt(EndPosX, Vector3.up);
                var raycastHit_X = Physics.Raycast(StartPosX, Start_X.transform.forward, out RaycastHit hit_X, x_width, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[0] = hit_X;
                
                Start_Y.transform.position = StartPosY;
                Start_Y.transform.LookAt(EndPosY, Vector3.up);
                var raycastHit_Y = Physics.Raycast(StartPosY, Start_Y.transform.forward, out var hit_Y, y_width, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[1] = hit_Y;
                
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
                
                pos.Add(new Vector3(StartPosY.x, landscapePos, StartPosY.z));
                pos.Add(new Vector3(EndPosY.x, landscapePos, EndPosY.z));
                pos.Add(new Vector3(EndPosX.x, landscapePos, EndPosX.z));
                _roadLineMesh.Positions = pos;
                _roadLineMesh.UpdatePositionRoad();
                
                Start_Y.transform.position = StartPosY;
                Start_Y.transform.LookAt(EndPosY, Vector3.up);
                var raycastHit_Y = Physics.Raycast(StartPosY, Start_Y.transform.forward, out RaycastHit hit_Y, y_width, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[0] = hit_Y;
                
                Start_X.transform.position = StartPosX;
                Start_X.transform.LookAt(EndPosX, Vector3.up);
                var raycastHit_X = Physics.Raycast(StartPosX, Start_X.transform.forward, out var hit_X, x_width, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
                hitdata[1] = hit_X;
                
                //print("Hit Y : " + raycastHit_Y + " Hit X : " + raycastHit_X + " Axis X : " + Start_X.transform.forward + " Axis X : " + Start_Y.transform.forward);
            }
        }
    }
}
