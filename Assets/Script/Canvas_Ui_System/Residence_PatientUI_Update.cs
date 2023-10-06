using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GDD
{
    public class Residence_PatientUI_Update : MonoBehaviour
    {
        [SerializeField] private Canvas_Element_List m_residence;
        [SerializeField] private Canvas_Element_List m_patient;

        private HumanResourceManager HRM;
        private GameManager GM;

        private void Start()
        {
            HRM = HumanResourceManager.Instance;
            GM = GameManager.Instance;
        }

        private void Update()
        {
            Resident_Update();
            Patient_Update();
            
            m_residence.texts[0].text = HRM.residence.Count.ToString();
            m_residence.texts[1].text = Mathf.Abs(HRM.residence.Count - (GM.gameInstance.get_villager_count() + GM.gameInstance.get_worker_count())).ToString();
            m_patient.texts[0].text = HRM.healing.Count.ToString();
            m_patient.texts[1].text = HRM.patients.Count.ToString();
        }

        private void Resident_Update()
        {
            //print("Resi : " + (HRM.residence.Count - (GM.gameInstance.get_villager_count() + GM.gameInstance.get_worker_count())));
            if (HRM.residence.Count - (GM.gameInstance.get_villager_count() + GM.gameInstance.get_worker_count()) < 0)
            {
                m_residence.animators[0].SetBool("IsStart", true);
            }
            else
            {
                m_residence.animators[0].SetBool("IsStart", false);
            }
        }
        
        private void Patient_Update()
        {
            if (HRM.healing.Count > 0 || HRM.patients.Count > 0)
            {
                m_patient.animators[0].SetBool("IsOn", true);
            }else
            {
                m_patient.animators[0].SetBool("IsOn", false);
            }
            
            if (HRM.patients.Count > 0)
            {
                m_patient.animators[0].SetBool("IsStart", true);
            }
            else
            {
                m_patient.animators[0].SetBool("IsStart", false);
            }
        }
    }
}