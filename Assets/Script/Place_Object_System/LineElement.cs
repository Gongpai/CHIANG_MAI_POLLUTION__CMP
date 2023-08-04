using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [Serializable]
    public class LineElement
    {
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 end;
        
        public Vector3 Start {
            get { return start; }
            set { start = value; }
        }
        public Vector3 End {
            get { return end; }
            set { end = value; }
        }
    }
}