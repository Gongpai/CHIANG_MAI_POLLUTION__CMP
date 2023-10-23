namespace GDD.Economy_UI_System
{
    public class TreeResourcesElementUIScript : EconomyResourcesElementUIScript
    {
        public override void Update_Resource_Data()
        {
            _canvasElementList.image[0].fillAmount = (float)GI.get_tree_resource() / (float)GI.max_resources.tree;
            _canvasElementList.texts[0].text = "จำนวนทรัพยากรไม้ในคลังขณะนี้ " + GI.get_tree_resource() + "/" + GI.max_resources.tree;

        }

        public override int Get_Old_Resource()
        {
            return GI.get_tree_resource();
        }
    }
}