using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class TechnologyUpgradeUI_Script : MonoBehaviour
    {
        [SerializeField] private int level;
        [SerializeField] private Vector2Int select;
        [SerializeField] private TechnologyUpgrade TU;
        [SerializeField] private Canvas_Element_List m_level_button;

        private ResourcesManager RM;
        
        int element_level;
        int index;

        private void Start()
        {
            RM = ResourcesManager.Instance;
            
            element_level = select.x;
            index = select.y;
            print("x :" + select.x + " y : " + select.y);
            m_level_button.texts[0].text = TU.TU_Details_Preset.lists[element_level - 1].details[index - 1].name;
            m_level_button.texts[1].text = level.ToString();
            m_level_button.image[0].enabled = TU.get_dataSave(element_level, index);
        }

        private void Update()
        {
            m_level_button.buttons[0].interactable = TU.get_current_level >= element_level;
            m_level_button.image[0].enabled = TU.get_dataSave(element_level, index);
        }

        public void CreateUI_Message()
        {
            if (!TU.get_dataSave(element_level, index))
            {
                Ui_Utilities _uiUtilities;

                if (GetComponent<Ui_Utilities>() == null)
                {
                    _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
                }
                else
                {
                    _uiUtilities = GetComponent<Ui_Utilities>();
                }

                _uiUtilities.canvasUI = TU.get_message_ui;
                _uiUtilities.useCameraOverlay = true;
                _uiUtilities.planeDistance = 0.5f;
                _uiUtilities.order_in_layer = 12;
                GameObject m_message_button = _uiUtilities.CreateMessageUI(() =>
                    {
                        int token = RM.Get_Resources_Token() - TU.get_preset_data(element_level, index);
                        if (RM.Can_Set_Resources_Token(token))
                        {
                            RM.Set_Resources_Token(-TU.get_preset_data(element_level, index));
                            TU.Level_UP(element_level, index);
                        }
                    },
                    () =>
                    {
                        print("Remove");
                    },
                    TU.TU_Details_Preset.lists[element_level - 1].details[index - 1].title,
                    TU.TU_Details_Preset.lists[element_level - 1].details[index - 1].message + " ต้องการ Token จำนวน " +
                    TU.get_preset_data(element_level, index), "Upgrade", "Cancel", false);

                Destroy(m_message_button.GetComponent<Back_UI_Button_Script>());
                m_message_button.GetComponent<Animator>().SetBool("IsStart", true);
            }
        }
    }
}