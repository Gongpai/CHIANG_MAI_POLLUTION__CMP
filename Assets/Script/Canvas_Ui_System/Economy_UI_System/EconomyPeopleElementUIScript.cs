using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD.Economy_UI_System
{
    public class EconomyPeopleElementUIScript: MonoBehaviour
    {
        [SerializeField] private GameObject m_element;
        [SerializeField] private GameObject m_empty;
        [SerializeField] private Canvas_Element_List _canvasElementList;
        private GameManager GM;
        private List<GameObject> villagers = new List<GameObject>();
        private GameObject villager_empty;
        private List<GameObject> workers = new List<GameObject>();
        private GameObject worker_empty;
        
        private void Start()
        {
            GM = GameManager.Instance;

            if (_canvasElementList == null)
                _canvasElementList = GetComponent<Canvas_Element_List>();
            
            Create_People_Element();
        }

        private void Update()
        {
            
        }

        private void Create_People_Element()
        {
            if (GM.gameInstance.villagerSaveDatas.Count <= 0 )
            {
                if (villager_empty == null)
                {
                    villager_empty = Instantiate(m_empty, _canvasElementList.canvas_gameObjects[0].transform);
                }
            }
            else
            {
                if(villager_empty != null)
                    Destroy(villager_empty);
                
                foreach (var villager in villagers)
                {
                    Destroy(villager);
                }
                
                villagers = new List<GameObject>();
                foreach (var data in GM.gameInstance.villagerSaveDatas)
                {
                    GameObject villager = Instantiate(m_element, _canvasElementList.canvas_gameObjects[0].transform);
                    Canvas_Element_List element = villager.GetComponent<Canvas_Element_List>();
                    element.texts[0].text = data.name;
                    element.texts[1].text = "สุภาพ : " + (int)(data.health * 100) + "%";
                    element.texts[2].text = "ความหิว : " + (int)((1 - data.hunger) * 100) + "%";
                    element.texts[3].text = "ความพึงพอใจ : " + (int)(data.content * 100) + "%";
                    element.texts[4].text = "งาน : " + (PeopleJob)data.job;
                    
                    villagers.Add(villager);
                }
            }

            if (GM.gameInstance.workerSaveDatas.Count <= 0)
            {
                if (worker_empty == null)
                {
                    worker_empty = Instantiate(m_empty, _canvasElementList.canvas_gameObjects[1].transform);
                }
            }
            else
            {
                if(worker_empty != null)
                    Destroy(worker_empty);
                
                foreach (var worker in workers)
                {
                    Destroy(worker);
                }
                
                workers = new List<GameObject>();
                foreach (var data in GM.gameInstance.workerSaveDatas)
                {
                    GameObject worker = Instantiate(m_element, _canvasElementList.canvas_gameObjects[1].transform);
                    Canvas_Element_List element = worker.GetComponent<Canvas_Element_List>();
                    element.texts[0].text = data.name;
                    element.texts[1].text = "สุภาพ : " + (int)(data.health * 100) + "%";
                    element.texts[2].text = "ความหิว : " + (int)((1 - data.hunger) * 100) + "%";
                    element.texts[3].text = "ความพึงพอใจ : " + (int)(data.content * 100) + "%";
                    element.texts[4].text = "งาน : " + (PeopleJob)data.job;
                    
                    workers.Add(worker);
                }
            }
        }
    }
}