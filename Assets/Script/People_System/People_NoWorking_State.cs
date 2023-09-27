using UnityEngine;

namespace GDD
{
    public class People_NoWorking_State : MonoBehaviour, IPeople_State
    {
        private People_System_Script _peopleSystemScript;
        
        public void Daily_life(People_System_Script peopleSystemScript)
        {
            _peopleSystemScript = peopleSystemScript;
        }
    }
}