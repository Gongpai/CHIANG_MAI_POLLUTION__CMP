using Unity.VisualScripting;
using UnityEngine;

namespace GDD.Economy_UI_System
{
    public abstract class EconomyBuildingProductElementUIScript : MonoBehaviour, IEconomy_Building_Product_UI
    {
        [SerializeField] protected Canvas_Element_List _canvasElementList;
        protected GameManager GM;
        protected HumanResourceManager HRM;
        
        private void Start()
        {
            GM = GameManager.Instance;
            HRM = HumanResourceManager.Instance;
            
            if (_canvasElementList == null)
                _canvasElementList = GetComponent<Canvas_Element_List>();
        }
        
        private void Update()
        {
            if (_canvasElementList.image[0] != null)
                Update_Building_Data();
            
            if(_canvasElementList.image.Count > 1)
                Update_Village();
            
            if(_canvasElementList.image.Count > 2)
                Update_Worker();
        }

        public abstract void Update_Building_Data();

        public void Update_Village()
        {
            _canvasElementList.image[1].fillAmount = (float)HRM.villagers_count / (float)GM.gameInstance.villagerSaveDatas.Count;
            _canvasElementList.texts[1].text = "จำนวนชาวบ้านคงเหลือ " + HRM.villagers_count + "/" + GM.gameInstance.villagerSaveDatas.Count + " คน";
        }

        public void Update_Worker()
        {
            _canvasElementList.image[2].fillAmount = (float)HRM.worker_count / (float)GM.gameInstance.workerSaveDatas.Count;
            _canvasElementList.texts[2].text = "จำนวนคนงานคงเหลือ " + HRM.worker_count + "/" + GM.gameInstance.workerSaveDatas.Count + " คน";
        }
    }
}