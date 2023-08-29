using System.IO;
using System.Collections.Generic;
using System;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class GetSet_Resources_Data : MonoBehaviour
    {
        private Dictionary<string, string> Resources_Data = new Dictionary<string, string>();

        private string Resources_Data_Path = "/Resources_Data/Ressources_Building.xml";

        public string resources_data_path
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        return Application.dataPath + "/Resources" + Resources_Data_Path;
                    case RuntimePlatform.WindowsPlayer:
                        return Application.streamingAssetsPath + Resources_Data_Path;
                    default:
                        return Application.dataPath + "/Resources" + Resources_Data_Path;
                }
            }
        }

        void Start()
        {
            var info = new DirectoryInfo(Application.dataPath + "/Resources/Construction/");
            print("path : " + Application.dataPath + "/Resources/Construction/");
            var folder = info.GetDirectories("**");
            Interface_Resources_PreferencesData IRP = new SaveLoad_Resources_Data();

            foreach (var dir in folder)
            {
                print("Dir : " + dir.Name);

                var R_Data = Resources.LoadAll("Construction/" + dir.Name);
                foreach (var r in R_Data)
                {
                    var path = "Construction/" + dir.Name + "/" + r.name;
                    print("path : " + path);
                    Resources_Data.Add(r.name, path);
                }
            }

            Resources_PreferencesData RPD = new Resources_PreferencesData { Resources_Data = Resources_Data };

            IRP.Set_Resources_PreferencesData(RPD, resources_data_path);

            foreach (var r in Resources_Data)
            {
                print("In Dict : " + r.Key + " : " + r.Value);
            }

            print(resources_data_path);
            var Out_ = IRP.Get_Resources_PreferencesData(resources_data_path).Resources_Data;
            foreach (var our_r in Out_)
            {
                print("In Xml : " + our_r.Key + " : " + our_r.Value);
            }
        }
    }
}