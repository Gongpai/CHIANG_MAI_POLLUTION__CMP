using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            if (_peopleSystemScript._current_healthScript != null)
            {
                print("On Admit");
                OnAdmit();
            }
            else
            {
                print("Find Treatment");
                _peopleSystemScript._current_healthScript = FindTreatment();
                
                if(_peopleSystemScript._current_healthScript != null)
                    _peopleSystemScript._current_healthScript.OnAdmit(_peopleSystemScript.saveData);
            }
        }

        private void OnAdmit()
        {
            if (health + 0.2f <= 1)
            {
                if(health > 0)
                    health += 0.2f;
            } else if (health + 0.2f >= 1)
            {
                health = 1;
                dailyLife = PeopleDailyLife.Working_State;
                _peopleSystemScript._current_healthScript.OnRecoverIllness(_peopleSystemScript.saveData);
                _peopleSystemScript._current_healthScript = null;
            }
        }

        private Health_Script FindTreatment()
        {
            List<Health_Script> _healthScripts = FindObjectsByType<Health_Script>(FindObjectsInactive.Exclude ,FindObjectsSortMode.None).ToList();
            Health_Script _healthScript = null;
            
            Parallel.ForEach(_healthScripts, (h_script, state) =>
            {
                if (h_script.get_patient_current < h_script.get_patient_max)
                {
                    _healthScript = h_script;
                    
                    Debug.LogWarning("Founded : " + _healthScript.name);
                    state.Break();
                }
                else
                {
                    Debug.LogError("Not Found");
                    _healthScripts = null;
                }
            });
            
            return _healthScript;
        }
    }
}