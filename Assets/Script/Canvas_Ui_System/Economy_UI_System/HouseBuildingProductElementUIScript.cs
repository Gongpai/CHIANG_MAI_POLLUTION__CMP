namespace GDD.Economy_UI_System
{
    public class HouseBuildingProductElementUIScript : EconomyBuildingProductElementUIScript
    {
        public override void Update_Building_Data()
        {
            _canvasElementList.image[0].fillAmount = (float)HRM.residence.Count / ((float)HRM.villagers_count + (float)HRM.worker_count);
            _canvasElementList.texts[0].text = "จำนวนชาวเมืองที่มีที่อยู่อาศัย " + (float)HRM.residence.Count + "/" + ((float)HRM.villagers_count + (float)HRM.worker_count) + " คน";
        }
    }
}