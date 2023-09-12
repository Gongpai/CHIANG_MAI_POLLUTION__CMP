using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private GameObject m_Button_list;
        [SerializeField] private GameObject m_BG_Button_list;
        [SerializeField] private GameObject m_GameObjectLayer;
        
        private GameObject old_objecthit;
        private GameObject objectHit;
        private GameObject old_menu = null;
        private GameObject old_buiding_setting_ui = null;
        private LayerMask L_Place_Object;
        private bool isOpenMenu = false;

        private void Start()
        {
            L_Place_Object = LayerMask.NameToLayer("Place_Object");
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
                
                if (hit_objraycast.transform.parent == m_GameObjectLayer.transform && CheckComponentDisable() && !isOpenMenu && !PointerOverUIElement.OnPointerOverUIElement())
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        Create_Buiding_Setting(objectHit.GetComponent<Building_System_Script>());
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        DestroyUI(old_menu);
                        DestroyUI(old_buiding_setting_ui);
                        CreateMenu();
                        isOpenMenu = true;
                    }
                }
                else if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && hit_objraycast.transform.parent != m_GameObjectLayer.transform && !PointerOverUIElement.OnPointerOverUIElement())
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
                    OnOutlineObject(objectHit, old_objecthit);
                
                if(objectHit.layer == L_Place_Object)
                    old_objecthit = objectHit;
            }
            
            if(Input.GetMouseButtonUp(0))
            {
                DestroyUI(old_menu);
                isOpenMenu = false;
            }
        }

        private void CreateMenu()
        {
            GameObject menu_ui = Instantiate(m_MenuUI);
            menu_ui.GetComponent<Animator>().SetBool("IsStart", true);
            menu_ui.GetComponent<RectTransform>().sizeDelta = new Vector2(260, 0);
            List<Button_Action_Data> menuDatas = objectHit.GetComponent<Building_System_Script>().GetInteractAction();
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

        private void Create_Buiding_Setting(Building_System_Script buildingSystemScript)
        {
            if(buildingSystemScript != null)
                print("Is nottt nuuulllllllllll");
            
            GameObject buiding_setting_ui = Instantiate(m_Building_Setting_UI);
            buiding_setting_ui.GetComponent<Animator>().SetBool("IsStart", true);
            Building_Setting_UI_Script buildingSettingUIScript = buiding_setting_ui.GetComponent<Building_Setting_UI_Script>();
            
            if(buildingSettingUIScript == null)
                print("gehghgdhdfghdfg n : ");
            
            buildingSettingUIScript.buildingSettingUIDatas = new List<Building_Setting_UI_Data>();
            buildingSettingUIScript.buildingName_text = buildingSystemScript.name;
            buildingSettingUIScript.building_Icon = Resources.Load<Sprite>("Icon/construction");
            buildingSettingUIScript.BuildingButtonActionDatas = buildingSystemScript.buildingButtonActionDatas;

            foreach (var actionbuilding in buildingSystemScript.actionsBuilding)
            {
                Create_Button_Data(buildingSettingUIScript, buildingSystemScript, actionbuilding.Key, actionbuilding.Value);
            }
            
            Create_Info_Ui_data(buildingSettingUIScript, buildingSystemScript, Building_Information_Type.ShowStatus);
            Create_Info_Ui_data(buildingSettingUIScript, buildingSystemScript, Building_Information_Type.ShowInformation);
            
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

        private void Create_Info_Ui_data(Building_Setting_UI_Script buildingSettingUIScript, Building_System_Script buildingSystemScript, Building_Information_Type buildingInformationType)
        {
            if (buildingInformationType == Building_Information_Type.ShowStatus)
            {
                int i = 0;
                foreach (var BI_data in buildingSystemScript.buildingInfoStruct.status)
                {
                    print("IIII : " + i);
                    buildingSettingUIScript.buildingStatusDatas.Add(new Building_Information_UI_Data(
                        BI_data.title,
                        buildingSystemScript.GetValueBuildingInformation(i).Item3,
                        buildingSystemScript.GetValueBuildingInformation(i).Item1.ConvertTo<float>(),
                        buildingSystemScript.GetValueBuildingInformation(i).Item2.ConvertTo<float>(),
                        BI_data.buildingInformationType
                    ));
                    
                    i++;
                }
            }
            else
            {
                int i = 0;
                foreach (var BI_data in buildingSystemScript.buildingInfoStruct.informations)
                {
                    buildingSettingUIScript.buildingInformationDatas.Add(new Building_Information_UI_Data(
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

        private void Create_Button_Data(Building_Setting_UI_Script buildingSettingUIScript, Building_System_Script buildingSystemScript, UnityAction<object> actionbuilding, Building_Setting_Data buildingSettingData)
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
        
        private void OnOutlineObject(GameObject gameObject, GameObject old_gameObject)
        {
            if (gameObject.transform.parent == m_GameObjectLayer.transform && CheckComponentDisable())
            {
                Outliner outliner_parent = gameObject.GetComponent<Outliner>();
                Outliner outliner_construction_progress = gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object.GetComponent<Outliner>();
                outliner_parent.enabled = true;
                outliner_construction_progress.enabled = true;
                outliner_parent.OutlineColor = Color.yellow;
                outliner_construction_progress.OutlineColor = Color.yellow;
            }

            if (old_gameObject != null && old_objecthit != gameObject && old_gameObject.transform.parent == m_GameObjectLayer.transform)
            {
                old_gameObject.layer = L_Place_Object;
                old_gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object.layer = L_Place_Object;
                
                old_gameObject.GetComponent<Outliner>().enabled = false;
                old_gameObject.GetComponent<Building_System_Script>().Construction_Progress_Object.GetComponent<Outliner>().enabled = false;
            }
        }
    }
}