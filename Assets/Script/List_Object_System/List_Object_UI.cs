using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class List_Object_UI : MonoBehaviour
    {
        [SerializeField] private List<Button> buttonTabs;
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
            
            Object_Data = IRPD.Get_Resources_PreferencesData(Application.dataPath + GS_RD.resources_data_path)
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
                if (ob_data.Value.Split("/")[1] == NameObjectTypeFolder)
                {
                    GameObject element = Instantiate(Content_Element, Content_List.transform);
                    Object_Element_UI element_ui = element.GetComponent<Object_Element_UI>();
                    element_ui.text.text = ob_data.Key;
                    element_ui.botton.onClick.AddListener(() => { buildingPlace.Select_Building(ob_data.Value); });
                }
            }
        }
        
        private void OnDestroy()
        {
            buildingPlace.OnDisabledBuilding_Place();
            Is_UI_Open = false;
        }
    }
}