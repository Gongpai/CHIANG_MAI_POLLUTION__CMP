using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class SliderInput : MonoBehaviour
    {
        [SerializeField] [TextArea] private string _text;
        [SerializeField] private TextMeshProUGUI m_result;
        [SerializeField] private TextMeshProUGUI m_min;
        [SerializeField] private TextMeshProUGUI m_max;
        [SerializeField] private Slider _slider;

        public string text
        {
            get => _text;
            set => _text = value;
        }
        
        private void Start()
        {
            m_result.text = _text + Mathf.FloorToInt(_slider.value);
            m_min.text = _slider.minValue.ToString();
            m_max.text = _slider.maxValue.ToString();
        }

        private void Update()
        {
            m_result.text = _text + Mathf.FloorToInt(_slider.value);
        }
    }
}