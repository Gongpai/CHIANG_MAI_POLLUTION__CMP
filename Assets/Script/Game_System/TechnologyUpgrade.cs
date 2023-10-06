using System;
using System.Linq;
using UnityEngine;

namespace GDD
{
    public class TechnologyUpgrade : MonoBehaviour
    {
        [SerializeField] private TechnologyUpgrade_Preset m_technologyUpgradePreset;
        [SerializeField] private TechnologyUpgrade_Details_Preset m_technologyUpgradeDetailsPreset;
        [SerializeField] private GameObject message_ui;
        private TechnologyUpgrade_DataSave _TUDataSave;
        
        private GameManager GM;
        private GameInstance GI;

        private TechnologyUpgrade_Details_Preset TU_Details_Preset
        {
            get => m_technologyUpgradeDetailsPreset;
        }

        private TechnologyUpgrade_Preset TU_Upgrade_Preset
        {
            get => m_technologyUpgradePreset;
        }

        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            _TUDataSave = GI.TUDataSave;
        }
        
        public void CreateUI_Message(int level, int index)
        {
            Ui_Utilities _uiUtilities;
            
            if (GetComponent<Ui_Utilities>() == null)
            {
                _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            }
            else
            {
                _uiUtilities = GetComponent<Ui_Utilities>();
            }

            _uiUtilities.canvasUI = message_ui;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.order_in_layer = 12;
            GameObject m_input_button = _uiUtilities.CreateMessageUI(() =>
                {
                    Level_UP(level, index);
                }, () =>
                {

                },
                TU_Details_Preset.lists[level].details[index].title, TU_Details_Preset.lists[level].details[index].message, "Upgrade", "Cancel", false);
            
            m_input_button.GetComponent<Animator>().SetBool("IsStart", true);
        }
        
        public void Level_UP(int level, int index)
        {
            switch (level)
            {
                case 1:
                    Upgrade_Level_One(index);
                    break;
                case 2:
                    Upgrade_Level_One(index);
                    break;
                case 3:
                    Upgrade_Level_One(index);
                    break;
                case 4:
                    Upgrade_Level_One(index);
                    break;
            }
        }
        
        public void Upgrade_Level_One(int index)
        {
            switch (index)
            {
                case 1:
                    _TUDataSave.generator_leveltwo = true;
                    break;
                case 2:
                    _TUDataSave.resident_leveltwo = true;
                    break;
                case 3:
                    _TUDataSave.wood_leveltwo = true;
                    break;
                case 4:
                    _TUDataSave.rock_leveltwo = true;
                    break;
                case 5:
                    _TUDataSave.food_leveltwo = true;
                    break;
            }
        }
        
        public void Upgrade_Level_Two(int index)
        {
            switch (index)
            {
                case 1:
                    _TUDataSave.resident_levelthree = true;
                    break;
                case 2:
                    _TUDataSave.gate_leveltwo = true;
                    break;
                case 3:
                    _TUDataSave.wood_levelthree = true;
                    break;
                case 4:
                    _TUDataSave.rock_levelthree = true;
                    break;
                case 5:
                    _TUDataSave.food_levelthree = true;
                    break;
                case 6:
                    _TUDataSave.rawfood_leveltwo = true;
                    break;
                case 7:
                    _TUDataSave.air_purifier_leveltwo = true;
                    break;
            }
        }
        
        public void Upgrade_Level_Three(int index)
        {
            switch (index)
            {
                case 1:
                    _TUDataSave.infirmary_unlock = true;
                    break;
                case 2:
                    _TUDataSave.gate_levelthree = true;
                    break;
                case 3:
                    _TUDataSave.wood_levelfour = true;
                    break;
                case 4:
                    _TUDataSave.rock_levelfour = true;
                    break;
                case 5:
                    _TUDataSave.food_levelfour = true;
                    break;
                case 6:
                    _TUDataSave.rawfood_levelthree = true;
                    break;
                case 7:
                    _TUDataSave.token_leveltwo = true;
                    break;
            }
        }
        
        public void Upgrade_Level_Four(int index)
        {
            switch (index)
            {
                case 1:
                    _TUDataSave.rawfood_levelfour = true;
                    break;
                case 2:
                    _TUDataSave.token_levelthree = true;
                    break;
                case 3:
                    _TUDataSave.air_purifier_levelthree = true;
                    break;
            }
        }
    }
}