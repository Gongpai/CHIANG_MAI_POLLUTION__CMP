using System;
using UnityEngine;

namespace GDD
{
    public class Sinagleton_CanDestroy<T> : Singleton_With_DontDestroy<T> where T : Sinagleton_CanDestroy<T>
    {
        public override void OnAwake()
        {
            
        }
    }
}