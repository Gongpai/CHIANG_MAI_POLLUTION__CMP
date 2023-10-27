using System;
using UnityEngine;

namespace GDD
{
    public class Credits_Scrolling_Script : MonoBehaviour
    {
        [SerializeField] private Back_UI_Button_Script m_buttonScript;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if(_rectTransform.anchoredPosition.y <= (_rectTransform.rect.height + (Screen.height * 2)))
            {
                if(Input.GetMouseButton(0))
                    _rectTransform.anchoredPosition += (Vector2.up * 130) * Time.deltaTime;
                else
                    _rectTransform.anchoredPosition += (Vector2.up * 30) * Time.deltaTime;
                //print("AnchoredPosition : " + _rectTransform.anchoredPosition);
                //print("Position : " + _rectTransform.position);
                //print("Rect Position : " + _rectTransform.rect.position);
            }

            if(_rectTransform.anchoredPosition.y >= (_rectTransform.rect.height + Screen.height))
                m_buttonScript.OnDestroyUI(true);
        }
    }
}