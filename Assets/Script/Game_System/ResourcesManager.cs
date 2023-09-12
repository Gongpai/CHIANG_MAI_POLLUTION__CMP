using System;
using System.Collections.Generic;

namespace GDD
{
    public class ResourcesManager : Sinagleton_CanDestroy<ResourcesManager>
    {
        private GameManager GM;
        private GameInstance GI;
        private List<Generator_Script> _generatorScripts = new List<Generator_Script>();
        private float sum_power_use = 0;

        public List<Generator_Script> generatorScripts
        {
            get => _generatorScripts;
            set => _generatorScripts = value;
        }

        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
        }

        private void Update()
        {
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
            
            //print("Power All : " + sum_power + " kw");
            GI.set_power_resource(sum_power - sum_power_use);
        }

        public bool Set_Resources_Tree(int tree)
        {
            if (GI.get_tree_resource() + tree <= 0)
            {
                return false;
            }
            else
            {
                GI.set_tree_resource(GI.get_tree_resource() + tree);
                return true;
            }
        }

        public int Get_Resources_Tree()
        {
            return GI.get_tree_resource();
        }
        
        public bool Set_Resources_Rock(int rock)
        {
            if (GI.get_rock_resource() + rock <= 0)
            {
                return false;
            }
            else
            {
                GI.set_rock_resource(GI.get_rock_resource() + rock);
                return true;
            }
        }

        public int Get_Resources_Rock()
        {
            return GI.get_rock_resource();
        }
        
        public bool Set_Resources_Food(int food)
        {
            if (GI.get_food_resource() + food <= 0)
            {
                return false;
            }
            else
            {
                GI.set_food_resource(GI.get_food_resource() + food);
                return true;
            }
        }

        public int Get_Resources_Food()
        {
            return GI.get_food_resource();
        }

        public void Set_Resources_Power_Use(float power_use)
        {
            if (sum_power_use + power_use > 0)
            {
                sum_power_use += power_use;
            }
            else
            {
                sum_power_use = 0;
            }
        }
        
        public float Get_Resources_Power()
        {
            return GI.get_power_resource();
        }
    }
}