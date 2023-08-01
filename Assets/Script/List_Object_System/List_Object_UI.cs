using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class List_Object_UI : MonoBehaviour
    {
        [SerializeField] private GameObject Content_List;
        [SerializeField] private GameObject Content_Element;

        private Dictionary<string, string> Object_Data = new Dictionary<string, string>();

        void Start()
        {
            Interface_Resources_PreferencesData IRPD = new SaveLoad_Resources_Data();
            GetSet_Resources_Data GS_RD = new GetSet_Resources_Data();
            Building_place buildingPlace = new Building_place();
            
            Object_Data = IRPD.Get_Resources_PreferencesData(Application.dataPath + GS_RD.resources_data_path)
                .Resources_Data;

            foreach (var ob_data in Object_Data)
            {
                GameObject element = Instantiate(Content_Element, Content_List.transform);
                Object_Element_UI element_ui = element.GetComponent<Object_Element_UI>();
                element_ui.text.text = ob_data.Key;
                
                element_ui.botton.onClick.AddListener(() => { buildingPlace.Select_Building(ob_data.Value); });
            }
        }
    }
}