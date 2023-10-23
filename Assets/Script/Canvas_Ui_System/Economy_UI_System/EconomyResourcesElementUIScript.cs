using System;
using UnityEngine;

namespace GDD.Economy_UI_System
{
    public abstract class EconomyResourcesElementUIScript : MonoBehaviour, IEconomy_Resources_UI
    {
        [SerializeField] protected Canvas_Element_List _canvasElementList;
        protected GameManager GM;
        protected GameInstance GI;
        protected int current_resource = 0;
        protected int old_resource = 0;

        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            
            if (_canvasElementList == null)
                _canvasElementList = GetComponent<Canvas_Element_List>();
            
            current_resource = Get_Old_Resource();
            old_resource = Get_Old_Resource();
        }

        private void Update()
        {
            forecast_resource();
            
            Update_Resource_Data();
            
            old_resource = Get_Old_Resource();
        }

        private void forecast_resource()
        {
            int value = current_resource - old_resource;

            if (value > 0)
                _canvasElementList.image[1].fillAmount = value / 50;
            else
                _canvasElementList.image[2].fillAmount = Mathf.Abs(value) / 50;
        }

        public abstract void Update_Resource_Data();

        public abstract int Get_Old_Resource();
    }
}