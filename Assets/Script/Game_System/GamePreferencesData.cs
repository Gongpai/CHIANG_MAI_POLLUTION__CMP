using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;

namespace GDD
{
    [Serializable]
    public class GamePreferencesData
    {
        public List<object> GameDataSave;
    }
}