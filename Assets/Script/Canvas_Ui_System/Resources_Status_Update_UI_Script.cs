using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class Resources_Status_Update_UI_Script : MonoBehaviour
    {
        [SerializeField] private Canvas_Element_List rock;
        [SerializeField] private Canvas_Element_List tree;
        [SerializeField] private Canvas_Element_List raw_food;
        [SerializeField] private Canvas_Element_List food;
        [SerializeField] private Canvas_Element_List power;
        [SerializeField] private Canvas_Element_List token;

        private GameManager GM;
        private GameInstance GI;
        private ResourcesManager RM;

        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
            RM = ResourcesManager.Instance;
        }

        private void Update()
        {
            rock.texts[0].text = GI.resources.rock.ToString();
            tree.texts[0].text = GI.resources.tree.ToString();
            raw_food.texts[0].text = GI.resources.raw_food.ToString();
            food.texts[0].text = GI.resources.food.ToString();
            power.texts[0].text = Mathf.FloorToInt(GI.resources.power).ToString();
            token.texts[0].text = GI.resources.token.ToString();
            
            rock.image[0].fillAmount = (float)GI.resources.rock / (float)GI.max_resources.rock;
            tree.image[0].fillAmount = (float)GI.resources.tree / (float)GI.max_resources.tree;
            raw_food.image[0].fillAmount = (float)GI.resources.raw_food / (float)GI.max_resources.raw_food;
            food.image[0].fillAmount = (float)GI.resources.food / (float)GI.max_resources.food;
            power.image[0].fillAmount = (float)Mathf.FloorToInt(GI.resources.power) / (float)RM.get_all_power_produce();
            token.image[0].fillAmount = (float)GI.resources.token / (float)GI.max_resources.rock;
        }
    }
}