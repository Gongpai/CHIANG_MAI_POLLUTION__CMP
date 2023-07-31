using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace GDD
{
    public class SaveLoad_Resources_Data : Interface_Resources_PreferencesData
    {
        private Interface_Resources_PreferencesData _interfaceImplementation;

        public Resources_PreferencesData Get_Resources_PreferencesData(string location)
        {
            StreamReader sr = new StreamReader(location);
            Resources_PreferencesData RPD = new Resources_PreferencesData();
            SerializableDictionary.Deserialize(sr, RPD.Resources_Data);
            sr.Close();
            return RPD;
        }

        public void Set_Resources_PreferencesData(Resources_PreferencesData RPD, string location)
        {
            StreamWriter sw = new StreamWriter(location);
            SerializableDictionary.Serialize(sw, RPD.Resources_Data);
            sw.Close();
        }
    }
}
