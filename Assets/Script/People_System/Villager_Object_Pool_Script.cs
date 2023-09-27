using UnityEngine;

namespace GDD
{
    public class Villager_Object_Pool_Script : People_Object_Pool_Script
    {
        public override People_System_Script CreatePeople()
        {
            GameObject _people = new GameObject();
            _people.name = "villager";
            _people.transform.parent = gameObject.transform;
            return _people.AddComponent<Villager_System_Script>();;
        }
    }
}