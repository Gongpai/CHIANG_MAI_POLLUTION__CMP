using System;
using UnityEngine;

namespace GDD
{
    public class People_Sick_State : MonoBehaviour, IPeople_State
    {
        private People_System_Script _peopleSystemScript;

        private float hunger
        {
            get => _peopleSystemScript.hunger;
            set => _peopleSystemScript.hunger = value;
        }

        private float health
        {
            get => _peopleSystemScript.health;
            set => _peopleSystemScript.health = value;
        }

        private PeopleDailyLife dailyLife
        {
            get => _peopleSystemScript.dailyLife;
            set => _peopleSystemScript.dailyLife = value;
        }

        public void Daily_life(People_System_Script peopleSystemScript)
        {
            print("DDDDDDD LLLLLL");
            _peopleSystemScript = peopleSystemScript;
            
            if (health + 0.1f <= 1)
            {
                health += 0.1f;
            } else if (health + 0.1f >= 1)
            {
                health = 1;
                dailyLife = PeopleDailyLife.Working_State;
            }
        }
    }
}