using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Button_Switch_Tab_Animation_Control : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_button;

        public List<GameObject> button
        {
            get => m_button;
        }
        public void OnSwitchTab(int index)
        {
            m_button[index].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", true);

            for(int i = 0; i < m_button.Count; i++)
            {
                if (i != index)
                {
                    m_button[i].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", false);
                }
            }
        }
    }
}