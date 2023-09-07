using System;
using UnityEngine;

namespace GDD
{
    public class Resources_Status_Update_UI_Script : MonoBehaviour
    {
        [SerializeField] private Canvas_Element_List rock;
        [SerializeField] private Canvas_Element_List tree;
        [SerializeField] private Canvas_Element_List food;
        [SerializeField] private Canvas_Element_List power;

        private GameManager GM;
        private GameInstance GI;

        private void Start()
        {
            GM = GameManager.Instance;
            GI = GM.gameInstance;
        }

        private void Update()
        {
            rock.texts[0].text = GI.resources.rock.ToString();
            tree.texts[0].text = GI.resources.tree.ToString();
            food.texts[0].text = GI.resources.food.ToString();
            power.texts[0].text = GI.resources.power.ToString();
        }
    }
}