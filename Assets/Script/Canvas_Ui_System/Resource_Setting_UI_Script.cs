using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class Resource_Setting_UI_Script : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_ResourceNameText;
        [SerializeField] private Image m_ResourceIcon;
        [SerializeField] private GameObject m_Button_Centor_Panel_List;
        [SerializeField] private GameObject m_button_group_setting;
        [SerializeField] private GameObject m_Content_Bottom_Panel;
        [SerializeField] private GameObject m_Top_Bar_button_Panel;
        [SerializeField] private GameObject m_Button_Bottom_Bar_List;
        [SerializeField] private GameObject m_Information_content;
        [SerializeField] private List<GameObject> m_Prefab_Button = new List<GameObject>();
        [SerializeField] private List<GameObject> m_Prefab_Resource_information = new List<GameObject>();
        
        private List<Resource_Setting_UI_Data> _resourceSettingUIDatas = new ();
        private List<Resource_Information_UI_Data> _resourceStatusDatas = new ();
        private List<Resource_Information_UI_Data> _resourceInformationDatas = new ();
        private List<Button_Action_Data> _buttonActionDatas = new List<Button_Action_Data>();
        private string _resourceNameText;
        private Sprite _resourceIcon;
        private GameObject horizontal_group;
        private Static_Object_Resource_System_Script _staticObjectResourceSystemScript;
        private Dictionary<GameObject, Resource_Setting_UI_Data> List_Button_Centor_Panels = new();
        private Dictionary<GameObject, Resource_Information_UI_Data> list_status_elements = new();
        private Dictionary<GameObject, Button_Action_Data> list_button_action_bottoms = new();
        private List<GameObject> List_Button_Bottom_Bars = new List<GameObject>();
        private Building_Information_Type buildingInformationType = Building_Information_Type.ShowStatus;
        private Animator m_animator;

        public Static_Object_Resource_System_Script resourceSystemScript
        {
            get => _staticObjectResourceSystemScript;
            set => _staticObjectResourceSystemScript = value;
        }
        
        public List<Resource_Setting_UI_Data> resourceSettingUIDatas
        {
            get => _resourceSettingUIDatas;
            set
            {
                _resourceSettingUIDatas = value;
            }
        }

        public List<Resource_Information_UI_Data> resourceStatusDatas
        {
            get => _resourceStatusDatas;
            set => _resourceStatusDatas = value;
        }

        public List<Resource_Information_UI_Data> resourceInformationDatas
        {
            get => _resourceInformationDatas;
            set => _resourceInformationDatas = value;
        }

        public List<Button_Action_Data> resourceButtonActionDatas
        {
            get => _buttonActionDatas;
            set => _buttonActionDatas = value;
        }
        
        public TextMeshProUGUI resourceNameText
        {
            set => m_ResourceNameText = value;
        }

        public Image resourceIcon
        {
            set => m_ResourceIcon = value;
        }

        public GameObject button_Centor_Panel_List
        {
            set => m_Button_Centor_Panel_List = value;
        }

        public GameObject button_group_setting
        {
            set => m_button_group_setting = value;
        }
        public GameObject content_Bottom_Panel
        {
            set => m_Content_Bottom_Panel = value;
        }

        public GameObject top_Bar_button_Panel
        {
            set => m_Top_Bar_button_Panel = value;
        }

        public GameObject button_Bottom_Bar_List
        {
            set => m_Button_Bottom_Bar_List = value;
        }

        public GameObject information_content
        {
            set => m_Information_content = value;
        }

        public List<GameObject> prefab_Button
        {
            get => m_Prefab_Button;
            set => m_Prefab_Button = value;
        }

        public List<GameObject> prefab_Resource_information
        {
            get => m_Prefab_Resource_information;
            set => m_Prefab_Resource_information = value;
        }
        
        public string resourceName_text
        {
            get => _resourceNameText;
            set => _resourceNameText = value;
        }

        public Sprite resource_Icon
        {
            get => _resourceIcon;
            set => _resourceIcon = value;
        }

        private void Start()
        {
            m_ResourceNameText.text = _resourceNameText;
            m_ResourceIcon.sprite = _resourceIcon;

            m_animator = GetComponent<Animator>();

            m_Top_Bar_button_Panel.GetComponent<Button_Switch_Tab_Animation_Control>().OnSwitchTab(0);
            int i = 0;
            foreach (var BS_ui in _resourceSettingUIDatas)
            {
                CreateButton(m_Prefab_Button, BS_ui, BS_ui.resourceSystemScript.GetValueBuilingSetting(i));
                //print("Object Type : ");
                i++;
            }

            CreateButtonBottomBar();
            Create_Building_Information();
        }

        private void Update()
        {
            Update_Button_Data();
            UpdateInfomationData();

            if (_staticObjectResourceSystemScript == null)
            {
                m_animator.SetBool("IsStart", false);
            }
        }

        private void Update_Button_Data()
        {
            if (List_Button_Centor_Panels.Count > 0)
            {
                int i = 0;
                foreach (var button in List_Button_Centor_Panels)
                {
                    Canvas_Element_List canvasElementList = button.Key.GetComponent<Canvas_Element_List>();

                    Tuple<float, float> resource_value;
                    try
                    {
                        resource_value = resourceSystemScript.GetValueBuilingSetting(i).ConvertTo<Tuple<float, float>>();
                    }
                    catch (Exception e)
                    {
                        resource_value = new Tuple<float, float>(0, 0);
                    }

                    if (button.Value.buildingSettingType == Building_Setting_Type.Centor_Button_with_progress)
                    {
                        canvasElementList.texts[0].text = button.Value.text_enable + Environment.NewLine +
                                                          resource_value.Item1 + "/" + resource_value.Item2;

                        if (canvasElementList.animators[0] != null)
                        {
                            if (resource_value.Item1 > 0)
                            {
                                canvasElementList.animators[0].SetBool("IsStart", true);
                            }
                            else
                            {
                                canvasElementList.animators[0].SetBool("IsStart", false);
                            }
                        }
                        
                        //print("Amount : " + ((float)buildingSystemScript.people / (float)buildingSystemScript.people_Max));
                        canvasElementList.image[2].fillAmount = resource_value.Item1 / resource_value.Item2;
                    }
                    else if (button.Value.buildingSettingType == Building_Setting_Type.Centor_Button_only)
                    {
                        canvasElementList.texts[0].text = button.Value.text_enable;
                        canvasElementList.image[0].sprite = button.Value.icon_enable;

                        if (canvasElementList.animators[0] != null)
                        {
                            //print("IOOUOUO : " + i);
                            if (resourceSystemScript.GetValueBuilingSetting(i).ConvertTo<bool>())
                            {
                                canvasElementList.animators[0].SetBool("IsStart", true);
                            }
                            else
                            {
                                canvasElementList.animators[0].SetBool("IsStart", false);
                            }
                        }
                    }

                    i++;
                }
            }
            
            if (list_button_action_bottoms.Count > 0)
            {
                int i_BAD = 0;
                foreach (KeyValuePair<GameObject, Button_Action_Data> buttonActionData in list_button_action_bottoms)
                {
                    Canvas_Element_List _element = buttonActionData.Key.GetComponent<Canvas_Element_List>();
                    _element.buttons[0].onClick
                        .AddListener(_staticObjectResourceSystemScript.GetUpdateButtonAction(i_BAD).unityAction);
                    _element.buttons[0].colors = _staticObjectResourceSystemScript.GetUpdateButtonAction(i_BAD).colorBlock;
                    _element.image[0].sprite = _staticObjectResourceSystemScript.GetUpdateButtonAction(i_BAD).sprite;

                    i_BAD++;
                }
            }
        }

        private void UpdateInfomationData()
        {
            int i = 0;

            if (list_status_elements.Count > 0 && _staticObjectResourceSystemScript != null)
            {
                foreach (var status in list_status_elements)
                {
                    if (status.Value.buildingInformationType == Building_Information_Type.ShowStatus &&
                        status.Key != null)
                    {
                        if (_staticObjectResourceSystemScript.GetValueBuildingInformation(i).Item3 != null)
                        {
                            status.Key.SetActive(true);
                            Canvas_Element_List elementList = status.Key.GetComponent<Canvas_Element_List>();
                            elementList.texts[1].text = _staticObjectResourceSystemScript.GetValueBuildingInformation(i).Item3;
                            elementList.image[0].fillAmount =
                                (float)_staticObjectResourceSystemScript.GetValueBuildingInformation(i).Item1 /
                                (float)_staticObjectResourceSystemScript.GetValueBuildingInformation(i).Item2;
                        }
                        else
                        {
                            status.Key.SetActive(false);
                        }

                        i++;
                    }
                }
            }
        }

        private void CreateButton(List<GameObject> button, Resource_Setting_UI_Data resourceSettingUIData, object building_object)
        {
            Tuple<float, float> resource_value;
            try
            {
                resource_value = building_object.ConvertTo<Tuple<float, float>>();
            }
            catch (Exception e)
            {
                resource_value = new Tuple<float, float>(0, 0);
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

            switch (resourceSettingUIData.buildingSettingType)
            {
                case Building_Setting_Type.Centor_Button_only:
                    GameObject button_element = Instantiate(button[0]);
                    Canvas_Element_List canvasElementList = button_element.GetComponent<Canvas_Element_List>();
                    List_Button_Centor_Panels.Add(button_element, resourceSettingUIData);
                    button_element.transform.parent = horizontal_group.transform;
                    canvasElementList.buttons[0].onClick.AddListener(resourceSettingUIData.actions[0]);
                    canvasElementList.image[0].sprite = resourceSettingUIData.icon_enable;
                    canvasElementList.texts[0].text = resourceSettingUIData.text_enable;

                    if (canvasElementList.animators[0] != null && building_object.ConvertTo<bool>())
                        canvasElementList.animators[0].SetBool("IsStart", true);

                    SetRectTransform(button_element);

                    break;
                case Building_Setting_Type.Centor_Button_with_progress:
                    GameObject progress_button_element = Instantiate(button[1]);
                    Canvas_Element_List progressCanvasElementList =
                        progress_button_element.GetComponent<Canvas_Element_List>();
                    List_Button_Centor_Panels.Add(progress_button_element, resourceSettingUIData);
                    progress_button_element.transform.parent = horizontal_group.transform;
                    progressCanvasElementList.buttons[0].onClick.AddListener(resourceSettingUIData.actions[0]);
                    progressCanvasElementList.buttons[1].onClick.AddListener(resourceSettingUIData.actions[1]);

                    progressCanvasElementList.texts[0].text = resourceSettingUIData.text_enable + Environment.NewLine +
                                                              resource_value.Item1 + "/" + resource_value.Item2;

                    if (progressCanvasElementList.animators[0] != null && resource_value.Item1 > 0)
                        progressCanvasElementList.animators[0].SetBool("IsStart", true);


                    progressCanvasElementList.image[2].fillAmount = resource_value.Item1 / resource_value.Item2;

                    SetRectTransform(progress_button_element);
                    break;
                case Building_Setting_Type.Bottom_Button_only:
                    GameObject bar_button_element = Instantiate(button[2]);
                    Canvas_Element_List barCanvasElementList = bar_button_element.GetComponent<Canvas_Element_List>();
                    List_Button_Bottom_Bars.Add(bar_button_element);
                    bar_button_element.transform.parent = m_Button_Bottom_Bar_List.transform;
                    Button element = barCanvasElementList.buttons[0];
                    element.onClick.AddListener(resourceSettingUIData.actions[0]);

                    ColorBlock colorBlock = new ColorBlock();
                    colorBlock.normalColor = resourceSettingUIData.dark_Color;
                    colorBlock.highlightedColor = resourceSettingUIData.light_Color;
                    colorBlock.pressedColor = resourceSettingUIData.dark_Color;
                    colorBlock.selectedColor = resourceSettingUIData.light_Color;
                    colorBlock.disabledColor = Color.black;
                    element.colors = colorBlock;

                    if (barCanvasElementList.animators[0] != null)
                        barCanvasElementList.animators[0].SetBool("IsStart", true);

                    SetRectTransform(bar_button_element);
                    break;
                default:
                    break;
            }
        }
        
        private void CreateButtonBottomBar()
        {
            foreach (Button_Action_Data buttonActionData in _buttonActionDatas)
            {
                GameObject button = Instantiate(m_Prefab_Button[2], m_Button_Bottom_Bar_List.transform);
                list_button_action_bottoms.Add(button, buttonActionData);
                Canvas_Element_List _element = button.GetComponent<Canvas_Element_List>();
                _element.buttons[0].onClick.RemoveAllListeners();
                _element.buttons[0].onClick.AddListener(buttonActionData.unityAction);
                _element.buttons[0].colors = buttonActionData.colorBlock;
                _element.image[0].sprite = buttonActionData.sprite;
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
                list_status_elements = new();
                
                foreach (var BSD_ui in _resourceStatusDatas)
                {
                    GameObject element = Instantiate(m_Prefab_Resource_information[0], m_Information_content.transform);
                    Canvas_Element_List elementList = element.GetComponent<Canvas_Element_List>();
                    list_status_elements.Add(element, BSD_ui);
                    elementList.texts[0].text = BSD_ui.title;
                    elementList.texts[1].text = BSD_ui.text;
                    elementList.image[0].fillAmount = BSD_ui.value / BSD_ui.max_value;
                    
                    
                    print("CSSSDS : " + element.name);
                }
            }
            else
            {
                foreach (var BIF_ui in _resourceInformationDatas)
                {
                    GameObject element = Instantiate(m_Prefab_Resource_information[1], m_Information_content.transform);
                    Canvas_Element_List elementList = element.GetComponent<Canvas_Element_List>();
                    elementList.texts[0].text = BIF_ui.title;
                    elementList.texts[1].text = BIF_ui.text;
                    
                    print("aaassaasS : " + element.name);
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