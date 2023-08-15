using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class ObjectInteract : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> m_Disable_Component;
        [SerializeField] private GameObject m_MenuUI;
        [SerializeField] private GameObject m_Button_list;
        [SerializeField] private GameObject m_BG_Button_list;
        [SerializeField] private GameObject m_GameObjectLayer;
        
        private GameObject old_objecthit;
        private GameObject objectHit;
        private GameObject old_menu = null;
        private bool isOpenMenu = false;

        private void Update()
        {
            Physics.Raycast(RaycastFromMouse.GetRayFromMouse(), out var hit_objraycast);

            if (hit_objraycast.transform != null)
            {
                objectHit = hit_objraycast.transform.gameObject;

                if (hit_objraycast.transform.parent == m_GameObjectLayer.transform && CheckComponentDisable() && !isOpenMenu && !PointerOverUIElement.OnPointerOverUIElement())
                {
                    if (Input.GetMouseButtonUp(1))
                    {
                        DestroyMenu(old_menu);
                        CreateMenu();
                        isOpenMenu = true;
                    }
                }
                else if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && hit_objraycast.transform.parent != m_GameObjectLayer.transform && !PointerOverUIElement.OnPointerOverUIElement())
                {
                    isOpenMenu = false;
                    DestroyMenu(old_menu);
                }
                else
                {
                    isOpenMenu = false;
                }

                if (objectHit != null)
                    OnOutlineObject(objectHit, old_objecthit);
                old_objecthit = objectHit;
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

        private void DestroyMenu(GameObject menu)
        {
            if (menu != null)
            {
                menu.GetComponent<Canvas>().sortingOrder = 8;
                Animator animator = menu.GetComponent<Animator>();
                animator.SetBool("IsStart", false);
                Destroy(menu, animator.GetCurrentAnimatorStateInfo(0).length);
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