﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private GameObject m_Top_Bar_button_Panel;
        [SerializeField] private GameObject m_Button_Bottom_Bar_List;
        [SerializeField] private GameObject m_Information_content;
        [SerializeField] private List<GameObject> m_Prefab_Button = new List<GameObject>();
        [SerializeField] private List<GameObject> m_Prefab_Building_information = new List<GameObject>();
        
        private List<Building_Setting_UI_Data> _buildingSettingUIDatas = new List<Building_Setting_UI_Data>();
        private List<Building_Information_UI_Data> _buildingStatusDatas = new List<Building_Information_UI_Data>();
        private List<Building_Information_UI_Data> _buildingInformationDatas = new List<Building_Information_UI_Data>();
        private string _buildingName_text;
        private Sprite _Building_Icon;
        private GameObject horizontal_group;
        private Dictionary<GameObject, Building_Setting_UI_Data> List_Button_Centor_Panels = new();
        private List<GameObject> List_Button_Bottom_Bars = new List<GameObject>();
        private Building_Information_Type buildingInformationType = Building_Information_Type.ShowStatus;

        public List<Building_Setting_UI_Data> buildingSettingUIDatas
        {
            get => _buildingSettingUIDatas;
            set => _buildingSettingUIDatas = value;
        }

        public List<Building_Information_UI_Data> buildingStatusDatas
        {
            get => _buildingStatusDatas;
            set => _buildingStatusDatas = value;
        }

        public List<Building_Information_UI_Data> buildingInformationDatas
        {
            get => _buildingInformationDatas;
            set => _buildingInformationDatas = value;
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

            m_Top_Bar_button_Panel.GetComponent<Button_Switch_Tab_Animation_Control>().OnSwitchTab(0);
            int i = 0;
            foreach (var BS_ui in _buildingSettingUIDatas)
            {
                CreateButton(m_Prefab_Button, BS_ui, BS_ui.buildingSystemScript.GetValueBuilingSetting(i));
                //print("Object Type : ");
                i++;
            }

            Create_Building_Information();
        }

        private void Update()
        {
            Update_Button_Data();
        }

        private void Update_Button_Data()
        {
            int i = 0;
            foreach (var button in List_Button_Centor_Panels)
            {
                Canvas_Element_List canvasElementList = button.Key.GetComponent<Canvas_Element_List>();
                Building_System_Script buildingSystemScript = button.Value.buildingSystemScript;
                
                Tuple<float, float> building_value;
                try
                {
                    building_value = buildingSystemScript.GetValueBuilingSetting(i).ConvertTo<Tuple<float, float>>();
                }
                catch (Exception e)
                {
                    building_value = new Tuple<float, float>(0, 0);
                }
                
                if (button.Value.buildingSettingType == Building_Setting_Type.Centor_Button_with_progress)
                {
                    if (buildingSystemScript.active)
                    {
                        canvasElementList.tests[0].text = button.Value.text_enable + Environment.NewLine + building_value.Item1 + "/" + building_value.Item2;
                        
                        if(canvasElementList.animators[0] != null)
                            canvasElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        canvasElementList.tests[0].text = button.Value.text_disable + Environment.NewLine + building_value.Item1 + "/" + building_value.Item2;
                        
                        if(canvasElementList.animators[0] != null)
                            canvasElementList.animators[0].SetBool("IsStart", false);
                    }
                    
                    //print("Amount : " + ((float)buildingSystemScript.people / (float)buildingSystemScript.people_Max));
                    canvasElementList.image[2].fillAmount = building_value.Item1 / building_value.Item2;
                }
                else if (button.Value.buildingSettingType == Building_Setting_Type.Centor_Button_only)
                {
                    if (buildingSystemScript.GetValueBuilingSetting(i).ConvertTo<bool>() && buildingSystemScript.active)
                    {
                        canvasElementList.tests[0].text = button.Value.text_enable;
                        canvasElementList.image[0].sprite = button.Value.icon_enable;
                        
                        if(canvasElementList.animators[0] != null)
                            canvasElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        canvasElementList.tests[0].text = button.Value.text_disable;
                        canvasElementList.image[0].sprite = button.Value.icon_disable;
                        
                        if(canvasElementList.animators[0] != null)
                            canvasElementList.animators[0].SetBool("IsStart", false);
                    }
                }

                i++;
            }
        }

        private void CreateButton(List<GameObject> button, Building_Setting_UI_Data buildingSettingUIData, object building_object)
        {
            Tuple<float, float> building_value;
            try
            {
                building_value = building_object.ConvertTo<Tuple<float, float>>();
            }
            catch (Exception e)
            {
                building_value = new Tuple<float, float>(0, 0);
            }
            
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
            
            switch(buildingSettingUIData.buildingSettingType)
            {
                case Building_Setting_Type.Centor_Button_only:
                    GameObject button_element = Instantiate(button[0]);
                    Canvas_Element_List canvasElementList = button_element.GetComponent<Canvas_Element_List>();
                    List_Button_Centor_Panels.Add(button_element, buildingSettingUIData);
                    button_element.transform.parent = horizontal_group.transform;
                    canvasElementList.buttons[0].onClick.AddListener(buildingSettingUIData.actions[0]);

                    if (building_object.ConvertTo<bool>() && buildingSystemScript.active)
                    {
                        canvasElementList.image[0].sprite = buildingSettingUIData.icon_enable;
                        canvasElementList.tests[0].text = buildingSettingUIData.text_enable;
                        
                        if(canvasElementList.animators[0] != null)
                            canvasElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        canvasElementList.image[0].sprite = buildingSettingUIData.icon_disable;
                        canvasElementList.tests[0].text = buildingSettingUIData.text_disable;
                        
                        if(canvasElementList.animators[0] != null)
                            canvasElementList.animators[0].SetBool("IsStart", false);
                    }

                    SetRectTransform(button_element);
                    
                    break;
                case Building_Setting_Type.Centor_Button_with_progress:
                    GameObject progress_button_element = Instantiate(button[1]);
                    Canvas_Element_List progressCanvasElementList = progress_button_element.GetComponent<Canvas_Element_List>();
                    List_Button_Centor_Panels.Add(progress_button_element, buildingSettingUIData);
                    progress_button_element.transform.parent = horizontal_group.transform;
                    progressCanvasElementList.buttons[0].onClick.AddListener(buildingSettingUIData.actions[0]);
                    progressCanvasElementList.buttons[1].onClick.AddListener(buildingSettingUIData.actions[1]);
                    
                    if (buildingSystemScript.active)
                    {
                        progressCanvasElementList.tests[0].text = buildingSettingUIData.text_enable + Environment.NewLine + building_value.Item1 + "/" + building_value.Item2;
                        
                        if(progressCanvasElementList.animators[0] != null)
                            progressCanvasElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        progressCanvasElementList.tests[0].text = buildingSettingUIData.text_disable + Environment.NewLine + building_value.Item1 + "/" + building_value.Item2;
                        
                        if(progressCanvasElementList.animators[0] != null)
                            progressCanvasElementList.animators[0].SetBool("IsStart", false);
                    }
                    
                    progressCanvasElementList.image[2].fillAmount = building_value.Item1 / building_value.Item2;
                    
                    SetRectTransform(progress_button_element);
                    break;
                case Building_Setting_Type.Bottom_Button_only:
                    GameObject bar_button_element = Instantiate(button[2]);
                    Canvas_Element_List barCanvasElementList = bar_button_element.GetComponent<Canvas_Element_List>();
                    List_Button_Bottom_Bars.Add(bar_button_element);
                    bar_button_element.transform.parent = m_Button_Bottom_Bar_List.transform;
                    Button element = barCanvasElementList.buttons[0];
                    element.onClick.AddListener(buildingSettingUIData.actions[0]);
                    
                    ColorBlock colorBlock = new ColorBlock();
                    colorBlock.normalColor = buildingSettingUIData.dark_Color;
                    colorBlock.highlightedColor = buildingSettingUIData.light_Color;
                    colorBlock.pressedColor = buildingSettingUIData.dark_Color;
                    colorBlock.selectedColor = buildingSettingUIData.light_Color;
                    colorBlock.disabledColor = Color.black;
                    element.colors = colorBlock;
                    
                    if (building_object.ConvertTo<bool>())
                    {
                        /*
                        colorBlock.normalColor = new Color(100, 0, 0, 200);
                        colorBlock.highlightedColor = new Color(150, 0, 0, 240);
                        colorBlock.pressedColor = new Color(100, 0, 0, 200);
                        colorBlock.selectedColor = new Color(150, 0, 0, 240);
                        */
                        
                        if(barCanvasElementList.animators[0] != null)
                            barCanvasElementList.animators[0].SetBool("IsStart", true);
                    }
                    else
                    {
                        if(barCanvasElementList.animators[0] != null)
                            barCanvasElementList.animators[0].SetBool("IsStart", false);
                    }
                    
                    SetRectTransform(bar_button_element);
                    break;
                default:
                    break;
            }
        }

        public void OnSwitchPageBuildingInformation(int InformationType)
        {
            buildingInformationType = (Building_Information_Type)InformationType;
            Create_Building_Information();
        }
        
        private void Create_Building_Information()
        {
            for (int i = 0; i < m_Information_content.transform.childCount; i++)
            {
                Destroy(m_Information_content.transform.GetChild(i).gameObject);
            }
            
            if (buildingInformationType == Building_Information_Type.ShowStatus)
            {
                foreach (var BSD_ui in _buildingStatusDatas)
                {
                    GameObject element = Instantiate(m_Prefab_Building_information[0], m_Information_content.transform);
                    Canvas_Element_List elementList = element.GetComponent<Canvas_Element_List>();
                    elementList.tests[0].text = BSD_ui.title;
                    elementList.tests[1].text = BSD_ui.text;
                    elementList.image[0].fillAmount = BSD_ui.value / BSD_ui.max_value;
                }
            }
            else
            {
                foreach (var BIF_ui in _buildingInformationDatas)
                {
                    GameObject element = Instantiate(m_Prefab_Building_information[1], m_Information_content.transform);
                    Canvas_Element_List elementList = element.GetComponent<Canvas_Element_List>();
                    elementList.tests[0].text = BIF_ui.title;
                    elementList.tests[1].text = BIF_ui.text;
                }
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