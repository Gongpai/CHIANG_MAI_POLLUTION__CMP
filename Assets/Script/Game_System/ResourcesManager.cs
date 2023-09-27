using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ResourcesManager : Sinagleton_CanDestroy<ResourcesManager>
    {
        [SerializeField] private int foooddd = 0;
        [SerializeField] private bool is_Set = false;
        private GameManager GM;
        private GameInstance GI;
        private List<Generator_Script> _generatorScripts = new List<Generator_Script>();
        private List<Building_System_Script> _buildingSystemScripts = new List<Building_System_Script>();

        public List<Generator_Script> generatorScripts
        {
            get => _generatorScripts;
            set => _generatorScripts = value;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            GM = GameManager.Instance;
            GI = GM.gameInstance;
        }

        private void Update()
        {
            if (is_Set)
            {
                GI.set_food_resource(foooddd);
            }

            Get_Sum_Resources_Power();
        }
        
        private void Get_Sum_Resources_Power()
        {
            float sum_power = 0;
            
            foreach (var _generatorScript in generatorScripts)
            {
                
                BuildingSaveData _buildingSaveData = _generatorScript.buildingSaveData;
                
                if ((BuildingType)_buildingSaveData.b_buildingtype == BuildingType.Generator)
                {
                    sum_power += _generatorScript.power_produce;
                }
            }

            //print("Building Script : " + _buildingSystemScripts.Count);
            foreach (var _buildingSystemScript in _buildingSystemScripts)
            {
                if (_buildingSystemScript._buildingType != BuildingType.Generator && _buildingSystemScript.building_preset.power_use > 0)
                {
                    if (sum_power - _buildingSystemScript.building_preset.power_use > 0 && _buildingSystemScript.active)
                    {
                        sum_power -= _buildingSystemScript.building_preset.power_use;
                        _buildingSystemScript.cant_use_power = false;
                    }
                    else
                    {
                            _buildingSystemScript.cant_use_power = true;
                    }
                }
            }
            
            //print("Power All : " + sum_power + " kw");
            GI.set_power_resource(sum_power);
        }

        public void Set_Resources_Tree(int tree)
        {
            if(GI.get_tree_resource() + tree >= 0) 
                GI.set_tree_resource(GI.get_tree_resource() + tree);
        }
        
        public bool Can_Set_Resources_Tree(int tree)
        {
            if (GI.get_tree_resource() + tree < 0 || GI.get_tree_resource() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Get_Resources_Tree()
        {
            return GI.get_tree_resource();
        }
        
        public void Set_Resources_Rock(int rock)
        {
            if(GI.get_rock_resource() + rock >= 0) 
                GI.set_rock_resource(GI.get_rock_resource() + rock);
        }

        public bool Can_Set_Resources_Rock(int rock)
        {
            if (GI.get_rock_resource() + rock < 0 || GI.get_rock_resource() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Get_Resources_Rock()
        {
            return GI.get_rock_resource();
        }
        
        public void Set_Resources_Food(int food)
        {
            if(GI.get_food_resource() + food >= 0) 
                GI.set_food_resource(GI.get_food_resource() + food);
        }
        
        public bool Can_Set_Resources_Food(int food)
        {
            if (GI.get_food_resource() + food < 0 || GI.get_food_resource() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Get_Resources_Food()
        {
            return GI.get_food_resource();
        }

        public void Set_Resources_Power_Use(Building_System_Script buildingSystemScript, bool isRemove = false)
        {
            if (!isRemove)
            {
                _buildingSystemScripts.Add(buildingSystemScript);
            }
            else
            {
                _buildingSystemScripts.Remove(buildingSystemScript);
            }
        }

        public float Get_Resources_Power()
        {
            return GI.get_power_resource();
        }
    }
}