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
        private HumanResourceManager HRM;
        private float re_health = 0.35f;
        private bool is_find_treatment = false;
        private bool is_healing = false;
        
        private float hunger
        {
            get => _peopleSystemScript.hunger;
            set => _peopleSystemScript.hunger = value;
        }

        private void Start()
        {
            HRM = HumanResourceManager.Instance;
        }

        private float re_health_efficiency
        {
            get
            {
                if (_peopleSystemScript.peopleJob == PeopleJob.Nurse)
                {
                    return re_health;
                }
                else
                {
                    return re_health * _peopleSystemScript._current_healthScript.efficiency;
                }
            }
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

            if (_peopleSystemScript.peopleJob == PeopleJob.Nurse)
            {
                OnNurseAdmit();
                OnRecoverIllness();
            }
            else if (_peopleSystemScript._current_healthScript != null)
            {
                OnPeopleAdmit();
                OnRecoverIllness();
            }
            else
            {
                print("Find Treatment");
                _peopleSystemScript._current_healthScript = FindTreatment();

                if (!is_find_treatment)
                {
                    HRM.patients.Add(_peopleSystemScript);
                }

                is_find_treatment = true;

                if (_peopleSystemScript.peopleJob == PeopleJob.Nurse)
                {
                    print("Nurse Add");
                    Building_System_Script _buildingSystem =
                        (Building_System_Script)_peopleSystemScript._constructionSystem;
                    Health_Script _healthScript = (Health_Script)_buildingSystem;
                    _healthScript.OnAdmit(_peopleSystemScript.saveData, true);
                    
                    if (is_find_treatment && !is_healing)
                        HRM.patients.Remove(_peopleSystemScript);

                    is_find_treatment = false;
                    HRM.healing.Add(_peopleSystemScript);
                    print("On Admit");
                    is_healing = true;
                }
                else if (_peopleSystemScript._current_healthScript != null)
                {
                    _peopleSystemScript._current_healthScript.OnAdmit(_peopleSystemScript.saveData);

                    if (is_find_treatment && !is_healing)
                        HRM.patients.Remove(_peopleSystemScript);

                    is_find_treatment = false;
                    HRM.healing.Add(_peopleSystemScript);
                    print("On Admit");
                    is_healing = true;
                }
            }
        }

        private void OnPeopleAdmit()
        {
            if (_peopleSystemScript._current_healthScript.building_is_active)
            {
                if (health + re_health_efficiency <= 1)
                {
                    if (health > 0)
                        health += re_health_efficiency;
                }
            }
        }

        private void OnNurseAdmit()
        {
            if (_peopleSystemScript._constructionSystem.Get_Construction_Active())
            {
                if (health + re_health_efficiency <= 1)
                {
                    if (health > 0)
                        health += re_health_efficiency;
                }
            }
        }

        private void OnRecoverIllness()
        {
            if (health + re_health_efficiency >= 1)
            {
                health = 1;
                dailyLife = PeopleDailyLife.Working_State;
                
                if (_peopleSystemScript.peopleJob == PeopleJob.Nurse)
                {
                    print("Nurse Remove");
                    Building_System_Script _buildingSystem = (Building_System_Script)_peopleSystemScript._constructionSystem;
                    Health_Script _healthScript = (Health_Script)_buildingSystem;
                    _healthScript.OnRecoverIllness(_peopleSystemScript.saveData, true);
                }
                else
                {
                    _peopleSystemScript._current_healthScript.OnRecoverIllness(_peopleSystemScript.saveData);
                }

                _peopleSystemScript._current_healthScript = null;
                    
                if (is_healing)
                {
                    HRM.healing.Remove(_peopleSystemScript);
                }
                is_healing = false;
            }
        }

        private Health_Script FindTreatment()
        {
            List<Health_Script> _healthScripts = FindObjectsByType<Health_Script>(FindObjectsInactive.Exclude ,FindObjectsSortMode.None).ToList();
            Health_Script _healthScript = null;
            
            Parallel.ForEach(_healthScripts, (h_script, state) =>
            {
                if (h_script.get_patient_count < h_script.get_patient_max)
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