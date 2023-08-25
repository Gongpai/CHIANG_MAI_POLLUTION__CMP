using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDD
{
    public class Element_Animation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]private Animator m_animator;
        [SerializeField]private bool m_usePointerDown = true;
        [SerializeField]private bool m_usePointerUp = true;
        [SerializeField]private bool m_usePointerEnter = true;
        [SerializeField]private bool m_usePointerExit = true;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_usePointerDown)
            {
                m_animator.SetBool("Pressed", true);
                m_animator.SetBool("Released", false);
                m_animator.SetBool("Horered", false);
                m_animator.SetBool("Unhorered", false);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_usePointerUp)
            {
                m_animator.SetBool("Pressed", false);
                m_animator.SetBool("Released", true);
                m_animator.SetBool("Horered", false);
                m_animator.SetBool("Unhorered", false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_usePointerEnter)
            {
                m_animator.SetBool("Pressed", false);
                m_animator.SetBool("Released", false);
                m_animator.SetBool("Horered", true);
                m_animator.SetBool("Unhorered", false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_usePointerExit)
            {
                m_animator.SetBool("Pressed", false);
                m_animator.SetBool("Released", false);
                m_animator.SetBool("Horered", false);
                m_animator.SetBool("Unhorered", true);
            }
        }
    }
}