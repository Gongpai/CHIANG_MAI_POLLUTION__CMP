using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GDD
{
    public class Spawner_Object_Grid : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSapwn;
        [SerializeField] private GameObject GameObjectLayer;
        [SerializeField] private GameObject Landscape;
        [SerializeField] private Material _highLightMaterial;
        [SerializeField] private Color activate_Color;
        [SerializeField] private Color deactivate_Color;

        private GameObject ObjectSpawn;
        private Vector3 ObjectSize;
        private Vector2 halfObjectSize;
        private Material _defaultMaterial;
        private Renderer _renderer;
        private RaycastHit box_Cast;
        private SetPositionShowGirdUseMouse setPositionShowGirdUseMouse;
        private int ObjectRotation = 0;
        private bool IsSelectObject = false;
        private List<string> m_ObjectData = new();
        GameObject old_objecthit = null;
        
        private int L_Landscape;
        private int L_Default;
        private int L_Building;
        private int L_Obstacle;
        private int L_Road;

        public Vector3 Place_Object_Size
        {
            get { return ObjectSize; }
            set { ObjectSize = value; }
        }

        public SetPositionShowGirdUseMouse SetPositionShowGirdUseMouse
        {
            get { return setPositionShowGirdUseMouse; }
        }

        public GameObject ObjectToSapwn
        {
            get { return objectToSapwn;}
            set
            {
                IsSelectObject = true;
                objectToSapwn = value;
                Destroy(ObjectSpawn);
                Snawner();
            }
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

        private void Awake()
        {
            setPositionShowGirdUseMouse = FindObjectOfType<SetPositionShowGirdUseMouse>();
            this.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            L_Landscape = LayerMask.NameToLayer("Landscape");
            L_Default = LayerMask.NameToLayer("Default");
            L_Building = LayerMask.NameToLayer("Place_Object");
            L_Obstacle = LayerMask.NameToLayer("Obstacle_Ojbect");
            L_Road = LayerMask.NameToLayer("Road_Object");

            if (GameObjectLayer == null)
            {
                GameObjectLayer = gameObject;
            }
            //Snawner();
        }
        // Update is called once per frame
        void Update()
        {
            var raycast_hit = CreateRaycast(RaycastFromMouse.GetRayFromMouse(), Color.blue, out bool hit_obj, out GameObject objectHit, out bool hit_floor);
                
            if (ObjectSpawn != null)
            {
                ObjectSpawn.transform.position = new Vector3(raycast_hit.Item3.x,
                    raycast_hit.Item1.point.y + (ObjectSize.y / 2), raycast_hit.Item3.y);
            }
            
            if (!PointerOverUIElement.OnPointerOverUIElement()&& hit_floor && IsSelectObject)
            {
                if (ObjectSpawn == null)
                {
                    Snawner();
                }
                Object_Place_System(raycast_hit, hit_obj);
            }
            else if (ObjectSpawn != null)
            {
                Destroy(ObjectSpawn);
            }
        }

        private void Object_Place_System(Tuple<RaycastHit, RaycastHit, Vector2, Vector3> raycast_hit, bool hit_Obj)
        {
            
            if (hit_Obj && raycast_hit.Item2.collider.gameObject == Landscape)
            {
                //print(" A : " + (raycast_hit.Item2.collider.gameObject == Landscape) + " | B : " + hit_Obj);
                //print("Place Mode");
                _renderer.material.SetColor("_ColorHighLight", activate_Color);
                if (Input.GetMouseButtonUp(0) && _renderer.material.GetColor("_ColorHighLight") == activate_Color)
                {
                    _renderer.material = _defaultMaterial;
                    ObjectSpawn.GetComponent<Collider>().enabled = true;

                    if (halfObjectSize.x > 0.5f)
                    {
                        GameObject Child_SpawnObject = new GameObject("Obstacle");
                        Child_SpawnObject.transform.parent = ObjectSpawn.transform;
                        Child_SpawnObject.transform.localPosition = Vector3.zero;
                        Child_SpawnObject.layer = L_Obstacle;
                        Child_SpawnObject.AddComponent<BoxCollider>();
                        BoxCollider childboxCollider = Child_SpawnObject.GetComponent<BoxCollider>();
                        childboxCollider.size = new Vector3(0.5f, 1, ((float)(Math.Floor(halfObjectSize.y) / 2) - halfObjectSize.x) * -1);
                        print("H Size : " + halfObjectSize.x);
                    }

                    Snawner();
                }
            }
            else if (raycast_hit.Item2.collider != null &&
                     raycast_hit.Item2.collider.gameObject != Landscape)
            {
                _renderer.material.SetColor("_ColorHighLight", deactivate_Color);
                //print((raycast_hit.Item2.collider.transform.parent == gameObject));
                if (Input.GetMouseButtonUp(1) && raycast_hit.Item2.collider.transform.parent == GameObjectLayer.transform)
                {
                    //print("Remove");
                    Destroy(raycast_hit.Item2.collider.gameObject);
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                Rotation_Place_Object();
            }
        }

        private void Rotation_Place_Object()
        {
            Vector3 vector_q = ObjectSpawn.transform.rotation.eulerAngles + new Vector3(0, 90, 0);
            ObjectSpawn.transform.rotation = Quaternion.Euler(vector_q);

            Quaternion q = Quaternion.Euler(vector_q).normalized;
            //Debug.LogWarning("Q : " + q + " | Axis : " + q.eulerAngles + " | Bool : " + (q.eulerAngles.y == 0 || q.eulerAngles.y == 180));

            //print("X : " + halfObjectSize.x + " | Y : " + halfObjectSize.y);
            if (halfObjectSize.x != halfObjectSize.y)
            {
                //print("IsCanRotation : " + (halfObjectSize.x != halfObjectSize.y));
                if (((int)q.eulerAngles.y == 0 || (int)q.eulerAngles.y == 180))
                {
                    ObjectRotation = 0;
                }
                else
                {
                    ObjectRotation = 1;
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            /*
            if (ObjectSpawn != null)
            {
                Gizmos.DrawCube(ObjectSpawn.transform.position, new Vector3(ObjectSize, ObjectSize, ObjectSize));
                Gizmos.DrawWireCube(box_Cast.point, new Vector3(ObjectSize  - 0.1f, ObjectSize - 0.1f, ObjectSize - 0.1f));
                Gizmos.DrawLine(box_Cast.point, box_Cast.normal);
            }
            */
        }

        public void ClearObjectSpawn()
        {
            if (ObjectSpawn != null)
            {
                Destroy(ObjectSpawn);
            }
        }

        public Tuple<RaycastHit, RaycastHit, Vector2, Vector3> CreateRaycast(Ray ray, Color color, out bool hit_obj, out GameObject object_hit, out bool hit_floor)
        {
            hit_floor = Physics.Raycast(ray, out var hit_floorraycasthit, 1000f, 1<<L_Landscape|0<<L_Default|0<<L_Building|0<<L_Obstacle|0<<L_Road);
            var hit1 = hit_floorraycasthit;

            Vector3 snapPosV3 = new Vector3(GridSnap(hit_floorraycasthit.point, default, true).x, 0,
                GridSnap(hit_floorraycasthit.point, default, true).y);
            Vector2 snapPosV2 = GridSnap(hit_floorraycasthit.point, default, true);
            Debug.DrawLine( snapPosV3 + (Vector3.up * 10), snapPosV3 + (Vector3.down * 10), color);
            
            Ray rayfloor = new Ray(snapPosV3 + (Vector3.up * 100), Vector3.down);
            hit_obj = Physics.Raycast(rayfloor, out var hit_objraycast);

            if (hit_objraycast.transform.parent == GameObjectLayer)
                object_hit = hit_objraycast.transform.gameObject;
            
            var hit2 = hit_objraycast;
            object_hit = null;

            return new Tuple<RaycastHit, RaycastHit, Vector2, Vector3>(hit1, hit2, snapPosV2, snapPosV3);
        }

        public Vector2 GridSnap(Vector3 point, Vector2 offset = default, bool useObjectSizeoffset = false)
        {
            Vector2 snapPos = new Vector2();

            //print("A : " + (halfObjectSize.y - 0.5f) + " B : " + (halfObjectSize.y * ObjectRotation) + " C : " + ObjectRotation);
            if (useObjectSizeoffset)
                offset = new Vector2((halfObjectSize.x + (halfObjectSize.y * ObjectRotation) * -(ObjectRotation)), halfObjectSize.y + (halfObjectSize.y * ObjectRotation));
            
            if (point.x >= 0)
            {
                snapPos.x = (int)point.x + offset.x;
                //print("snap int x : " + snapPos.x + " Offset x " + Mathf.FloorToInt(ObjectSize.x) + " result x : " + offset.x);
                if ((int)point.x > (setPositionShowGirdUseMouse.LandscapeSize.x / 2) - Mathf.FloorToInt(ObjectSize.x))
                {
                    //print("X Yessssssssssssssssssssssss");
                    snapPos.x -= Mathf.FloorToInt(offset.x);
                }
                
                //print("snap int x : " + ((int)point.x - Mathf.FloorToInt(ObjectSize.x)) + " Offset x " + Mathf.FloorToInt(ObjectSize.x) + " result x : " + snapPos.x);
            }
            else
            {
                snapPos.x = Mathf.FloorToInt(point.x) + offset.x;
                /*
                if ((int)point.x < (setPositionShowGirdUseMouse.LandscapeSize.x / 2) + Mathf.CeilToInt(ObjectSize.x))
                {
                    //print("X Yessssssssssssssssssssssss");
                    snapPos.x += Mathf.CeilToInt(offset.x);
                }
                */
            }
            
            if (point.z >= 0)
            {
                if (halfObjectSize.x == halfObjectSize.y || ObjectRotation < 1)
                {
                    snapPos.y = (int)point.z + offset.y;
                    //print("snap int y : " + ((int)point.z + offset.y) + " Offset z " + Mathf.FloorToInt(ObjectSize.z) + " result y : " + (snapPos.y - Mathf.FloorToInt(offset.y)));
                    //print("snap int y : " + ((setPositionShowGirdUseMouse.MeshSize.z / 2) - Mathf.FloorToInt(ObjectSize.z)) + " Offset y : " + Mathf.FloorToInt(ObjectSize.z) + " result y : " + offset.y);
                    if ((int)point.z > (setPositionShowGirdUseMouse.LandscapeSize.z / 2) -
                        Mathf.FloorToInt(ObjectSize.z))
                    {
                            snapPos.y -= Mathf.FloorToInt(offset.y);
                    }
                }
                else
                {
                    snapPos.y = (int)point.z + offset.y;
                    
                    //print("snap int y : " + ((setPositionShowGirdUseMouse.MeshSize.z / 2) - Mathf.FloorToInt(ObjectSize.z)) + " Offset y : " + Mathf.FloorToInt(ObjectSize.z) + " result y : " + offset.y);
                    if ((int)point.z > (setPositionShowGirdUseMouse.LandscapeSize.z / 2) - Mathf.FloorToInt(ObjectSize.x))
                    {
                        snapPos.y -= Mathf.FloorToInt(offset.x * 2);

                    }
                    //print("Snap pos : " + snapPos.y);
                }
                //print("snap int y : " + ((int)point.z - Mathf.FloorToInt(ObjectSize.y)) + " Offset y : " + Mathf.FloorToInt(ObjectSize.y) + " result y : " + snapPos.y);
            }
            else
            {
                snapPos.y = Mathf.FloorToInt(point.z) + offset.y;
                //print("snap int y : " + snapPos.y + " Offset y : " + offset.y + " result y : " + point.z);
                
            }

            return snapPos;
        }

        public void Snawner()
        {
            GameObject Old_ObjectSpawn = ObjectSpawn;
            
            if (Old_ObjectSpawn != null)
            {
                Outliner old_outliner = Old_ObjectSpawn.AddComponent<Outliner>();
                old_outliner.OutlineWidth = 1.05f;
                old_outliner.enabled = false;
                Old_ObjectSpawn.GetComponent<Building_System_Script>().name = objectData[0];
                Old_ObjectSpawn.GetComponent<Building_System_Script>().path = objectData[1];
                Old_ObjectSpawn.GetComponent<Building_System_Script>().OnPlaceBuilding();   
            }
            
            ObjectSpawn = Instantiate(objectToSapwn, GameObjectLayer.transform);
            //print(ObjectSpawn.name);
            ObjectSpawn.GetComponent<Collider>().enabled = false;
            ObjectSize = vector_one_decimal(ObjectSpawn.GetComponent<Renderer>().bounds.size);
            print("Object Size : " + ObjectSize);
            halfObjectSize = new Vector2(ObjectSize.x / 2, ObjectSize.z / 2);
            _renderer = ObjectSpawn.GetComponent<Renderer>();
            _defaultMaterial = _renderer.material;
            _renderer.material = _highLightMaterial;

            if (Old_ObjectSpawn == null || Old_ObjectSpawn.GetComponent<Building_System_Script>()._buildingType !=
                ObjectToSapwn.GetComponent<Building_System_Script>()._buildingType)
            {
                ObjectRotation = 0;
            }
            else
            {
                ObjectSpawn.transform.rotation = Old_ObjectSpawn.transform.rotation;
            }
                
        }

        public Vector3 vector_one_decimal(Vector3 vector_decimal)
        {
            float x = Mathf.Floor(vector_decimal.x * 10f);
            float y = Mathf.Floor(vector_decimal.y * 10f);
            float z = Mathf.Floor(vector_decimal.z * 10f);

            x /= 10f;
            y /= 10f;
            z /= 10f;

            return new Vector3(x, y, z);
        }

        public void SpawnerWithLoadScene(BuildingSaveData buildingSaveData, GameObject buildingObject)
        {
            if (GameObjectLayer == null)
            {
                GameObjectLayer = gameObject;
            }
            
            GameObject spawn = Instantiate(buildingObject, GameObjectLayer.transform);
            Outliner old_outliner = spawn.AddComponent<Outliner>();
            old_outliner.OutlineWidth = 1.05f;
            old_outliner.enabled = false;
            Building_System_Script buildingSystemScript = spawn.GetComponent<Building_System_Script>();
            
            //print(buildingSaveData.Position.X + " | " + buildingSaveData.Position.Y + " | " + buildingSaveData.Position.Z);
            buildingSystemScript.buildingSaveData = buildingSaveData;
            buildingSystemScript.OnGameLoad();
        }

        public Vector2 SizeObjectForGrid(Vector2 size)
        {
            return new Vector2((float)Math.Ceiling(size.x), (float)Math.Ceiling(size.y));
        }
    }
}
