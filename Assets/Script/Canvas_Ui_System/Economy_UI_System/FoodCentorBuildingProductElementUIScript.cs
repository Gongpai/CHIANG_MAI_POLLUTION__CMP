using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD.Economy_UI_System
{
    public class FoodCentorBuildingProductElementUIScript : EconomyBuildingProductElementUIScript
    {
        public override void Update_Building_Data()
        {
            Tuple<int, int> product = get_all_food_product();
            
            _canvasElementList.image[0].fillAmount = (float)product.Item1 / (float)product.Item2;
            _canvasElementList.texts[0].text = "กำลังการผลิตทั้งหมด " + (float)product.Item1 + "/" + ((float)product.Item2) + " คน";
        }

        private Tuple<int, int> get_all_food_product()
        {
            int resource = 0;
            int max_resource = 0;
            foreach (var buildingSystemScript in FindObjectsByType<Building_System_Script>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                if (buildingSystemScript.job == PeopleJob.Chef)
                {
                    resource += buildingSystemScript.ConvertTo<Cookhouse_Script>().get_product_output().Item1;
                    max_resource += buildingSystemScript.ConvertTo<Cookhouse_Script>().get_product_output().Item2;
                }
            }

            return new Tuple<int, int>(resource, max_resource);
        }
    }
}