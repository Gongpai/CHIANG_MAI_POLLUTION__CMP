using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ElementFitSize : MonoBehaviour
    {
        [SerializeField]public Vector2 m_Size = new Vector2(1f, 1f);
        private Vector2 max_size = new Vector2(260, 100);
        
        // Start is called before the first frame update
        private void OnEnable()
        {
            max_size = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
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