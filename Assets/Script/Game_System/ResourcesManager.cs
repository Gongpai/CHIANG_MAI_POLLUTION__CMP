using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                        sum_power -= _buildingSystemScript.get_power_use;
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

        public int get_all_power_produce()
        {
            float all_power = 0;
            List<Generator_Script> generaters = FindObjectsByType<Generator_Script>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
            Parallel.ForEach(generaters, generatorScript =>
            {
                all_power += generatorScript.max_power_produce;
            });

            return (int)all_power;
        }

        public void Set_Resources_Tree(int tree)
        {
            int resource = GI.get_tree_resource() + tree;
            if(resource >= 0 && (resource < GI.max_resources.tree || tree <= 0)) 
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
            int resource = GI.get_rock_resource() + rock;
            if(resource >= 0 && (resource < GI.max_resources.rock || rock <= 0)) 
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
        
        public void Set_Resources_Raw_Food(int raw_food)
        {
            int resource = GI.get_raw_food_resource() + raw_food;
            if(resource >= 0 && (resource < GI.max_resources.raw_food || raw_food <= 0))  
                GI.set_raw_food_resource(GI.get_raw_food_resource() + raw_food);
        }
        
        public bool Can_Set_Resources_Raw_Food(int raw_food)
        {
            return !(GI.get_raw_food_resource() + raw_food < 0 || GI.get_raw_food_resource() == 0);
        }

        public int Get_Resources_Raw_Food()
        {
            return GI.get_raw_food_resource();
        }
        
        public void Set_Resources_Food(int food)
        {
            int resource = GI.get_food_resource() + food;
            if(resource >= 0 && (resource < GI.max_resources.food || food <= 0)) 
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
        
        public void Set_Resources_Token(int token)
        {
            int resource = GI.get_token_resource() + token;
            if(resource >= 0 && (resource < GI.max_resources.token || token <= 0)) 
                GI.set_token_resource(GI.get_token_resource() + token);
        }
        
        public bool Can_Set_Resources_Token(int token)
        {
            return !(GI.get_token_resource() + token < 0 || GI.get_token_resource() == 0);
        }

        public int Get_Resources_Token()
        {
            return GI.get_token_resource();
        }
    }
}