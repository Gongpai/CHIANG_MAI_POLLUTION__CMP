using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class GetSet_Resources_Data : MonoBehaviour
    {
        private Dictionary<string, string> Resources_Data = new Dictionary<string, string>();

        private string Resources_Data_Path = "/Resources/Resources_Data/Ressources_Building.xml";

        public string resources_data_path
        {
            get { return Resources_Data_Path; }
        }
        void Start()
        {
            var R_Data = Resources.LoadAll("Building");
            Interface_Resources_PreferencesData IRP = new SaveLoad_Resources_Data();

            foreach (var r in R_Data)
            {
                print(r.name);
                var path = "Building/" + r.name;
                Resources_Data.Add(r.name, path);
            }

            Resources_PreferencesData RPD = new Resources_PreferencesData { Resources_Data = Resources_Data };
            
            IRP.Set_Resources_PreferencesData(RPD, Application.dataPath + Resources_Data_Path);
            
            foreach (var r in Resources_Data)
            {
                print("In Dict : " + r.Key + " : " + r.Value);
            }

            print(Application.dataPath + Resources_Data_Path);
            var Out_ = IRP.Get_Resources_PreferencesData(Application.dataPath + Resources_Data_Path).Resources_Data;
            foreach (var our_r in Out_)
            {
                print("In Xml : " + our_r.Key + " : " + our_r.Value);
            }
        }
    }
}
