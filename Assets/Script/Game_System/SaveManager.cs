using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

namespace GDD
{
    public class SaveManager : Singleton<SaveManager>
    {
        public object LoadGamePreferencesData(string location, Type type)
        {
            location += ".json";
            string json = "";
            StreamReader sr = new StreamReader(location);
            json = sr.ReadToEnd();
            sr.Close();
            
            var data = JsonConvert.DeserializeObject(json, type);
            print(data);
            return data;
        }

        public void SaveGamePreferencesData(dynamic gpd, string location)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            
            location += ".json";
            string json = null;
            json = JsonConvert.SerializeObject(gm.gameInstance);
            
            StreamWriter sw = new StreamWriter(location);
            sw.Write(json);
            sw.Close();
        }
        public List<FileInfo> GetAllSaveGame(string location = default, bool isDefaultPath = false)
        {
            if(isDefaultPath)
                location = Application.persistentDataPath;
            
            var Info = new DirectoryInfo(location);
            var SaveGameInfo = Info.GetFiles("*.json*").OrderByDescending(f => f.LastWriteTime.Year <= 1601 ? f.CreationTime : f.LastWriteTime).ToList();
            //print("SGI : " + SaveGameInfo.Count);
            /**
            foreach (var SGI in SaveGameInfo)
            {
                print(SGI.Name.Split('.')[0]);
            }**/

            return SaveGameInfo;
        }
    }
}
