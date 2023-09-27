using System;
using UnityEngine;

namespace GDD
{
    public class People_Context_Script
    {
        public IPeople_State CurrentState { get; set; }

        private readonly People_System_Script _peopleSystemScript;

        public People_Context_Script(People_System_Script peopleSystemScript)
        {
            _peopleSystemScript = peopleSystemScript;
        }

        public void Transition(IPeople_State state)
        {
            CurrentState = state;
        }

        public void Update_per_hour()
        {
            Debug.Log("Update");
            if(CurrentState != null)
                CurrentState.Daily_life(_peopleSystemScript);
        }
    }
}