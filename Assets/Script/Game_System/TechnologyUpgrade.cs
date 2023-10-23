using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class TechnologyUpgrade : MonoBehaviour
    {
        [SerializeField] private TechnologyUpgrade_Preset m_technologyUpgradePreset;
        [SerializeField] private TechnologyUpgrade_Details_Preset m_technologyUpgradeDetailsPreset;
        [SerializeField] private GameObject message_ui;
        [SerializeField] private List<Button> m_button_level;
        private TechnologyUpgrade_DataSave _TUDataSave;

        private GameManager GM;
        private GameInstance GI;
        private ResourcesManager RM;

        public GameObject get_message_ui
        {
            get => message_ui;
        }

        public TechnologyUpgrade_Details_Preset TU_Details_Preset
        {
            get => m_technologyUpgradeDetailsPreset;
        }

        public TechnologyUpgrade_Preset TU_Upgrade_Preset
        {
            get => m_technologyUpgradePreset;
        }

        public int get_current_level
        {
            get => _TUDataSave.level;
        }

        public int get_preset_data(int level, int index)
        {
            switch (level)
            {
                case 1:
                    switch (index)
                    {
                        case 1:
                            return TU_Upgrade_Preset.generator_leveltwo;
                        case 2:
                            return TU_Upgrade_Preset.resident_leveltwo;
                        case 3:
                            return TU_Upgrade_Preset.wood_leveltwo;
                        case 4:
                            return TU_Upgrade_Preset.rock_leveltwo;
                        case 5:
                            return TU_Upgrade_Preset.food_leveltwo;
                    }

                    break;
                case 2:
                    switch (index)
                    {
                        case 1:
                            return TU_Upgrade_Preset.resident_levelthree;
                        case 2:
                            return TU_Upgrade_Preset.gate_leveltwo;
                        case 3:
                            return TU_Upgrade_Preset.wood_levelthree;
                        case 4:
                            return TU_Upgrade_Preset.rock_levelthree;
                        case 5:
                            return TU_Upgrade_Preset.food_levelthree;
                        case 6:
                            return TU_Upgrade_Preset.rawfood_leveltwo;
                        case 7:
                            return TU_Upgrade_Preset.air_purifier_leveltwo;
                    }

                    break;
                case 3:
                    switch (index)
                    {
                        case 1:
                            return TU_Upgrade_Preset.infirmary_unlock;
                        case 2:
                            return TU_Upgrade_Preset.gate_levelthree;
                        case 3:
                            return TU_Upgrade_Preset.wood_levelfour;
                        case 4:
                            return TU_Upgrade_Preset.rock_levelfour;
                        case 5:
                            return TU_Upgrade_Preset.food_levelfour;
                        case 6:
                            return TU_Upgrade_Preset.rawfood_levelthree;
                        case 7:
                            return TU_Upgrade_Preset.token_leveltwo;
                    }

                    break;
                case 4:
                    switch (index)
                    {
                        case 1:
                            return TU_Upgrade_Preset.rawfood_levelfour;
                        case 2:
                            return TU_Upgrade_Preset.gate_levelfour;
                        case 3:
                            return TU_Upgrade_Preset.token_levelthree;
                        case 4:
                            return TU_Upgrade_Preset.air_purifier_levelthree;
                    }

                    break;
            }

            return 0;
        }

        public bool get_dataSave(int level, int index)
        {
            switch (level)
            {
                case 1:
                    switch (index)
                    {
                        case 1:
                            return _TUDataSave.generator_leveltwo;
                        case 2:
                            return _TUDataSave.resident_leveltwo;
                        case 3:
                            return _TUDataSave.wood_leveltwo;
                        case 4:
                            return _TUDataSave.rock_leveltwo;
                        case 5:
                            return _TUDataSave.food_leveltwo;
                    }

                    break;
                case 2:
                    switch (index)
                    {
                        case 1:
                            return _TUDataSave.resident_levelthree;
                        case 2:
                            return Get_Gate_Unlock(300003);
                        case 3:
                            return _TUDataSave.wood_levelthree;
                        case 4:
                            return _TUDataSave.rock_levelthree;
                        case 5:
                            return _TUDataSave.food_levelthree;
                        case 6:
                            return _TUDataSave.rawfood_leveltwo;
                        case 7:
                            return _TUDataSave.air_purifier_leveltwo;
                    }

                    break;
                case 3:
                    switch (index)
                    {
                        case 1:
                            return _TUDataSave.infirmary_unlock;
                        case 2:
                            return Get_Gate_Unlock(300000);
                        case 3:
                            return _TUDataSave.wood_levelfour;
                        case 4:
                            return _TUDataSave.rock_levelfour;
                        case 5:
                            return _TUDataSave.food_levelfour;
                        case 6:
                            return _TUDataSave.rawfood_levelthree;
                        case 7:
                            return _TUDataSave.token_leveltwo;
                    }

                    break;
                case 4:
                    switch (index)
                    {
                        case 1:
                            return _TUDataSave.rawfood_levelfour;
                        case 2:
                            return Get_Gate_Unlock(300002);
                        case 3:
                            return _TUDataSave.token_levelthree;
                        case 4:
                            return _TUDataSave.air_purifier_levelthree;
                    }

                    break;
            }

            return false;
        }

        private bool Get_Gate_Unlock(int id)
        {
            foreach (var gateScript in FindObjectsByType<Gate_Script>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (gateScript.get_resource_id == id)
                {
                    return gateScript.is_unlock;
                }
            }

            return false;
        }
        
        private void Set_Gate_Unlock(int id, bool value)
        {
            foreach (var gateScript in FindObjectsByType<Gate_Script>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (gateScript.get_resource_id == id)
                {
                    gateScript.is_unlock = value;
                    break;
                }
            }
        }
        
        public int get_token_level_use(int index)
        {
            switch (index)
            {
                case 2:
                    return TU_Upgrade_Preset.level_two;
                case 3:
                    return TU_Upgrade_Preset.level_three;
                case 4:
                    return TU_Upgrade_Preset.level_four;
            }

            return 0;
        }

        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            RM = ResourcesManager.Instance;
            
            _TUDataSave = GI.TUDataSave;

            int i = 1;
            foreach (var button in m_button_level)
            {
                i++;
                
                print("DataSave : " + _TUDataSave.level);
                if (i <= get_current_level)
                {
                    button.gameObject.SetActive(false);
                }
            }

            if (m_button_level[0].gameObject.activeSelf)
            {
                m_button_level[0].onClick.AddListener(() => { CreateUI_Message_Level_Button(m_button_level[0], 2, TU_Upgrade_Preset.level_two); });
            }
            if (m_button_level[1].gameObject.activeSelf)
            {
                m_button_level[1].onClick.AddListener(() => { CreateUI_Message_Level_Button(m_button_level[1], 3, TU_Upgrade_Preset.level_three); });
            }
            if (m_button_level[2].gameObject.activeSelf)
            {
                m_button_level[2].onClick.AddListener(() => { CreateUI_Message_Level_Button(m_button_level[2], 4, TU_Upgrade_Preset.level_four); });
            }
        }

        private void Update()
        {
            int i = 1;
            foreach (var button in m_button_level)
            {
                button.interactable = get_current_level == i || i < get_current_level;
                i++;
            }
        }

        public void CreateUI_Message_Level_Button(Button button, int i, int token_resource)
        {
            Ui_Utilities _uiUtilities;
            int index = i;
            
            if (GetComponent<Ui_Utilities>() == null)
            {
                _uiUtilities = gameObject.AddComponent<Ui_Utilities>();
            }
            else
            {
                _uiUtilities = GetComponent<Ui_Utilities>();
            }

            _uiUtilities.canvasUI = get_message_ui;
            _uiUtilities.useCameraOverlay = true;
            _uiUtilities.planeDistance = 0.5f;
            _uiUtilities.order_in_layer = 12;
            GameObject m_message_button = _uiUtilities.CreateMessageUI(() =>
                {
                    int token = RM.Get_Resources_Token() - token_resource;
                    if (RM.Can_Set_Resources_Token(token))
                    {
                        RM.Set_Resources_Token(-token_resource);
                        _TUDataSave.level = index;
                        button.gameObject.SetActive(false);
                        print("IIIIII :" + index);
                    }
                }, () =>
                {
                    
                },
                "ต้องการอัพเกรดหรือไม่?", "อัพเกรดเลเวล " + index + " เพื่อปลดล็อคเทคโนโลยีในเลเวลนี้ ต้องการ Token จำนวน " + get_token_level_use(index), "Upgrade", "Cancel", false);
            
            Destroy(m_message_button.GetComponent<Back_UI_Button_Script>());
            m_message_button.GetComponent<Animator>().SetBool("IsStart", true);
        }
        
        public void Level_UP(int level, int index)
        {
            switch (level)
            {
                case 1:
                    Upgrade_Level_One(index);
                    break;
                case 2:
                    Upgrade_Level_Two(index);
                    break;
                case 3:
                    Upgrade_Level_Three(index);
                    break;
                case 4:
                    Upgrade_Level_Four(index);
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
                    GI.max_resources.tree += 500;
                    break;
                case 4:
                    _TUDataSave.rock_leveltwo = true;
                    GI.max_resources.rock += 500;
                    break;
                case 5:
                    _TUDataSave.food_leveltwo = true;
                    GI.max_resources.food += 500;
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
                    Set_Gate_Unlock(300003, true);
                    break;
                case 3:
                    _TUDataSave.wood_levelthree = true;
                    GI.max_resources.tree += 500;
                    break;
                case 4:
                    _TUDataSave.rock_levelthree = true;
                    GI.max_resources.rock += 500;
                    break;
                case 5:
                    _TUDataSave.food_levelthree = true;
                    GI.max_resources.food += 500;
                    break;
                case 6:
                    _TUDataSave.rawfood_leveltwo = true;
                    GI.max_resources.raw_food += 500;
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
                    Set_Gate_Unlock(300000, true);
                    break;
                case 3:
                    _TUDataSave.wood_levelfour = true;
                    GI.max_resources.tree += 500;
                    break;
                case 4:
                    _TUDataSave.rock_levelfour = true;
                    GI.max_resources.rock += 500;
                    break;
                case 5:
                    _TUDataSave.food_levelfour = true;
                    GI.max_resources.food += 500;
                    break;
                case 6:
                    _TUDataSave.rawfood_levelthree = true;
                    GI.max_resources.raw_food += 500;
                    break;
                case 7:
                    _TUDataSave.token_leveltwo = true;
                    GI.max_resources.token += 100;
                    break;
            }
        }
        
        public void Upgrade_Level_Four(int index)
        {
            switch (index)
            {
                case 1:
                    _TUDataSave.rawfood_levelfour = true;
                    GI.max_resources.raw_food += 500;
                    break;
                case 2:
                    Set_Gate_Unlock(300002, true);
                    break;
                case 3:
                    _TUDataSave.token_levelthree = true;
                    GI.max_resources.token += 100;
                    break;
                case 4:
                    _TUDataSave.air_purifier_levelthree = true;
                    break;
            }
        }
    }
}