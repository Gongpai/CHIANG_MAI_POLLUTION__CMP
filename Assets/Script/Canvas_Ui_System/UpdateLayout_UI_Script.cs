using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class UpdateLayout_UI_Script : MonoBehaviour
    {
        private GameObject button_BG;
        private Button _button;
        private Button Bg_button_sc;
        private Animator Bg_animator;
        
        public GameObject button_bg
        {
            get { return button_BG; }
            set { button_BG = value; }
        }
        public Button button
        {
            get { return _button; }
            set { _button = value; }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (button_BG != null)
            {
                Bg_button_sc = button_BG.GetComponent<Button>();
                Bg_animator = button_BG.GetComponent<Animator>();
            }
            
            if (button_BG != null && gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>() != null)
            {
                UpdateLayout();
            }
        }
        
        private void UpdateLayout()
        {
            bool layoutUpdate = gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth;
            bool BGlayoutUpdate = button_BG.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth;
            gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = !layoutUpdate;
            Bg_button_sc.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = !BGlayoutUpdate;
        }
    }
}