using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

namespace GDD
{
    public class SaveManager : Singleton<SaveManager>
    {
        public GamePreferencesData LoadGamePreferencesData(string location)
        {
            location += ".json";
            string json = "";
            StreamReader sr = new StreamReader(location);
            json = sr.ReadToEnd();
            sr.Close();

            GamePreferencesData GPD = new GamePreferencesData();
            GPD = JsonConvert.DeserializeObject<GamePreferencesData>(json);
            return GPD;
        }

        public void SaveGamePreferencesData(GamePreferencesData gpd, string location)
        {
            location += ".json";
            string json = null;
            json = JsonConvert.SerializeObject(gpd);
            
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
