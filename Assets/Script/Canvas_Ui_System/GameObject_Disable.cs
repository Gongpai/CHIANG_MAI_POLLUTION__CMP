using System;
using UnityEngine;

namespace GDD
{
    public class GameObject_Disable : MonoBehaviour
    {
        [SerializeField] public bool _active = true;

        private void Update()
        {
            if(!_active)
                gameObject.SetActive(false);
        }
    }
}