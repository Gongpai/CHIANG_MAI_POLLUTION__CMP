using UnityEngine;

namespace GDD
{
    public class Worker_Object_Pool_Script : People_Object_Pool_Script
    {
        public override People_System_Script CreatePeople()
        {
            GameObject _people = new GameObject();
            _people.name = "worker";
            _people.transform.parent = gameObject.transform;
            return _people.AddComponent<Worker_System_Script>();;
        }
    }
}