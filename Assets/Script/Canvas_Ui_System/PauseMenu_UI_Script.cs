using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GDD
{
    public class PauseMenu_UI_Script : MonoBehaviour
    {
        [SerializeField] private GameObject parent_UI;
        [SerializeField] private Button m_resume_button;
        [SerializeField] private Button m_savegame_button;
        [SerializeField] private GameObject savegame_ui;
        [SerializeField] private Button m_loadgame_button;
        [SerializeField] private GameObject loadgame_ui;
        [SerializeField] private Button m_setting_button;
        [SerializeField] private Button m_backtomainmenu_button;
        [SerializeField] private Button m_quit_button;
        [SerializeField] private Animator m_animator;

        private Ui_Utilities _uiUtilities;
        private BT_CS_Instance _btCsInstance;
        private List<String> anim_param_names =new List<string>(2);

        private void Start()
        {
            anim_param_names.Add("IsStart");
            anim_param_names.Add("IsStart_No_Blur");
            var ddd = GraphicsSettings.currentRenderPipeline;
            _btCsInstance = parent_UI.GetComponent<BT_CS_Instance>();
            Button_Control_Script bt_cs = _btCsInstance.BT_CS;
            _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            
            m_animator.SetBool(anim_param_names[0], true);
            m_animator.SetBool(anim_param_names[1], false);
            
            m_resume_button.onClick.AddListener(() =>
            {
                bt_cs.OnDestroyCanvas(default, true, anim_param_names[1], true);
            });
            m_backtomainmenu_button.onClick.AddListener((() =>
            {
                SceneManager.LoadScene(1);
                Destroy(GameManager.Instance.gameObject);
                Destroy(SaveManager.Instance.gameObject);
            }));
            m_quit_button.onClick.AddListener((() =>
            {
                Application.Quit(0);
            }));
            m_savegame_button.onClick.AddListener((() =>
            {
                OpenSaveLoadUI(true);
            }));
            m_loadgame_button.onClick.AddListener((() =>
            {
                OpenSaveLoadUI(false);
            }));
        }

        public void OpenSaveLoadUI(bool isSaveUI)
        {
            savegame_ui.GetComponent<Back_UI_Button_Script>().UI_Back = parent_UI;
            _uiUtilities.canvasUI = savegame_ui;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.order_in_layer = 11;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.cameraOverlay_Index = 0;
            GameObject ui = _uiUtilities.CreateUI();
            
            SaveLoadUi _saveLoadUi = ui.transform.GetChild(1).GetComponent<SaveLoadUi>();
            _saveLoadUi.IsOpenSaveUi = isSaveUI;
            m_animator.SetBool(anim_param_names[0], false);
            m_animator.SetBool(anim_param_names[1], false);
        }
    }
}