namespace GDD.Economy_UI_System
{
    public class RockResourcesElementUIScript : EconomyResourcesElementUIScript
    {
        public override void Update_Resource_Data()
        {
            _canvasElementList.image[0].fillAmount = (float)GI.get_rock_resource() / (float)GI.max_resources.rock;
            _canvasElementList.texts[0].text = "จำนวนทรัพยากรหินในคลังขณะนี้ " + GI.get_rock_resource() + "/" + GI.max_resources.rock;
        }

        public override int Get_Old_Resource()
        {
            return GI.get_rock_resource();
        }
    }
}