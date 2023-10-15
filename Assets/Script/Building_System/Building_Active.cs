using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Building_Active : MonoBehaviour
    {
        [SerializeField] private List<HingeJoint> m_hingeJoints = new List<HingeJoint>();
        [SerializeField] private List<MeshRenderer> m_materials = new ();
        [SerializeField] private List<Light> m_lights = new List<Light>();
        
        public void Stop()
        {
            foreach (var hingeJoint in m_hingeJoints)
            {
                hingeJoint.useMotor = false;
            }

            foreach (var material in m_materials)
            {
                material.material.DisableKeyword("_EMISSION");
            }

            foreach (var vLight in m_lights)
            {
                vLight.enabled = false;
            }
        }
        
        public void Play()
        {
            foreach (var hingeJoint in m_hingeJoints)
            {
                hingeJoint.useMotor = true;
            }

            foreach (var material in m_materials)
            {
                material.material.EnableKeyword("_EMISSION");
            }
            
            foreach (var vLight in m_lights)
            {
                vLight.enabled = true;
            }
        }
    }
}