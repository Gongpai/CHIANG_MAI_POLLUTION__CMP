using System;
using UnityEngine;

namespace GDD
{
    public class Back_UI_Button_Script : MonoBehaviour
    {
        [SerializeField] private GameObject m_ui_back;
        private Ui_Utilities _uiUtilities;

        public GameObject UI_Back
        {
            get => m_ui_back;
            set => m_ui_back = value;
        }

        private void Start()
        {
            if (GetComponent<Ui_Utilities>() == null)
            {
                _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            }
            else
            {
                _uiUtilities = gameObject.GetComponent<Ui_Utilities>();
            }
        }

        public void OnDestroyUI(bool is_anim_other_param = true)
        {
            GetComponent<Canvas>().sortingOrder = UI_Back.GetComponent<Canvas>().sortingOrder - 1;
            _uiUtilities.canvasUI = gameObject;
            _uiUtilities.RemoveUI();
            Animator ui_back_animator = UI_Back.GetComponent<Animator>();
            ui_back_animator.SetBool("IsStart", true);
            
            if(is_anim_other_param)
                ui_back_animator.SetBool("IsStart_No_Blur", true);
        }
    }
}