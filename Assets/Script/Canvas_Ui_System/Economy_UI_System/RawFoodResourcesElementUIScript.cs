namespace GDD.Economy_UI_System
{
    public class RawFoodResourcesElementUIScript : EconomyResourcesElementUIScript
    {
        public override void Update_Resource_Data()
        {
            _canvasElementList.image[0].fillAmount = (float)GI.get_raw_food_resource() / (float)GI.max_resources.raw_food;
            _canvasElementList.texts[0].text = "จำนวนทรัพยากรอาหารดิบในคลังขณะนี้ " + GI.get_raw_food_resource() + "/" + GI.max_resources.raw_food;
        }

        public override int Get_Old_Resource()
        {
            return GI.get_raw_food_resource();
        }
    }
}