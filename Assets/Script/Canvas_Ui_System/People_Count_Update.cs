using System;
using UnityEngine;

namespace GDD
{
    public class People_Count_Update : MonoBehaviour
    {
        [SerializeField] private Canvas_Element_List _elementList;
        private GameManager GM;
        private HumanResourceManager HRM;

        private void Start()
        {
            GM = GameManager.Instance;
            HRM = HumanResourceManager.Instance;
        }

        private void Update()
        {
            _elementList.texts[0].text = HRM.villagers_count + "/" + GM.gameInstance.villagerSaveDatas.Count;
            _elementList.texts[1].text = HRM.worker_count + "/" + GM.gameInstance.workerSaveDatas.Count;
        }
    }
}