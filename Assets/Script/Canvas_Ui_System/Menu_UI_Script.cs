using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GDD
{
    public class Menu_UI_Script : MonoBehaviour
    {
        [SerializeField] private LoadingSceneSystem m_loadingSceneSystem;
        [SerializeField] private GameObject parent_UI;
        [SerializeField] private Button m_new_game_button;
        [SerializeField] private Button m_continue_button;
        [SerializeField] private Button m_resume_button;
        [SerializeField] private Button m_savegame_button;
        [SerializeField] private GameObject savegame_ui;
        [SerializeField] private Button m_loadgame_button;
        [SerializeField] private Button m_setting_button;
        [SerializeField] private GameObject setting_ui;
        [SerializeField] private Button m_backtomainmenu_button;
        [SerializeField] private Button m_credits_button;
        [SerializeField] private GameObject credits_ui;
        [SerializeField] private Button m_quit_button;
        [SerializeField] private Animator m_animator;

        private Ui_Utilities _uiUtilities;
        private BT_CS_Instance _btCsInstance;
        private SaveManager SM;
        private GameManager GM;
        private List<String> anim_param_names =new List<string>(2);

        private void Start()
        {
            SM = SaveManager.Instance;
            GM = GameManager.Instance;

            if (m_loadingSceneSystem == null)
                m_loadingSceneSystem = FindObjectOfType<LoadingSceneSystem>();
            
            anim_param_names.Add("IsStart");
            anim_param_names.Add("IsStart_No_Blur");
            var ddd = GraphicsSettings.currentRenderPipeline;
            _btCsInstance = parent_UI.GetComponent<BT_CS_Instance>();
            Button_Control_Script bt_cs = _btCsInstance.BT_CS;
            _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            
            m_animator.SetBool(anim_param_names[0], true);
            m_animator.SetBool(anim_param_names[1], false);
            
            if (SM.GetAllSaveGame(default, true).Count > 0 && m_continue_button != null)
            {
                m_continue_button.gameObject.SetActive(true);
            }
            
            if (m_resume_button != null)
            {
                m_resume_button.onClick.AddListener(() =>
                {
                    bt_cs.OnDestroyCanvas(default, true, anim_param_names[1], true);
                });
            }

            if (m_backtomainmenu_button != null)
            {
                m_backtomainmenu_button.onClick.AddListener((() =>
                {
                    m_loadingSceneSystem.LoadScene("MainMenu");
                    GM.Reset();
                    Destroy(HumanResourceManager.Instance.gameObject);
                    Destroy(ResourcesManager.Instance.gameObject);
                }));
            }

            if (m_new_game_button != null)
            {
                m_new_game_button.onClick.AddListener(() =>
                {
                    m_loadingSceneSystem.action_loading_scane = () =>
                    {
                        Destroy(TimeManager.Instance.gameObject);
                        Destroy(SaveManager.Instance.gameObject);
                    };
                    
                    m_animator.SetBool(anim_param_names[0], false);
                    m_loadingSceneSystem.LoadScene("GameLevel");
                });
            }

            if (m_continue_button != null)
            {
                m_continue_button.onClick.AddListener(() =>
                {
                    m_loadingSceneSystem.action_loading_scane = () =>
                    {
                        string fileName = SM.GetAllSaveGame(default, true)[0].Name.Split(".")[0];

                        var saveData = SM.LoadGamePreferencesData(Application.persistentDataPath + "/" + fileName,
                            GM.gameInstance.GetType());

                        print("Load Save : " + fileName);

                        GM = GameManager.Instance;
                        GM.GetSetGameManager = saveData;
                        GM.OnGameLoad();
                    };
                    
                    m_animator.SetBool(anim_param_names[0], false);
                    m_loadingSceneSystem.LoadScene("GameLevel");
                    Destroy(SaveManager.Instance.gameObject);
                    Destroy(TimeManager.Instance.gameObject);
                });
            }

            if (m_quit_button != null)
            {
                m_quit_button.onClick.AddListener((() => { Application.Quit(0); }));
            }

            if (m_savegame_button != null)
            {
                m_savegame_button.onClick.AddListener((() => { OpenSaveLoadUI(true); }));
            }
            
            if(m_setting_button != null)
                m_setting_button.onClick.AddListener((() => { OpenSettingUI(); }));

            if (m_loadgame_button != null)
            {
                m_loadgame_button.onClick.AddListener((() => { OpenSaveLoadUI(false); }));
            }
            
            if(m_credits_button != null)
                m_credits_button.onClick.AddListener((() => { OpenCreditsUI(); }));
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

        public void OpenSettingUI()
        {
            setting_ui.GetComponent<Back_UI_Button_Script>().UI_Back = parent_UI;
            _uiUtilities.canvasUI = setting_ui;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.order_in_layer = 11;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.cameraOverlay_Index = 0;
            GameObject set_ui = _uiUtilities.CreateUI();

            set_ui.GetComponent<Animator>().enabled = true;
            set_ui.GetComponent<Animator>().SetBool("IsStart", true);
            m_animator.SetBool(anim_param_names[0], false);
            m_animator.SetBool(anim_param_names[1], false);
        }
        
        public void OpenCreditsUI()
        {
            credits_ui.GetComponent<Back_UI_Button_Script>().UI_Back = parent_UI;
            _uiUtilities.canvasUI = credits_ui;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.order_in_layer = 11;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.cameraOverlay_Index = 0;
            GameObject set_ui = _uiUtilities.CreateUI();

            set_ui.GetComponent<Animator>().enabled = true;
            set_ui.GetComponent<Animator>().SetBool("IsStart", true);
            m_animator.SetBool(anim_param_names[0], false);
            m_animator.SetBool(anim_param_names[1], false);
        }
    }
}