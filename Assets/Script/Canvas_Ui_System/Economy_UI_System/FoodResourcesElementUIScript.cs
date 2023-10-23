namespace GDD.Economy_UI_System
{
    public class FoodResourcesElementUIScript : EconomyResourcesElementUIScript
    {
        public override void Update_Resource_Data()
        {
            _canvasElementList.image[0].fillAmount = (float)GI.get_food_resource() / (float)GI.max_resources.food;
            _canvasElementList.texts[0].text = "จำนวนทรัพยากรอาหารสุกในคลังขณะนี้ " + GI.get_food_resource() + "/" + GI.max_resources.food;
        }

        public override int Get_Old_Resource()
        {
            return GI.get_food_resource();
        }
    }
}