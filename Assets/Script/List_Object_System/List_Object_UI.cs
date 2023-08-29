using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class List_Object_UI : MonoBehaviour
    {
        [SerializeField] private List<Button> buttonTabs;
        [SerializeField] private Sprite m_road_remove_Icon;
        [SerializeField] private GameObject Content_List;
        [SerializeField] private GameObject Content_Element;
        
        private Dictionary<string, string> Object_Data = new Dictionary<string, string>();
        private bool Is_UI_Open = false;
        private Building_place buildingPlace;

        void Start()
        {
            buildingPlace = gameObject.AddComponent<Building_place>();
            Is_UI_Open = true;
            Interface_Resources_PreferencesData IRPD = new SaveLoad_Resources_Data();
            GetSet_Resources_Data GS_RD = new GetSet_Resources_Data();
            
            Object_Data = IRPD.Get_Resources_PreferencesData(GS_RD.resources_data_path)
                .Resources_Data;

            foreach (var _but in buttonTabs)
            {
                _but.onClick.AddListener(() => { ReadObjectInDirectory(_but.name); });
            }

            ReadObjectInDirectory("Generator");
            buildingPlace.OnActivateBuilding_Place();
        }

        public void ReadObjectInDirectory(string NameObjectTypeFolder)
        {
            if (Content_List.transform.childCount > 0)
            {
                foreach (Transform child in Content_List.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            
            foreach (var ob_data in Object_Data)
            {
                var nameAsset = ob_data.Value.Split("/")[1];
                if (nameAsset == NameObjectTypeFolder)
                {
                    if (nameAsset == "Road")
                    {
                        Object_Element_UI road_element_ui = CreateButtonElement();
                        road_element_ui.text.text = "Remove Road";
                        road_element_ui.image.sprite = m_road_remove_Icon;
                        road_element_ui.botton.onClick.AddListener(() =>
                        {
                            buildingPlace.Select_Road(null);
                        });
                    }
                    
                    Object_Element_UI element_ui = CreateButtonElement();
                    element_ui.text.text = ob_data.Key;
                    element_ui.botton.onClick.AddListener(() =>
                    {
                        if (nameAsset != "Road")
                        {
                            buildingPlace.Select_Building(ob_data.Value);
                        }
                        else
                        {
                            buildingPlace.Select_Road(ob_data.Value);
                        }
                        
                    });
                }
            }
        }

        private Object_Element_UI CreateButtonElement()
        {
            GameObject element = Instantiate(Content_Element, Content_List.transform);
            return element.GetComponent<Object_Element_UI>();
        }
        
        private void OnDestroy()
        {
            buildingPlace.OnDisabledRoad_Place();
            buildingPlace.OnDisabledBuilding_Place();
            Is_UI_Open = false;
        }
    }
}