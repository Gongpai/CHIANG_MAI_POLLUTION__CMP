using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class Economy_Control_UI : MonoBehaviour
    {
        [SerializeField] private List<Economy_Data> m_elements;
        [SerializeField] private GameObject m_viewport;
        [SerializeField] private ScrollRect m_scrollRect;

        public void SwitchPage(int i)
        {
            if(m_viewport.transform.GetChild(0) != null)
                Destroy(m_viewport.transform.GetChild(0).gameObject);

            GameObject element = Instantiate(m_elements[i].gameObject, m_viewport.transform);
            m_scrollRect.content = element.GetComponent<RectTransform>();
            m_scrollRect.horizontal = m_elements[i].scrollMode == Economy_Data.ScrollMode.Horizontal || m_elements[i].scrollMode == Economy_Data.ScrollMode.HorizontalAndVertical;
            m_scrollRect.vertical = m_elements[i].scrollMode == Economy_Data.ScrollMode.Vertical || m_elements[i].scrollMode == Economy_Data.ScrollMode.HorizontalAndVertical;
        }
    }

    [Serializable]
    public class Economy_Data
    {
        [Serializable]
        public enum ScrollMode
        {
            HorizontalAndVertical,
            Horizontal,
            Vertical
        }

        public ScrollMode scrollMode;
        public GameObject gameObject;
    }
}