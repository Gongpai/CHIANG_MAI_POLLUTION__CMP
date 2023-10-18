using System;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class Warning_PM2_5_UI_Script : MonoBehaviour
    {
        private UnityAction _action;
        
        public UnityAction action
        {
            get => _action;
            set => _action = value;
        }

        private void Update()
        {
            
        }
    }
}