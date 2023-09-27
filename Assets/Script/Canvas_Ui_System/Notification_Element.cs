using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class Notification_Element : MonoBehaviour
    {
        [SerializeField] private Canvas_Element_List m_elementList;
        private Ui_Utilities _uiUtilities;
        private float current_time;
        private Notification m_notification;
        private NotificationManager NM;

        public Notification notification
        {
            set
            {
                current_time = value.duration;
                m_notification = value;
            }
        }

        private void Start()
        {
            NM = NotificationManager.Instance;
            _uiUtilities = GetComponent<Ui_Utilities>();
            _uiUtilities.canvasUI = gameObject;

            if (m_notification.soundSFX != null)
            {
                m_elementList.audioSources[0].clip = m_notification.soundSFX;
                m_elementList.audioSources[0].Play();
            }

            m_elementList.animators[0].SetBool("IsStart", true);
        }

        private void Update()
        {
            m_elementList.texts[0].text = m_notification.text;
            m_elementList.image[0].sprite = m_notification.icon;
            m_elementList.image[0].color = m_notification.iconColor;

            if (!m_notification.isWaitTrigger)
            {
                if (current_time > 0)
                    current_time -= Time.deltaTime;
                else if(m_elementList.animators[0].GetBool("IsStart"))
                    DestroyNotification();
            }
            else
            {
                if(m_notification.isTrigger && m_elementList.animators[0].GetBool("IsStart"))
                    DestroyNotification();
            }
        }

        private void DestroyNotification()
        {
            NM.RemoveNotification(new Tuple<Notification, GameObject>(m_notification, gameObject));
            _uiUtilities.RemoveUI(0.75f);
        }
    }
}