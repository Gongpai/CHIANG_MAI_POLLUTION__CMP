using System;
using UnityEngine;

namespace GDD
{
    public class ElementFitSizeToOtherElement : MonoBehaviour
    {
        [SerializeField] private GameObject m_Element;
        private RectTransform _rectTransforml;

        private void Start()
        {
            _rectTransforml = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransforml.anchoredPosition = m_Element.GetComponent<RectTransform>().anchoredPosition;
            _rectTransforml.sizeDelta = m_Element.GetComponent<RectTransform>().sizeDelta;
            _rectTransforml.localScale = m_Element.GetComponent<RectTransform>().localScale;
        }
    }
}