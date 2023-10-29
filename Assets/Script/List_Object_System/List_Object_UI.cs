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

        private GameManager GM;
        private GameInstance GI;
        private Resources_PreferencesData RPD;
        private Dictionary<string, string> Object_Data = new Dictionary<string, string>();
        private bool Is_UI_Open = false;
        private Building_place buildingPlace;

        void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            
            buildingPlace = gameObject.AddComponent<Building_place>();
            Is_UI_Open = true;
            Interface_Resources_PreferencesData IRPD = new SaveLoad_Resources_Data();
            GetSet_Resources_Data GS_RD = new GetSet_Resources_Data();
            
            RPD = IRPD.Get_Resources_PreferencesData(GS_RD.resources_data_path);
            Object_Data = RPD.Resources_Data;

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
                        road_element_ui.text.text = "ลบถนน";
                        road_element_ui.image.sprite = m_road_remove_Icon;
                        road_element_ui.botton.onClick.AddListener(() => { buildingPlace.Select_Road(null); });
                    }

                    Object_Element_UI element_ui = CreateButtonElement();
                    
                    if (nameAsset != "Road")
                        element_ui.text.text = Resources.Load<GameObject>(ob_data.Value).GetComponent<Building_System_Script>().name;
                    else
                        element_ui.text.text = "วางถนน";
                    
                    if (nameAsset != "Road")
                        element_ui.image.sprite = Resources.Load<GameObject>(ob_data.Value)
                            .GetComponent<Building_System_Script>().icon;
                    else
                        element_ui.image.sprite = Resources.Load<Sprite>("Icon/add_road_icon");
                    
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

                    if (nameAsset != "Road" && !Get_Building_Unlock(Resources.Load<GameObject>(ob_data.Value)
                            .GetComponent<Building_System_Script>().unlock_code))
                    {
                        Destroy(element_ui.gameObject);
                    }
                }
            }
        }

        private bool Get_Building_Unlock(string code)
        {
            int level = int.Parse(code.Split("/")[0]);
            int element = int.Parse(code.Split("/")[1]);

            switch (level)
            {
                case 1:
                    switch (element)
                    {
                        case 1:
                            return GI.TUDataSave.generator_leveltwo;
                        case 2:
                            return GI.TUDataSave.resident_leveltwo;
                        default:
                            return true;
                    }
                    break;
                case 2:
                    switch (element)
                    {
                        case 1:
                            return GI.TUDataSave.resident_levelthree;
                        default:
                            return true;
                    }
                    break;
                case 3:
                    switch (element)
                    {
                        case 1:
                            return GI.TUDataSave.infirmary_unlock;
                        default:
                            return true;
                    }
                    break;
                default:
                    return true;
            }
        }
        
        private Object_Element_UI CreateButtonElement()
        {
            GameObject element = Instantiate(Content_Element, Content_List.transform);
            return element.GetComponent<Object_Element_UI>();
        }
        
        private void Update()
        {
            Time_Controll_UI_Script.SetSpeed(0);
        }
        
        private void OnDestroy()
        {
            Time_Controll_UI_Script.auto_Resume_Time();
            
            buildingPlace.OnDisabledRoad_Place();
            buildingPlace.OnDisabledBuilding_Place();
            Is_UI_Open = false;
        }
    }
}