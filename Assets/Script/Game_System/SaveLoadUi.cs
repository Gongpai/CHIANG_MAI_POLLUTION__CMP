using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class SaveLoadUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Save_Load_Text;
        [SerializeField] private GameObject m_Button_SaveLoad;
        [SerializeField] private GameObject m_Button_Delete;
        [SerializeField] private GameObject List_Content;
        [SerializeField] private GameObject InputUI;
        [SerializeField] private GameObject MessageBoxUI;
        [SerializeField] private Button BackButton;

        private bool IsOpenSaveUi = false;
        enum SelectModeSaveGame
        {
            remove,
            overwrite,
            read
        }

        private GameManager GM = default;
        private SaveManager SM = default;
        private Ui_Utilities uiUtilities;
        private string NameSaveGame_File;

        public void OnOpenUi(bool OpenSaveUi)
        {
            IsOpenSaveUi = OpenSaveUi;
            
            GM = FindObjectOfType<GameManager>();
            SM = FindObjectOfType<SaveManager>();

            if(GetComponent<Ui_Utilities>() == null)
                gameObject.AddComponent<Ui_Utilities>();
            
            uiUtilities = GetComponent<Ui_Utilities>();
            
            BackButton.onClick.AddListener(() => { Destroy(gameObject); });
            
            if (OpenSaveUi)
            {
                Save_Load_Text.text = "Save Game";
                
                AddNewSaveGameSlot();
                ListAllSaveGame(SelectModeSaveGame.overwrite);
            }
            else
            {
                Save_Load_Text.text = "Load Game";
                ListAllSaveGame(SelectModeSaveGame.read);
                //print(OpenSaveUi);
            }
        }

        private void AddNewSaveGameSlot()
        {
            uiUtilities.UI_Elemant = m_Button_SaveLoad;
            uiUtilities.Add_Button_Element_To_List_View(List_Content, () =>
            {
                uiUtilities.CreateInputUI(InputUI, arg0 =>
                    {
                        OnChangeSaveGameFile(arg0);
                    }, 
                    () =>
                    {
                        foreach (Transform child in List_Content.transform)
                        {
                            Destroy(child.gameObject);
                        }

                        AddNewSaveGameSlot();
                        SaveGame(NameSaveGame_File);
                        ListAllSaveGame(SelectModeSaveGame.overwrite);
                    }, "Name your save file :", "Save Game", "Cancel");
            }, "New Save Game", 0);
        }
        private void ListAllSaveGame(SelectModeSaveGame savemode)
        {
            SaveManager SM = FindObjectOfType<SaveManager>();
            var SaveGameList = SM.GetAllSaveGame(isDefaultPath: true);
            
            foreach (var SG in SaveGameList)
            {
                string textbutton = SG.Name.Split('.')[0];
                uiUtilities.UI_Elemant = m_Button_SaveLoad;
                GameObject m_button = uiUtilities.Add_Button_Element_To_List_View(List_Content, () =>
                {
                    switch (savemode)
                    {
                        case SelectModeSaveGame.overwrite:
                            uiUtilities.CreateMessageUI(MessageBoxUI,
                                () =>
                                { 
                                    SaveGame(textbutton);
                                }, "Do you want to overwrite this game save file?", "Yes", "Cancel");
                            break;
                        case SelectModeSaveGame.read:
                            uiUtilities.CreateMessageUI(MessageBoxUI,
                                () =>
                                { 
                                    LoadSave(textbutton);
                                }, "Do you want to load a save game?", "Yes", "Cancel");
                            break;
                        default:
                            break;
                    }
                }, textbutton, 0);
                
                uiUtilities.UI_Elemant = m_Button_Delete;
                uiUtilities.Add_Button_Element_To_List_View(m_button, () =>
                {
                    uiUtilities.CreateMessageUI(MessageBoxUI,
                        () =>
                        { 
                            DeleteSaveGame(textbutton);
                            
                            foreach (Transform child in List_Content.transform)
                            {
                                Destroy(child.gameObject);
                            }
                            
                            if (IsOpenSaveUi)
                            {
                                Save_Load_Text.text = "Save Game";
                
                                AddNewSaveGameSlot();
                                ListAllSaveGame(SelectModeSaveGame.overwrite);
                            }
                            else
                            {
                                Save_Load_Text.text = "Load Game";
                                ListAllSaveGame(SelectModeSaveGame.read);
                            }
                        }, "Do you want to delete this game save file?", "Yes", "Cancel");
                }, textbutton, 0);
            }
        }

        public void LoadSave(string fileName)
        {
            GamePreferencesData GPD = new GamePreferencesData();
            GPD = SM.LoadGamePreferencesData(Application.persistentDataPath + "/" + fileName);

            GM.GetSetGameManager = GPD.GameDataSave;
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
                GamePreferencesData GPD = new GamePreferencesData
                {
                    GameDataSave = GM.GetSetGameManager
                };

                SM.SaveGamePreferencesData(GPD, Application.persistentDataPath + "/" + fileName);
                NameSaveGame_File = null;
            }
        }

        private void DeleteSaveGame(string fileName)
        {
            File.Delete(Application.persistentDataPath + "/" + fileName + ".json");
        }
    }
}
