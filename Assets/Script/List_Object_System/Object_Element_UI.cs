using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class Object_Element_UI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        public Image image
        {
            get { return _image; }
        }

        public TextMeshProUGUI text
        {
            get { return _text; }
        }

        public Button botton
        {
            get { return _button; }
        }
    }
}
