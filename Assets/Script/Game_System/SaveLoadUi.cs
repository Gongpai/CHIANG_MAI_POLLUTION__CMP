using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GDD
{
    public class SaveLoadUi : MonoBehaviour
    {
        [SerializeField] private LoadingSceneSystem m_loadingSceneSystem;
        [SerializeField] private TextMeshProUGUI m_save_Load_Text;
        [SerializeField] private Button m_button_new_savegame;
        [SerializeField] private GameObject m_Button_SaveLoad;
        [SerializeField] private GameObject m_Button_Delete;
        [SerializeField] private GameObject List_Content;
        [SerializeField] private GameObject InputUI;
        [SerializeField] private GameObject MessageBoxUI;
        [SerializeField] private Button BackButton;
        [SerializeField] private Animator m_animator;

        private bool _isOpenSaveUi;
        private GameManager GM;
        private SaveManager SM;
        private Ui_Utilities uiUtilities;
        private string NameSaveGame_File;
        
        public enum SelectModeSaveGame
        {
            remove,
            overwrite,
            read
        }

        public bool IsOpenSaveUi
        {
            get => _isOpenSaveUi;
            set
            {
                print("Is Open : " + _isOpenSaveUi);
                _isOpenSaveUi = value;
            }
        }

        private void Start()
        {
            if (m_animator != null)
            {
                m_animator.SetBool("IsStart", true);
            }

            if (m_loadingSceneSystem == null)
                m_loadingSceneSystem = FindObjectOfType<LoadingSceneSystem>();

            OnOpenUi();
        }

        public void OnOpenUi()
        {
            GM = GameManager.Instance;
            SM = SaveManager.Instance;

            
            if(GetComponent<Ui_Utilities>() == null)
                gameObject.AddComponent<Ui_Utilities>();
            
            uiUtilities = GetComponent<Ui_Utilities>();
             /*
            BackButton.onClick.AddListener(() =>
            {
                
                Destroy(gameObject);
            });
            */
            
             print("Is Open in game : " + _isOpenSaveUi);
            if (_isOpenSaveUi)
            {
                m_save_Load_Text.text = "Save Game";
                
                m_button_new_savegame.onClick.AddListener(saveButton);
                ListAllSaveGame(SelectModeSaveGame.overwrite);
                print(m_save_Load_Text.text);
            }
            else
            {
                m_button_new_savegame.transform.gameObject.SetActive(false);
                m_save_Load_Text.text = "Load Game";
                ListAllSaveGame(SelectModeSaveGame.read);
                print(m_save_Load_Text.text);
            }
        }

        private void AddNewSaveGameSlot()
        {
            uiUtilities.UI_Elemant = m_Button_SaveLoad;
            List<UnityAction> button_action = new List<UnityAction>();
            button_action.Add(saveAction);
            uiUtilities.Add_Button_Element_To_List_View(List_Content, button_action, "New Save Game");
        }
        private void ListAllSaveGame(SelectModeSaveGame savemode)
        {
            SaveManager SM = SaveManager.Instance;
            var SaveGameList = SM.GetAllSaveGame(isDefaultPath: true);
            
            foreach (var SG in SaveGameList)
            {
                string textbutton = SG.Name.Split('.')[0];
                List<UnityAction> button_action = new List<UnityAction>();
                button_action.Add((() =>
                {
                    print("Load or Overwrite"); loadAndOverwriteAction(savemode, textbutton);
                }));
                button_action.Add((() => { print("Delete Saveeee"); deleteAction(textbutton);}));
                
                uiUtilities.UI_Elemant = m_Button_SaveLoad;
                uiUtilities.Add_Button_Element_To_List_View(List_Content, button_action, textbutton);
                
                //uiUtilities.UI_Elemant = m_Button_Delete;
                //uiUtilities.Add_Button_Element_To_List_View(m_button, , textbutton);
            }
        }

        public void saveAction()
        {
            uiUtilities.canvasUI = InputUI;
            uiUtilities.useCameraOverlay = true;
            uiUtilities.planeDistance = 0.5f;
            uiUtilities.order_in_layer = 12;
            GameObject m_input_button = uiUtilities.CreateInputUI(arg0 => { OnChangeSaveGameFile(arg0); },
                () =>
                {
                    foreach (Transform child in List_Content.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    //AddNewSaveGameSlot();
                    SaveGame(NameSaveGame_File);
                    ListAllSaveGame(SelectModeSaveGame.overwrite);
                }, (() => { m_animator.SetBool("IsStart", true); }), "Name your save file :", "Save Game", "Cancel");

            m_animator.SetBool("IsEnd", false);
            m_animator.SetBool("IsStart", false);
            m_input_button.GetComponent<Animator>().SetBool("IsStart", true);
            m_input_button.GetComponent<Back_UI_Button_Script>().UI_Back = transform.parent.gameObject;

        }

        public void loadAndOverwriteAction(SelectModeSaveGame savemode, string textbutton)
        {
            switch (savemode)
            {
                case SelectModeSaveGame.overwrite:
                    uiUtilities.canvasUI = MessageBoxUI;
                    uiUtilities.useCameraOverlay = true;
                    uiUtilities.planeDistance = 0.5f;
                    uiUtilities.order_in_layer = 12;
                    GameObject message_overwrite_ui = uiUtilities.CreateMessageUI(
                        () => { SaveGame(textbutton); }, (() => { m_animator.SetBool("IsStart", true); }),
                        "Overwrite game save file?", "Do you want to overwrite this game save file?", "Yes", "Cancel");

                    m_animator.SetBool("IsEnd", false);
                    m_animator.SetBool("IsStart", false);
                    message_overwrite_ui.GetComponent<Animator>().SetBool("IsStart", true);
                    message_overwrite_ui.GetComponent<Back_UI_Button_Script>().UI_Back = transform.parent.gameObject;
                    break;
                case SelectModeSaveGame.read:
                    uiUtilities.canvasUI = MessageBoxUI;
                    uiUtilities.useCameraOverlay = true;
                    uiUtilities.planeDistance = 0.5f;
                    uiUtilities.order_in_layer = 12;
                    GameObject message_read_ui = uiUtilities.CreateMessageUI(
                        () => { LoadSave(textbutton); }, (() => { m_animator.SetBool("IsStart", true); }),
                        "Load this game save?", "Do you want to load a save game?", "Yes", "Cancel");

                    m_animator.SetBool("IsEnd", false);
                    m_animator.SetBool("IsStart", false);
                    message_read_ui.GetComponent<Animator>().SetBool("IsStart", true);
                    message_read_ui.GetComponent<Back_UI_Button_Script>().UI_Back = transform.parent.gameObject;
                    break;
                default:
                    break;
            }
        }

        public void deleteAction(string textbutton)
        {
            uiUtilities.canvasUI = MessageBoxUI;
            uiUtilities.useCameraOverlay = true;
            uiUtilities.planeDistance = 0.5f;
            uiUtilities.order_in_layer = 12;
            GameObject message_del_ui = uiUtilities.CreateMessageUI(
                () =>
                {
                    DeleteSaveGame(textbutton);

                    foreach (Transform child in List_Content.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    if (_isOpenSaveUi)
                    {
                        m_save_Load_Text.text = "Save Game";

                        //AddNewSaveGameSlot();
                        ListAllSaveGame(SelectModeSaveGame.overwrite);
                    }
                    else
                    {
                        m_save_Load_Text.text = "Load Game";
                        ListAllSaveGame(SelectModeSaveGame.read);
                    }
                }, (() => { }), "Delete game save file?", "Do you want to delete game save file?", "Yes", "Cancel");

            m_animator.SetBool("IsEnd", false);
            m_animator.SetBool("IsStart", false);
            message_del_ui.GetComponent<Animator>().SetBool("IsStart", true);
            message_del_ui.GetComponent<Back_UI_Button_Script>().UI_Back = transform.parent.gameObject;
        }

        public void saveButton()
        {
            uiUtilities.canvasUI = InputUI;
            uiUtilities.useCameraOverlay = true;
            uiUtilities.planeDistance = 0.5f;
            uiUtilities.order_in_layer = 12;
            GameObject m_input_button = uiUtilities.CreateInputUI(arg0 =>
                {
                    OnChangeSaveGameFile(arg0);
                }, 
                () =>
                {
                    foreach (Transform child in List_Content.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    //AddNewSaveGameSlot();
                    SaveGame(NameSaveGame_File);
                    ListAllSaveGame(SelectModeSaveGame.overwrite);
                }, (() =>
                {
                    
                }),"Name your save file :", "Save Game", "Cancel");
            
            m_animator.SetBool("IsEnd", false);
            m_animator.SetBool("IsStart", false);
            m_input_button.GetComponent<Animator>().SetBool("IsStart", true);
            m_input_button.GetComponent<Back_UI_Button_Script>().UI_Back = transform.parent.gameObject;
        }

        public void LoadSave(string fileName)
        {
            Destroy(TimeManager.Instance.gameObject);
            
            var saveData = SM.LoadGamePreferencesData(Application.persistentDataPath + "/" + fileName, GM.gameInstance.GetType());

            GM.GetSetGameManager = saveData;
            GM.OnGameLoad();
            
            m_loadingSceneSystem.LoadScene("GameLevel");
        }

        public void OnChangeSaveGameFile(string value)
        {
                NameSaveGame_File = value;
        }
        
        public void SaveGame(string fileName)
        {
            if (fileName != null)
            {
                print(fileName + " save");
                SM.SaveGamePreferencesData(GM.gameInstance, Application.persistentDataPath + "/" + fileName);
                NameSaveGame_File = null;
            }
        }

        private void DeleteSaveGame(string fileName)
        {
            File.Delete(Application.persistentDataPath + "/" + fileName + ".json");
        }
    }
}
