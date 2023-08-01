using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDD
{
    public class Spawner_Road_Grid : MonoBehaviour
    {
        [SerializeField] private Vector2 Endlocation;
        private SetPositionShowGirdUseMouse _setPositionShowGirdUseMouse;
        private float timer = 0;
        [SerializeField] private Vector2 StartLocation = new Vector2();
        [SerializeField] private GameObject Start_X;
        [SerializeField] private GameObject Start_Y;
        
        private int L_Default;
        private int L_Building;
        private int L_Obstacle;

        List<RaycastHit> hitdata = new List<RaycastHit>();

        void Start()
        {
            L_Default = LayerMask.NameToLayer("Default");
            L_Building = LayerMask.NameToLayer("Place_Object");
            L_Obstacle = LayerMask.NameToLayer("Obstacle_Ojbect");
            _setPositionShowGirdUseMouse = FindObjectOfType<SetPositionShowGirdUseMouse>();
            Renderer r = _setPositionShowGirdUseMouse.GetComponent<Renderer>();

            hitdata.Add(new RaycastHit());
            hitdata.Add(new RaycastHit());
            /*
            print("RUN    " + gridX + gridY);
            
            for (int x = (int)gridX; x > 0; x--)
            {
                print(x);
                for (int y = (int)gridY; y > 0; y--)
                {
                    print(y);
                    GameObject tt = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tt.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    float floor = _setPositionShowGirdUseMouse.gameObject.transform.position.y;
                    Vector3 vv = new Vector3(((gridX / 2) - gridX) + (x - 0f), floor, ((gridY / 2) - gridY) + (y - 0f));
                    tt.gameObject.transform.position = vv;
                }
            }
            */
        }

        private void Update()
        {
            Ray Camray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Physics.Raycast(Camray, out var raycastHit, 100f, 1<<L_Default|0<<L_Building|0<<L_Obstacle);
            Endlocation =  new Vector2((int)raycastHit.point.x, (int)raycastHit.point.z);
            /*
            Debug.DrawLine(new Vector3(OldCurrentLocation.x, 0 , OldCurrentLocation.y) + (Vector3.up * 5), new Vector3(OldCurrentLocation.x, 0 , OldCurrentLocation.y), Color.red);

            gameObject.transform.position = new Vector3(OldCurrentLocation.x, 0, OldCurrentLocation.y);
            gameObject.transform.LookAt(new Vector3(currentLocation.x, 0 , currentLocation.y), Vector3.up);
            
            Debug.DrawLine(gameObject.transform.position + (Vector3.up * 5) + gameObject.transform.right, gameObject.transform.position + gameObject.transform.right, Color.blue);
            Debug.DrawLine(gameObject.transform.position + (Vector3.up * 5) + (gameObject.transform.right * -1), gameObject.transform.position + (gameObject.transform.right * -1), Color.blue);
            Debug.DrawLine(new Vector3(currentLocation.x, 0 , currentLocation.y) + (Vector3.up * 5), new Vector3(currentLocation.x, 0 , currentLocation.y), Color.blue);
            */

            /*
            if (timer >= 1)
            {
                print("Rot : " + gameObject.transform.rotation);
                //Generate_Road((int)location.x, (int)location.y);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
            */
            
            //var raycastHit_Y = Physics.Raycast(Vector3.zero, Vector3.back, out var hit_Y, 100f, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
            //hitdata[0] = hit_Y;
            foreach (var hit in hitdata)
            {
                Debug.DrawLine(hit.normal, hit.point, Color.yellow);
            }
            

            RayCast_RoadLine_Checker();
        }

        private void RayCast_RoadLine_Checker()
        {
            float x_width = Math.Abs(StartLocation.x + Endlocation.x);
            float y_width = Math.Abs(StartLocation.y + Endlocation.y);

            //print("X : " + x_width + " Y : " + y_width);
            if (x_width > y_width)
            {
                Vector3 StartPosX = new Vector3(StartLocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosX = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 StartPosY = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, StartLocation.y);
                Vector3 EndPosY = new Vector3(Endlocation.x, _setPositionShowGirdUseMouse.transform.position.y + 0.5f, Endlocation.y);
                //print("A");
                Debug.DrawLine(StartPosX, EndPosX, Color.red);
                Debug.DrawLine(StartPosY, EndPosY, Color.green);
                
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

    /*
    private void Generate_Road(int LocationX, int LocationY)
    {
        bool is_turn_x = false;
        bool is_turn_y = false;
        
        OldCurrentLocation = currentLocation;
        print("X : " + currentLocation.x + " Y : " + currentLocation.y);
        
        GameObject tt = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tt.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        float floor = _setPositionShowGirdUseMouse.gameObject.transform.position.y;
        Vector3 vv = new Vector3(currentLocation.x, floor, currentLocation.y);
        tt.gameObject.transform.position = vv;
        
        var raycastHit_R = Physics.Raycast(gameObject.transform.position + (Vector3.up * 100) + (gameObject.transform.right / 2f) + gameObject.transform.forward,
            Vector3.down, out RaycastHit hit_L, 100f, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
        var raycastHit_L = Physics.Raycast(gameObject.transform.position + (Vector3.up * 100) + ((gameObject.transform.right / 2f) * -1) + gameObject.transform.forward,
            Vector3.down, out var hit_R, 100f, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
        var raycastHit_F = Physics.Raycast(new Vector3(currentLocation.x, 0 , currentLocation.y) + (Vector3.up * 100) + gameObject.transform.forward,
            Vector3.down, out var hit_F, 100f, 0<<L_Default|0<<L_Building|1<<L_Obstacle);
        
        hitdata[0] = hit_L;
        hitdata[1] = hit_R;
        hitdata[2] = hit_F;
        
        print("Ray R : " + raycastHit_R + " Ray L " + raycastHit_L + " Ray F " + raycastHit_F);

        if (raycastHit_F && LocationX > LocationY && currentLocation.x < LocationX)
        {
            is_turn_x = true;
        }
        if (raycastHit_F && LocationX < LocationY && currentLocation.y < LocationY)
        {
            is_turn_y = true;
        }
        
        if (LocationX > LocationY || is_turn_x)
        {
            if (currentLocation.x > LocationX)
                currentLocation.x--;
            else if (currentLocation.x < LocationX)
                currentLocation.x++;
        }
        else
        {
            if (currentLocation.y > LocationY)
                currentLocation.y--;
            else if (currentLocation.y < LocationY)
                currentLocation.y++;
        }
    }
    */
}
