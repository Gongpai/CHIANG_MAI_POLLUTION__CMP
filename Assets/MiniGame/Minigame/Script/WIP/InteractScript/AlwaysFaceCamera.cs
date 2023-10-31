using System;
using UnityEngine;

namespace Samarnggoon.GameDev3.Chapter5.Utility
{
    public class AlwaysFaceCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = 
                Quaternion.LookRotation( transform.position - Camera.main.transform.position );
        }
    }
}