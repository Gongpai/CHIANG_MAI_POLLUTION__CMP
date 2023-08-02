using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class SetPositionShowGirdUseMouse : MonoBehaviour
    {
        [SerializeField] private Shader _shader;
        [SerializeField] private Color activate_Color;
        [SerializeField] private Color deactivate_Color;

        private Material _material;
        private MeshRenderer _meshRenderer;

        private Vector3 _landscapeSize;
        private Spawner_Object_Grid _spawnerObjectGrid;

        private RaycastHit hit1;
        private RaycastHit hit2;

        public Vector3 LandscapeSize
        {
            get { return _landscapeSize; }
            set { _landscapeSize = value; }
        }

        private void Awake()
        {
            this.enabled = false;
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
            Ray_to_Lanscape(_spawnerObjectGrid.GetRayFromMouse());
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


            _material.SetVector("_worldPosition", new Vector4(1 - ((raycast_hit.Item3.x / _landscapeSize.x) + 0.5f), 1 - ((raycast_hit.Item3.y / _landscapeSize.x) + 0.5f)));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(hit2.point, hit2.normal);
        }
    }
}
