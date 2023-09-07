using System;
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
        [SerializeField] private Button m_loadgame_button;
        [SerializeField] private Button m_setting_button;
        [SerializeField] private Button m_backtomainmenu_button;
        [SerializeField] private Button m_quit_button;
        [SerializeField] private Animator m_animator;

        private BT_CS_Instance _btCsInstance;

        private void Start()
        {
            var ddd = GraphicsSettings.currentRenderPipeline;
            _btCsInstance = parent_UI.GetComponent<BT_CS_Instance>();
            Button_Control_Script bt_cs = _btCsInstance.BT_CS;
            
            m_animator.SetBool("IsStart", true);
            
            m_resume_button.onClick.AddListener(() =>
            {
                bt_cs.OnDestroyCanvas();
            });
            m_backtomainmenu_button.onClick.AddListener((() =>
            {
                SceneManager.LoadScene(0);
                Destroy(GameManager.Instance.gameObject);
                Destroy(SaveManager.Instance.gameObject);
            }));
            m_quit_button.onClick.AddListener((() =>
            {
                Application.Quit(0);
            }));
        }
    }
}