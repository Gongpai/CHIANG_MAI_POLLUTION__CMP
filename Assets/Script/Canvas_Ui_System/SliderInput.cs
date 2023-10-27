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
        [SerializeField] private bool _is_percent;

        public string text
        {
            get => _text;
            set => _text = value;
        }
        
        private void Start()
        {
            if (_is_percent)
            {
                m_result.text = _text + Mathf.FloorToInt(((_slider.value + 80) / 80) * 100) + "%";
                m_min.text = "0";
                m_max.text = "125";
            }
            else
            {
                m_result.text = _text + Mathf.FloorToInt(_slider.value);
                m_min.text = _slider.minValue.ToString();
                m_max.text = _slider.maxValue.ToString();
            }
        }

        private void Update()
        {
            if (_is_percent)
                m_result.text = _text + Mathf.FloorToInt(((_slider.value + 80) / 80) * 100) + "%";
            else
                m_result.text = _text + Mathf.FloorToInt(_slider.value);
        }
    }
}