using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Outline.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class ObjectInteract : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> m_Disable_Component;
        [SerializeField] private GameObject m_MenuUI;
        [SerializeField] private GameObject m_Building_Setting_UI;
        [SerializeField] private GameObject m_Resource_Setting_UI;
        [SerializeField] private GameObject m_Button_list;
        [SerializeField] private GameObject m_BG_Button_list;
        [SerializeField] private GameObject m_GameObjectLayer;
        [SerializeField] private GameObject m_ResourceLayer;
        
        private GameObject old_objecthit;
        private GameObject objectHit;
        private GameObject old_menu = null;
        private GameObject old_buiding_setting_ui = null;
        private LayerMask L_Place_Object;
        private LayerMask L_Resource_Object;
        private bool isOpenMenu = false;

        private void Start()
        {
            L_Place_Object = LayerMask.NameToLayer("Place_Object");
            L_Resource_Object = LayerMask.NameToLayer("Resource_Object");
        }
        private void Update()
        {
            Physics.Raycast(RaycastFromMouse.GetRayFromMouse(), out var hit_objraycast);

            if(Input.GetMouseButtonUp(0) && !PointerOverUIElement.OnPointerOverUIElement())
            {
                DestroyUI(old_buiding_setting_ui);
            }
            
            if (old_objecthit == null)
            {
                DestroyUI(old_buiding_setting_ui);
            }
            
            if (hit_objraycast.transform != null)
            {
                objectHit = hit_objraycast.transform.gameObject;
                
                if ((hit_objraycast.transform.parent == m_GameObjectLayer.transform || hit_objraycast.transform.parent == m_ResourceLayer.transform) && CheckComponentDisable() && !isOpenMenu && !PointerOverUIElement.OnPointerOverUIElement())
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        if(hit_objraycast.transform.parent == m_GameObjectLayer.transform)
                            Create_Buiding_Setting<Building_Setting_UI_Script>(objectHit.GetComponent<Building_System_Script>(), typeof(Building_System_Script));
                        
                        if(hit_objraycast.transform.parent == m_ResourceLayer.transform && objectHit.GetComponent<Static_Object_Resource_System_Script>().is_unlock)
                            Create_Buiding_Setting<Resource_Setting_UI_Script>(objectHit.GetComponent<Static_Object_Resource_System_Script>(), typeof(Static_Object_Resource_System_Script));
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        DestroyUI(old_menu);
                        DestroyUI(old_buiding_setting_ui);
                        
                        if(hit_objraycast.transform.parent == m_GameObjectLayer.transform)
                            CreateMenu<Building_System_Script>();
                        
                        if(hit_objraycast.transform.parent == m_ResourceLayer.transform)
                            CreateMenu<Static_Object_Resource_System_Script>();
                        
                        isOpenMenu = true;
                    }
                }
                else if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && (hit_objraycast.transform.parent != m_GameObjectLayer.transform || hit_objraycast.transform.parent != m_ResourceLayer.transform) && !PointerOverUIElement.OnPointerOverUIElement())
                {
                    isOpenMenu = false;
                    DestroyUI(old_buiding_setting_ui);
                    DestroyUI(old_menu);
                }
                else
                {
                    isOpenMenu = false;
                }

                if (objectHit != null)
                {
                    OnOutlineObject(objectHit, old_objecthit);
                }
                    
                
                if(objectHit.layer == L_Place_Object || objectHit.layer == L_Resource_Object)
                    old_objecthit = objectHit;
            }
            
            if(Input.GetMouseButtonUp(0))
            {
                DestroyUI(old_menu);
                isOpenMenu = false;
            }
        }

        private void CreateMenu<T>()
        {
            GameObject menu_ui = Instantiate(m_MenuUI);
            menu_ui.GetComponent<Animator>().SetBool("IsStart", true);
            menu_ui.GetComponent<RectTransform>().sizeDelta = new Vector2(260, 0);

            List<Button_Action_Data> menuDatas = new();
            if(typeof(T) == typeof(Building_System_Script))
                menuDatas = objectHit.GetComponent<Building_System_Script>().GetInteractAction();
            
            if(typeof(T) == typeof(Static_Object_Resource_System_Script))
                menuDatas = objectHit.GetComponent<Static_Object_Resource_System_Script>().GetInteractAction();
            
            foreach (var data in menuDatas)
            {
                Instantiate(m_BG_Button_list, menu_ui.transform.GetChild(0).GetChild(0));
                GameObject button = Instantiate(m_Button_list, menu_ui.transform.GetChild(0).GetChild(1).GetChild(0));
                
                button.GetComponent<Button>().onClick.AddListener(data.unityAction);
                
                if(button.transform.GetChild(0).GetComponent<Image>() != null)
                    print("dfvds : " + data.sprite);
                button.transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
                button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.text;
            }
            
            old_menu = menu_ui;
            Canvas canvas = menu_ui.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.25f;
            canvas.sortingOrder = 9;

            RectTransform rect_element = menu_ui.transform.GetChild(0).GetComponent<RectTransform>();
            rect_element.anchorMin = new Vector2(0, 0);
            rect_element.anchorMax = new Vector2(0, 0);
            rect_element.pivot = new Vector2(0, 1);
            rect_element.anchoredPosition = Input.mousePosition;
        }

        private void Create_Buiding_Setting<T>(object objectSystemScript, Type script_type)
        {
            Building_System_Script buildingSystemScript = null;
            Static_Object_Resource_System_Script resourceSystemScript = null;

            GameObject setting_ui = Instantiate(m_Building_Setting_UI);
            setting_ui.AddComponent(typeof(T));
            setting_ui.GetComponent<Animator>().SetBool("IsStart", true);
            if (objectSystemScript != null)
            {
                print("Is nottt nuuulllllllllll");
                if (script_type == typeof(Building_System_Script))
                {
                    buildingSystemScript = objectSystemScript.ConvertTo<Building_System_Script>();
                    Set_Building_Setting_UI(buildingSystemScript, setting_ui);
                }

                if (script_type == typeof(Static_Object_Resource_System_Script))
                {
                    resourceSystemScript = objectSystemScript.ConvertTo<Static_Object_Resource_System_Script>();
                    Set_Resource_Setting_UI(resourceSystemScript, setting_ui);
                }
            }
        }

        private void Set_Building_Setting_UI(Building_System_Script buildingSystemScript, GameObject buiding_setting_ui)
        {
            Building_Setting_UI_Script buildingSettingUIScript = buiding_setting_ui.GetComponent<Building_Setting_UI_Script>();
            Canvas_Element_List _element = buiding_setting_ui.GetComponent<Canvas_Element_List>();
            _element.buttons[0].onClick.AddListener((() =>
            {
                print("CHihiiihih 0000");
                buildingSettingUIScript.OnSwitchPageBuildingInformation(0);
            }));
            _element.buttons[1].onClick.AddListener((() =>
            {
                print("CHihiiihih 1111");
                buildingSettingUIScript.OnSwitchPageBuildingInformation(1);
            }));
            
            buildingSettingUIScript.buildingNameText = _element.texts[0];
            buildingSettingUIScript.buildingIcon = _element.image[0];
            buildingSettingUIScript.button_Centor_Panel_List = _element.canvas_gameObjects[0];
            buildingSettingUIScript.button_group_setting = _element.canvas_gameObjects[1];
            buildingSettingUIScript.content_Bottom_Panel = _element.canvas_gameObjects[2];
            buildingSettingUIScript.top_Bar_button_Panel = _element.canvas_gameObjects[3];
            buildingSettingUIScript.button_Bottom_Bar_List = _element.canvas_gameObjects[4];
            buildingSettingUIScript.information_content = _element.canvas_gameObjects[5];

            for (int i = 6; i <= 8; i++)
            {
                buildingSettingUIScript.prefab_Button.Add(_element.canvas_gameObjects[i]);
            }
            
            for (int i = 9; i <= 10; i++)
            {
                buildingSettingUIScript.prefab_Building_information.Add(_element.canvas_gameObjects[i]);
            }
            
            buildingSettingUIScript.buildingSystemScript = buildingSystemScript;
            buildingSettingUIScript.buildingSettingUIDatas = new List<Building_Setting_UI_Data>();
            buildingSettingUIScript.buildingName_text = buildingSystemScript.name;
            buildingSettingUIScript.building_Icon = Resources.Load<Sprite>("Icon/construction");
            buildingSettingUIScript.BuildingButtonActionDatas = buildingSystemScript.buildingButtonActionDatas;
                
            foreach (var actionbuilding in buildingSystemScript.actionsBuilding)
            {
                Create_Button_Building_Data(buildingSettingUIScript, buildingSystemScript, actionbuilding.Key, actionbuilding.Value);
            }
            
            Create_Info_Building_Ui_data(buildingSettingUIScript, buildingSystemScript, Building_Information_Type.ShowStatus);
            Create_Info_Building_Ui_data(buildingSettingUIScript, buildingSystemScript, Building_Information_Type.ShowInformation);
            
            Canvas canvas = buiding_setting_ui.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.5f;
            canvas.sortingOrder = 10;

            old_buiding_setting_ui = buiding_setting_ui;
        }
        
        private void Set_Resource_Setting_UI(Static_Object_Resource_System_Script resourceSystemScript, GameObject buiding_setting_ui)
        {
            Resource_Setting_UI_Script resourceSettingUIScript = buiding_setting_ui.GetComponent<Resource_Setting_UI_Script>();
            Canvas_Element_List _element = buiding_setting_ui.GetComponent<Canvas_Element_List>();
            _element.buttons[0].onClick.AddListener((() =>
            {
                print("CHihiiihih 0000");
                resourceSettingUIScript.OnSwitchPageBuildingInformation(0);
            }));
            _element.buttons[1].onClick.AddListener((() =>
            {
                print("CHihiiihih 1111");
                resourceSettingUIScript.OnSwitchPageBuildingInformation(1);
            }));
            
            resourceSettingUIScript.resourceNameText = _element.texts[0];
            resourceSettingUIScript.resourceIcon = _element.image[0];
            resourceSettingUIScript.button_Centor_Panel_List = _element.canvas_gameObjects[0];
            resourceSettingUIScript.button_group_setting = _element.canvas_gameObjects[1];
            resourceSettingUIScript.content_Bottom_Panel = _element.canvas_gameObjects[2];
            resourceSettingUIScript.top_Bar_button_Panel = _element.canvas_gameObjects[3];
            resourceSettingUIScript.button_Bottom_Bar_List = _element.canvas_gameObjects[4];
            resourceSettingUIScript.information_content = _element.canvas_gameObjects[5];

            for (int i = 6; i <= 8; i++)
            {
                resourceSettingUIScript.prefab_Button.Add(_element.canvas_gameObjects[i]);
            }
            
            for (int i = 9; i <= 10; i++)
            {
                resourceSettingUIScript.prefab_Resource_information.Add(_element.canvas_gameObjects[i]);
            }
            
            resourceSettingUIScript.resourceSystemScript = resourceSystemScript;
            resourceSettingUIScript.resourceSettingUIDatas = new List<Resource_Setting_UI_Data>();
            resourceSettingUIScript.resourceName_text = resourceSystemScript.name;
            resourceSettingUIScript.resource_Icon = Resources.Load<Sprite>("Icon/construction");
            resourceSettingUIScript.resourceButtonActionDatas = resourceSystemScript.buildingButtonActionDatas;
            
            foreach (var actionbuilding in resourceSystemScript.actionsBuilding)
            {
                Create_Button_Resource_Data(resourceSettingUIScript, resourceSystemScript, actionbuilding.Key, actionbuilding.Value);
            }
            
            Create_Info_Resource_Ui_data(resourceSettingUIScript, resourceSystemScript, Building_Information_Type.ShowStatus);
            Create_Info_Resource_Ui_data(resourceSettingUIScript, resourceSystemScript, Building_Information_Type.ShowInformation);
            
            Canvas canvas = buiding_setting_ui.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.5f;
            canvas.sortingOrder = 10;

            old_buiding_setting_ui = buiding_setting_ui;
        }

        private void DestroyUI(GameObject canvas_ui)
        {
            if (canvas_ui != null)
            {
                canvas_ui.GetComponent<Canvas>().sortingOrder = 8;
                Animator animator = canvas_ui.GetComponent<Animator>();
                animator.SetBool("IsStart", false);
                Destroy(canvas_ui, animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }

        private void Create_Info_Building_Ui_data(Building_Setting_UI_Script buildingSettingUIScript, Building_System_Script buildingSystemScript, Building_Information_Type buildingInformationType)
        {
            if (buildingInformationType == Building_Information_Type.ShowStatus)
            {
                int i = 0;
                foreach (var BI_data in buildingSystemScript.buildingInfoStruct.status)
                {
                    if (BI_data.buildingShowMode == Building_Show_mode.TextOnly)
                    {
                        print("IIII Status : " + i);
                        buildingSettingUIScript.buildingStatusDatas.Add(new Building_Information_UI_Data(
                            BI_data.title,
                            buildingSystemScript.GetValueBuildingInformation(i).Item3,
                            0,
                            0,
                            BI_data.buildingInformationType,
                            BI_data.buildingShowMode
                        ));
                    }
                    else
                    {
                        print("IIII Status : " + i);
                        buildingSettingUIScript.buildingStatusDatas.Add(new Building_Information_UI_Data(
                            BI_data.title,
                            buildingSystemScript.GetValueBuildingInformation(i).Item3,
                            buildingSystemScript.GetValueBuildingInformation(i).Item1.ConvertTo<float>(),
                            buildingSystemScript.GetValueBuildingInformation(i).Item2.ConvertTo<float>(),
                            BI_data.buildingInformationType,
                            BI_data.buildingShowMode
                        ));
                    }
                    
                    i++;
                }
            }
            else
            {
                int i = 0;
                foreach (var BI_data in buildingSystemScript.buildingInfoStruct.informations)
                {
                    print("IIII Information : " + i);
                    buildingSettingUIScript.buildingInformationDatas.Add(new Building_Information_UI_Data(
                        BI_data.title,
                        BI_data.text,
                        0,
                        0,
                        BI_data.buildingInformationType,
                        BI_data.buildingShowMode
                    ));
                    i++;
                }
            }
        }
        
        private void Create_Info_Resource_Ui_data(Resource_Setting_UI_Script resourceSettingUIScript, Static_Object_Resource_System_Script resourceSystemScript, Building_Information_Type buildingInformationType)
        {
            if (buildingInformationType == Building_Information_Type.ShowStatus)
            {
                int i = 0;
                foreach (var BI_data in resourceSystemScript.buildingInfoStruct.status)
                {
                    print("IIII : " + i);
                    resourceSettingUIScript.resourceStatusDatas.Add(new (
                        BI_data.title,
                        resourceSystemScript.GetValueBuildingInformation(i).Item3,
                        resourceSystemScript.GetValueBuildingInformation(i).Item1.ConvertTo<float>(),
                        resourceSystemScript.GetValueBuildingInformation(i).Item2.ConvertTo<float>(),
                        BI_data.buildingInformationType
                    ));
                    
                    i++;
                }
            }
            else
            {
                int i = 0;
                foreach (var BI_data in resourceSystemScript.buildingInfoStruct.informations)
                {
                    resourceSettingUIScript.resourceInformationDatas.Add(new (
                        BI_data.title,
                        BI_data.text,
                        0,
                        0,
                        BI_data.buildingInformationType
                    ));
                    i++;
                }
            }
        }

        private void Create_Button_Building_Data(Building_Setting_UI_Script buildingSettingUIScript, Building_System_Script buildingSystemScript, UnityAction<object> actionbuilding, Building_Setting_Data buildingSettingData)
        {
            Building_Setting_UI_Data buildingSettingUIData = new Building_Setting_UI_Data();
            buildingSettingUIData.actions = new List<UnityAction>();

            if (buildingSettingUIData.buildingSettingType == Building_Setting_Type.Centor_Button_only && buildingSettingUIData.buildingSettingType == Building_Setting_Type.Bottom_Button_only)
            {
                buildingSettingUIData.actions.Add((() => { actionbuilding.Invoke(0); }));
            }
            else
            {
                buildingSettingUIData.actions.Add((() => { actionbuilding.Invoke(-1); }));
                buildingSettingUIData.actions.Add((() => { actionbuilding.Invoke(1); }));
            }

            buildingSettingUIData.buildingSystemScript = buildingSystemScript;
            buildingSettingUIData.buildingSettingType = buildingSettingData.buildingSettingType;
            buildingSettingUIData.icon_enable = buildingSettingData.icon_enable;
            buildingSettingUIData.icon_disable = buildingSettingData.icon_disable;
            buildingSettingUIData.text_enable = buildingSettingData.text_enable;
            buildingSettingUIData.text_disable = buildingSettingData.text_disable;
            buildingSettingUIData.light_Color = buildingSettingData.light_Color;
            buildingSettingUIData.dark_Color = buildingSettingData.dark_Color;
            buildingSettingUIScript.buildingSettingUIDatas.Add(buildingSettingUIData);

        }
        
        private void Create_Button_Resource_Data(Resource_Setting_UI_Script resourceSettingUIScript, Static_Object_Resource_System_Script resourceSystemScript, UnityAction<object> actionbuilding, Static_Resource_Setting_Data buildingSettingData)
        {
            Resource_Setting_UI_Data resourceSettingUIData = new ();
            resourceSettingUIData.actions = new List<UnityAction>();

            if (resourceSettingUIData.buildingSettingType == Building_Setting_Type.Centor_Button_only && resourceSettingUIData.buildingSettingType == Building_Setting_Type.Bottom_Button_only)
            {
                resourceSettingUIData.actions.Add((() => { actionbuilding.Invoke(0); }));
            }
            else
            {
                resourceSettingUIData.actions.Add((() => { actionbuilding.Invoke(-1); }));
                resourceSettingUIData.actions.Add((() => { actionbuilding.Invoke(1); }));
            }

            resourceSettingUIData.resourceSystemScript = resourceSystemScript;
            resourceSettingUIData.buildingSettingType = buildingSettingData.buildingSettingType;
            resourceSettingUIData.icon_enable = buildingSettingData.icon_enable;
            resourceSettingUIData.icon_disable = buildingSettingData.icon_disable;
            resourceSettingUIData.text_enable = buildingSettingData.text_enable;
            resourceSettingUIData.text_disable = buildingSettingData.text_disable;
            resourceSettingUIData.light_Color = buildingSettingData.light_Color;
            resourceSettingUIData.dark_Color = buildingSettingData.dark_Color;
            resourceSettingUIScript.resourceSettingUIDatas.Add(resourceSettingUIData);

        }
        
        private bool CheckComponentDisable()
        {
            bool isDisable = true;

            foreach (var component in m_Disable_Component)
            {
                if (component.enabled)
                    isDisable = false;
            }

            return isDisable;
        }
        
        private void OnOutlineObject(GameObject _gameObject, GameObject old_gameObject)
        {
            if ((_gameObject.transform.parent == m_GameObjectLayer.transform || _gameObject.transform.parent == m_ResourceLayer.transform) && CheckComponentDisable())
            {
                List<Outliner> outliner_parents = GetOutliner(_gameObject);

                
                if (_gameObject.GetComponent<Building_System_Script>() != null && _gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object != null)
                {
                    List<Outliner> outliner_construction_progress = GetOutliner(_gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object);
                    foreach (var _outliner in outliner_construction_progress)
                    {
                        _outliner.enabled = true;
                        _outliner.OutlineColor = Color.yellow;
                    }
                }

                foreach (var _outliner in outliner_parents)
                {
                    _outliner.enabled = true;
                    _outliner.OutlineColor = Color.yellow;
                }
                
                
            }

            if (old_gameObject != null && old_objecthit != _gameObject && (old_gameObject.transform.parent == m_GameObjectLayer.transform || old_gameObject.transform.parent == m_ResourceLayer.transform))
            {
                if (old_gameObject.GetComponent<Building_System_Script>() != null && old_gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object != null)
                {
                    old_gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object.layer = L_Place_Object;
                    HideOutliner(old_gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object);
                }
                
                HideOutliner(old_gameObject);
            }
            
            //print("Outline : " + (old_gameObject == null));
        }

        private List<Outliner> GetOutliner(GameObject _gameObject)
        {
            List<Outliner> outliner_parents = new ();
            if(_gameObject.GetComponent<Outliner>() != null)
                outliner_parents.Add(_gameObject.GetComponent<Outliner>());
            else if (_gameObject.GetComponent<AutoAddOutlinerGameObjects>() != null)
            {
                foreach (var _outliner in _gameObject.GetComponent<AutoAddOutlinerGameObjects>().get_outliners)
                {
                    outliner_parents.Add(_outliner);
                }
            } else if (_gameObject.GetComponent<Building_System_Script>() != null)
            {
                foreach (var _gObject in _gameObject.GetComponent<Building_System_Script>().building_Objects)
                {
                    foreach (var _outliner in _gObject.GetComponent<AutoAddOutlinerGameObjects>().get_outliners)
                    {
                        outliner_parents.Add(_outliner);
                    }
                }
            }

            return outliner_parents;
        }
        
        private void HideOutliner(GameObject _gameObject)
        {
            if(_gameObject.GetComponent<Outliner>() != null)
                _gameObject.GetComponent<Outliner>().enabled = false;
            else if (_gameObject.GetComponent<AutoAddOutlinerGameObjects>() != null)
            {
                foreach (var _outliner in _gameObject.GetComponent<AutoAddOutlinerGameObjects>().get_outliners)
                {
                    _outliner.enabled = false;
                }
            }else if (_gameObject.GetComponent<Building_System_Script>() != null)
            {
                foreach (var _gObject in _gameObject.GetComponent<Building_System_Script>().building_Objects)
                {
                    foreach (var _outliner in _gObject.GetComponent<AutoAddOutlinerGameObjects>().get_outliners)
                    {
                        _outliner.enabled = false;
                    }
                }
            }
        }
    }
}