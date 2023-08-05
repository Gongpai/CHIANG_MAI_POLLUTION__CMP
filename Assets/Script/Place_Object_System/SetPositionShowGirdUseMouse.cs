using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class SetPositionShowGirdUseMouse : MonoBehaviour
    {
        [SerializeField] private Material _materialGrid;
        [SerializeField] private Color activate_Color;
        [SerializeField] private Color deactivate_Color;

        private Material _defaultMaterial;
        private Material _material;
        private MeshRenderer _meshRenderer;
        private bool isShowGrid = true;

        private Vector3 _landscapeSize;
        private Spawner_Object_Grid _spawnerObjectGrid;

        private RaycastHit hit1;
        private RaycastHit hit2;

        public bool IsShowGrid
        {
            get { return isShowGrid; }
            set { isShowGrid = value; }
        }
        
        public Vector3 LandscapeSize
        {
            get { return _landscapeSize; }
            set { _landscapeSize = value; }
        }

        private void Awake()
        {
            this.enabled = false;
        }

        private void OnEnable()
        {
            _defaultMaterial = GetComponent<Renderer>().sharedMaterial;
            _materialGrid.SetColor("_HighLightGrid", Color.white);
            GetComponent<Renderer>().sharedMaterial = _materialGrid;
        }

        private void OnDisable()
        {
            _spawnerObjectGrid.isSelectObject = false;
            _spawnerObjectGrid.ClearObjectSpawn();
            GetComponent<Renderer>().sharedMaterial = _defaultMaterial;
        }

        // Start is called before the first frame update
        void Start()
        {
            _spawnerObjectGrid = FindObjectOfType<Spawner_Object_Grid>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _material = _meshRenderer.sharedMaterial;
            _landscapeSize = _meshRenderer.bounds.size;
            _material.SetVector("_Call_Size", new Vector4(_landscapeSize.x, _landscapeSize.x));
            _material.SetFloat("_CircleSize", 0.9f - ((_spawnerObjectGrid.SizeObjectForGrid(_spawnerObjectGrid.Place_Object_Size).x * 0.1f) / 4));
        }

        // Update is called once per frame
        void Update()
        {
            if (_spawnerObjectGrid.isSelectObject && isShowGrid)
            {
                Ray_to_Lanscape(_spawnerObjectGrid.GetRayFromMouse());
            }
            else if(!isShowGrid)
            {
                _materialGrid.SetColor("_HighLightGrid", Color.black);
            }
                
        }

        private void Ray_to_Lanscape(Ray ray)
        {
            var raycast_hit = _spawnerObjectGrid.CreateRaycast(ray, Color.red, out bool hit_obj, out bool hit_floor);

            if (hit_floor && !_spawnerObjectGrid.IsPointerOverUIElement())
            {
                if (hit_obj && raycast_hit.Item2.collider.gameObject == gameObject)
                {
                    _material.SetColor("_HighLightGrid", activate_Color);
                }
                else if (raycast_hit.Item2.collider != null && raycast_hit.Item2.collider.gameObject != gameObject)
                {
                    _material.SetColor("_HighLightGrid", deactivate_Color);
                }
            }
            else
            {
                _material.SetColor("_HighLightGrid", Color.white);
            }
            
            _material.SetFloat("_CircleSize", 0.9f - ((_spawnerObjectGrid.SizeObjectForGrid(_spawnerObjectGrid.Place_Object_Size).x * 0.1f) / 4));
            _material.SetVector("_worldPosition", new Vector4(1 - ((raycast_hit.Item3.x / _landscapeSize.x) + 0.5f), 1 - ((raycast_hit.Item3.y / _landscapeSize.x) + 0.5f)));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(hit2.point, hit2.normal);
        }
    }
}
