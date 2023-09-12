using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class Button_Switch_Tab_Animation_Control : MonoBehaviour
    {
        [SerializeField] private bool m_playAnimBeginStart = true;
        [SerializeField] private int index = 0;
        [SerializeField] private List<GameObject> m_button;
        [SerializeField] private int limit_tab_index = -1;
        
        public List<GameObject> button
        {
            get => m_button;
        }

        private void Start()
        {
            if(m_playAnimBeginStart)
                m_button[index].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", true);
        }

        public void OnSwitchTab(int i_tab)
        {
            if (m_button[i_tab].GetComponent<Canvas_Element_List>() != null)
                m_button[i_tab].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", true);

            for(int i = 0; i < m_button.Count; i++)
            {
                if (limit_tab_index > -1)
                {
                    if (i_tab > limit_tab_index)
                    {
                        m_button[0].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", true);
                    }
                    if (i != i_tab)
                    {
                        //print("I Tab : " + i_tab + " Index : " + i);
                            
                        if (m_button[i].GetComponent<Canvas_Element_List>())
                        {
                            m_button[i].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", false);
                        }
                    }
                }
                else
                {
                    if (i != i_tab)
                    {
                        m_button[i].GetComponent<Canvas_Element_List>().animators[0].SetBool("Select_Button", false);
                    }
                }
            }
        }
    }
}