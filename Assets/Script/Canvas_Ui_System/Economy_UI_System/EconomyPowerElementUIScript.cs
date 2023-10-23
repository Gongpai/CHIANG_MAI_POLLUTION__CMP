using System;
using UnityEngine;

namespace GDD.Economy_UI_System
{
    public class EconomyPowerElementUIScript : MonoBehaviour
    {
        [SerializeField] private Canvas_Element_List _canvasElementList;
        protected GameManager GM;
        protected GameInstance GI;
        private ResourcesManager RM;
        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            RM = ResourcesManager.Instance;
        }
        private void Update()
        {
            _canvasElementList.texts[0].text = "กำลังไฟที่สามารถผลิตได้ตอนนี้ " + GI.get_power_resource() + "/" +
                                               RM.get_all_power_produce() + " kw";
        }
    }
}