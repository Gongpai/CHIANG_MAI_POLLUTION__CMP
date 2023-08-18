using System;
using System.Collections.Generic;
using TMPro;
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
        private bool isOpenMenu = false;

        private void Update()
        {
            Physics.Raycast(RaycastFromMouse.GetRayFromMouse(), out var hit_objraycast);

            if(Input.GetMouseButtonUp(0) && !PointerOverUIElement.OnPointerOverUIElement())
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
            List<Menu_Data> menuDatas = objectHit.GetComponent<Building_System_Script>().GetInteractAction();
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
            buildingSettingUIScript.buildingSettingUIDatas = new List<Building_Setting_UI_Data>();
            buildingSettingUIScript.buildingName_text = buildingSystemScript.name;
            buildingSettingUIScript.building_Icon = Resources.Load<Sprite>("Icon/construction");
            
            Building_Setting_UI_Data buildingSettingUIData = new Building_Setting_UI_Data();
            buildingSettingUIData.actions = new List<UnityAction>();
            buildingSettingUIData.actions.Add(() =>
                {
                    print("Busssssssssssssssssssssssssssssssssss active");
                    buildingSystemScript.active = !buildingSystemScript.active;
                });
            buildingSettingUIData.buildingSystemScript = buildingSystemScript;
            buildingSettingUIData.buildingSettingButton = Building_Setting_Button.Centor_Button_only;
            buildingSettingUIData.icon_enable = Resources.Load<Sprite>("Icon/lightbulb_On");
            buildingSettingUIData.icon_disable = Resources.Load<Sprite>("Icon/lightbulb_Off");
            buildingSettingUIData.text_enable = "On";
            buildingSettingUIData.text_disable = "Off";
            buildingSettingUIData.light_Color = Color.white;
            buildingSettingUIData.dark_Color = Color.white;
            buildingSettingUIScript.buildingSettingUIDatas.Add(buildingSettingUIData);
            
            Building_Setting_UI_Data buildingSettingUIData2 = new Building_Setting_UI_Data();
            buildingSettingUIData2.actions = new List<UnityAction>();
            buildingSettingUIData2.actions.Add(() =>
                {
                    print("Busssssssssssssssssssssssssssssssssss -----");
                    if(buildingSystemScript.people > 0)
                        buildingSystemScript.people--;
                });
            buildingSettingUIData2.actions.Add(() =>
                {
                    print("Busssssssssssssssssssssssssssssssssss +++++");
                    if(buildingSystemScript.people < buildingSystemScript.people_Max)
                    buildingSystemScript.people++;
                });
            buildingSettingUIData2.buildingSystemScript = buildingSystemScript;
            buildingSettingUIData2.buildingSettingButton = Building_Setting_Button.Centor_Button_with_progress;
            buildingSettingUIData2.icon_enable = Resources.Load<Sprite>("Icon/lightbulb_On");
            buildingSettingUIData2.icon_disable = Resources.Load<Sprite>("Icon/lightbulb_Off");
            buildingSettingUIData2.text_enable = "People";
            buildingSettingUIData2.text_disable = "People";
            buildingSettingUIData2.light_Color = Color.white;
            buildingSettingUIData2.dark_Color = Color.white;
            buildingSettingUIScript.buildingSettingUIDatas.Add(buildingSettingUIData2);
            
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
                Outliner outliner = gameObject.GetComponent<Outliner>();
                outliner.enabled = true;
                outliner.OutlineColor = Color.yellow;
            }

            if (old_gameObject != null && old_objecthit != gameObject && old_gameObject.transform.parent == m_GameObjectLayer.transform)
                old_gameObject.GetComponent<Outliner>().enabled = false;
        }
    }
}