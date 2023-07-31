using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class GameManager:Singleton<GameManager>
    {
        private string Name = "";
        private int Level = 0;
        private int EXP = 0;
        private int Score = 0;

        public List<object> GetSetGameManager
        {
            get
            {
                var fieldValues = MemberInfoGetting.GetFieldValues(this);

                return fieldValues;
            }
            set
            {
                MemberInfoGetting.SetFieldValues(value, this);
            }
        }

        public string GetName
        {
            get { return Name; } 
            set { Name = value; } 
        }
        public int GetLevel
        {
            get { return Level; } 
            set { Level = value; } 
        }
        
        public int GetEXP
        {
            get { return EXP; } 
            set { EXP = value; } 
        }
        
        public int GetScore
        {
            get { return Score; } 
            set { Score = value; } 
        }
    }
}