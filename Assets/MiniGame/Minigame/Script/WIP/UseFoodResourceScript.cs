using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class UseFoodResourceScript : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_input_food;
        [SerializeField] private List<Button> m_buttons;
        [SerializeField] private GameAppFlowManager GAFM;

        private ResourcesManager RM;
        private GameManager GM;
        private SaveManager SM;
        private NotificationManager NM;
        private LoadingSceneSystem m_loadingSceneSystem;
        private int food = 0;
        
        private void Start()
        {
            RM = ResourcesManager.Instance;
            GM = GameManager.Instance;
            SM = SaveManager.Instance;
            NM = NotificationManager.Instance;
            m_loadingSceneSystem = FindObjectOfType<LoadingSceneSystem>();
            
            m_input_food.onValueChanged.AddListener(arg =>
            {
                food = int.Parse(arg);
            });
            
            m_buttons[0].onClick.AddListener((() =>
            {
                if (RM.Can_Set_Resources_Food(-food))
                {
                    RM.Set_Resources_Food(-food);
                    Debug.LogWarning("Food Current : " + RM.Get_Resources_Food());
                    GAFM.RandomLoadScene(0);
                }
                else
                {
                    Notification notification = new Notification();
                    notification.text = "Food not enough";
                    notification.icon = Resources.Load<Sprite>("Icon/save_icon");
                    notification.iconColor = Color.white;
                    notification.duration = 5.0f;
                    notification.isWaitTrigger = false;
                    NM.AddNotification(notification); 
                }
            }));
            
            m_buttons[1].onClick.AddListener(() =>
            {
                var saveData = SM.LoadGamePreferencesData(GM.gameInstance.savefile_backtocity, GM.gameInstance.GetType());

                GM.GetSetGameManager = saveData;
                GM.OnGameLoad();
                
                m_loadingSceneSystem.LoadScene("GameLevel");
            });
        }

        private void Update()
        {
            
        }
    }
}