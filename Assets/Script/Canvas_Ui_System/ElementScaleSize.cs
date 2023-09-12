using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ElementScaleSize : MonoBehaviour
    {
        [SerializeField]public Vector2 m_Size = new Vector2(1f, 1f);
        [SerializeField]private Vector2 max_size = new Vector2(260, 100);
        [SerializeField] private bool m_useSizeCanvasOtherElement;
        [SerializeField] private GameObject m_CanvasOtherElement;
        
        // Start is called before the first frame update
        private void OnEnable()
        {
            if (m_useSizeCanvasOtherElement && m_CanvasOtherElement != null)
            {
                max_size = m_CanvasOtherElement.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
            }
            else
            {
                max_size = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
            }

            print(transform.GetChild(0).GetComponent<RectTransform>().sizeDelta);
        }

        // Update is called once per frame
        void Update()
        {
            max_size = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(max_size.x * m_Size.x, max_size.y * m_Size.y);
        }
    }
}