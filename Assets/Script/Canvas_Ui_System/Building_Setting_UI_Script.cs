using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class Building_Setting_UI_Script : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_BuildingNameText;
        [SerializeField] private Image m_BuildingIcon;
        [SerializeField] private GameObject m_Button_Centor_Panel_List;
        [SerializeField] private GameObject m_Content_Bottom_Panel;
        [SerializeField] private GameObject m_Button_Bottom_Bar_List;

        [SerializeField] private List<GameObject> m_Prefab_Button = new List<GameObject>();
        private List<Building_Setting_UI_Data> _buildingSettingUIDatas;
        private string _buildingName_text;
        private Sprite _Building_Icon;
        private GameObject horizontal_group;
        private Dictionary<GameObject, Building_Setting_UI_Data> List_Button_Centor_Panels = new();
        private List<GameObject> List_Button_Bottom_Bars = new List<GameObject>();

        public List<Building_Setting_UI_Data> buildingSettingUIDatas
        {
            get => _buildingSettingUIDatas;
            set => _buildingSettingUIDatas = value;
        }

        public string buildingName_text
        {
            get => _buildingName_text;
            set => _buildingName_text = value;
        }

        public Sprite building_Icon
        {
            get => _Building_Icon;
            set => _Building_Icon = value;
        }

        private void Start()
        {
            m_BuildingNameText.text = _buildingName_text;
            m_BuildingIcon.sprite = _Building_Icon;

            foreach (var BS_ui in _buildingSettingUIDatas)
            {
                CreateButton(m_Prefab_Button, BS_ui);
            }
        }

        private void Update()
        {
            foreach (var button in List_Button_Centor_Panels)
            {
                Button_Element_List buttonElementList = button.Key.GetComponent<Button_Element_List>();
                Building_System_Script buildingSystemScript = button.Value.buildingSystemScript;
                
                if (button.Value.buildingSettingButton == Building_Setting_Button.Centor_Button_with_progress)
                {
                    if (buildingSystemScript.active)
                    {
                        buttonElementList.tests[0].text = button.Value.text_enable + Environment.NewLine + buildingSystemScript.people + "/" + buildingSystemScript.people_Max;
                        
                        if(buttonElementList.animators[0] != null)
                            buttonElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        buttonElementList.tests[0].text = button.Value.text_disable + Environment.NewLine + buildingSystemScript.people + "/" + buildingSystemScript.people_Max;
                        
                        if(buttonElementList.animators[0] != null)
                            buttonElementList.animators[0].SetBool("IsStart", false);
                    }
                    
                    //print("Amount : " + ((float)buildingSystemScript.people / (float)buildingSystemScript.people_Max));
                    buttonElementList.image[2].fillAmount = (float)buildingSystemScript.people / (float)buildingSystemScript.people_Max;
                }
                else if (button.Value.buildingSettingButton == Building_Setting_Button.Centor_Button_only)
                {
                    if (buildingSystemScript.active)
                    {
                        buttonElementList.tests[0].text = button.Value.text_enable;
                        buttonElementList.image[0].sprite = button.Value.icon_enable;
                        
                        if(buttonElementList.animators[0] != null)
                            buttonElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        buttonElementList.tests[0].text = button.Value.text_disable;
                        buttonElementList.image[0].sprite = button.Value.icon_disable;
                        
                        if(buttonElementList.animators[0] != null)
                            buttonElementList.animators[0].SetBool("IsStart", false);
                    }
                }
            }
        }

        private void CreateButton(List<GameObject> button, Building_Setting_UI_Data buildingSettingUIData)
        {
            if (List_Button_Centor_Panels.Count % 2 == 0)
            {
                horizontal_group = new GameObject("Horizontal Group");
                horizontal_group.transform.parent = m_Button_Centor_Panel_List.transform;
                HorizontalLayoutGroup horizontalLayoutGroup = horizontal_group.AddComponent<HorizontalLayoutGroup>();
                LayoutElement layoutElement = horizontal_group.AddComponent<LayoutElement>();
                horizontalLayoutGroup.childControlWidth = true;
                horizontalLayoutGroup.childControlHeight = true;
                horizontalLayoutGroup.spacing = 5;
                
                SetRectTransform(horizontal_group);
            }
            
            Building_System_Script buildingSystemScript = buildingSettingUIData.buildingSystemScript;
            
            switch(buildingSettingUIData.buildingSettingButton)
            {
                case Building_Setting_Button.Centor_Button_only:
                    GameObject button_element = Instantiate(button[0]);
                    Button_Element_List buttonElementList = button_element.GetComponent<Button_Element_List>();
                    List_Button_Centor_Panels.Add(button_element, buildingSettingUIData);
                    button_element.transform.parent = horizontal_group.transform;
                    buttonElementList.buttons[0].onClick.AddListener(buildingSettingUIData.actions[0]);

                    if (buildingSystemScript.active)
                    {
                        buttonElementList.image[0].sprite = buildingSettingUIData.icon_enable;
                        buttonElementList.tests[0].text = buildingSettingUIData.text_enable;
                        
                        if(buttonElementList.animators[0] != null)
                            buttonElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        buttonElementList.image[0].sprite = buildingSettingUIData.icon_disable;
                        buttonElementList.tests[0].text = buildingSettingUIData.text_disable;
                        
                        if(buttonElementList.animators[0] != null)
                            buttonElementList.animators[0].SetBool("IsStart", false);
                    }

                    SetRectTransform(button_element);
                    
                    break;
                case Building_Setting_Button.Centor_Button_with_progress:
                    GameObject progress_button_element = Instantiate(button[1]);
                    Button_Element_List progress_buttonElementList = progress_button_element.GetComponent<Button_Element_List>();
                    List_Button_Centor_Panels.Add(progress_button_element, buildingSettingUIData);
                    progress_button_element.transform.parent = horizontal_group.transform;
                    progress_buttonElementList.buttons[0].onClick.AddListener(buildingSettingUIData.actions[0]);
                    progress_buttonElementList.buttons[1].onClick.AddListener(buildingSettingUIData.actions[1]);
                    
                    if (buildingSystemScript.active)
                    {
                        progress_buttonElementList.tests[0].text = buildingSettingUIData.text_enable + Environment.NewLine + buildingSystemScript.people + "/" + buildingSystemScript.people_Max;
                        
                        if(progress_buttonElementList.animators[0] != null)
                            progress_buttonElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        progress_buttonElementList.tests[0].text = buildingSettingUIData.text_disable + Environment.NewLine + buildingSystemScript.people + "/" + buildingSystemScript.people_Max;
                        
                        if(progress_buttonElementList.animators[0] != null)
                            progress_buttonElementList.animators[0].SetBool("IsStart", false);
                    }
                    
                    progress_buttonElementList.image[2].fillAmount = (float)buildingSystemScript.people / (float)buildingSystemScript.people_Max;
                    
                    SetRectTransform(progress_button_element);
                    break;
                case Building_Setting_Button.Bottom_Button_only:
                    GameObject bar_button_element = Instantiate(button[2]);
                    Button_Element_List bar_buttonElementList = bar_button_element.GetComponent<Button_Element_List>();
                    List_Button_Bottom_Bars.Add(bar_button_element);
                    bar_button_element.transform.parent = m_Button_Bottom_Bar_List.transform;
                    Button element = bar_buttonElementList.buttons[0];
                    element.onClick.AddListener(buildingSettingUIData.actions[0]);
                    
                    ColorBlock colorBlock = new ColorBlock();
                    colorBlock.normalColor = buildingSettingUIData.dark_Color;
                    colorBlock.highlightedColor = buildingSettingUIData.light_Color;
                    colorBlock.pressedColor = buildingSettingUIData.dark_Color;
                    colorBlock.selectedColor = buildingSettingUIData.light_Color;
                    colorBlock.disabledColor = Color.black;
                    element.colors = colorBlock;
                    
                    if (buildingSystemScript.active)
                    {
                        /*
                        colorBlock.normalColor = new Color(100, 0, 0, 200);
                        colorBlock.highlightedColor = new Color(150, 0, 0, 240);
                        colorBlock.pressedColor = new Color(100, 0, 0, 200);
                        colorBlock.selectedColor = new Color(150, 0, 0, 240);
                        */
                        
                        if(bar_buttonElementList.animators[0] != null)
                            bar_buttonElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        if(bar_buttonElementList.animators[0] != null)
                            bar_buttonElementList.animators[0].SetBool("IsStart", false);
                    }
                    
                    SetRectTransform(bar_button_element);
                    break;
                default:
                    break;
            }
        }

        private void SetRectTransform(GameObject element)
        {
            RectTransform HG_recttransform = element.GetComponent<RectTransform>();
            HG_recttransform.anchoredPosition = Vector2.zero;
            HG_recttransform.localPosition = Vector3.zero;
            HG_recttransform.localRotation = Quaternion.Euler(Vector3.zero);
            HG_recttransform.localScale = new Vector3(1, 1, 1);
        }
    }
}