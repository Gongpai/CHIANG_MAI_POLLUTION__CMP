using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Outline.Scripts
{
    public class AutoAddOutlinerGameObjects : MonoBehaviour
    {
        private List<Outliner> _outliners = new ();

        public List<Outliner> get_outliners
        {
            get => _outliners;
        }

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Outliner>() == null && transform.GetChild(i).GetComponent<MeshFilter>() != null)
                {
                    _outliners.Add(transform.GetChild(i).AddComponent<Outliner>());
                    if (transform.GetChild(i).gameObject.tag != "Movement")
                    {
                        transform.GetChild(i).localScale = Vector3.one;
                        transform.GetChild(i).transform.localPosition = Vector3.zero;
                    }
                }
            }

            foreach (var _outliner in _outliners)
            {
                _outliner.OutlineWidth = 1.05f;
                _outliner.enabled = false;
            }
            
            print("All Outliner : " + get_outliners.Count);
        }
    }
}