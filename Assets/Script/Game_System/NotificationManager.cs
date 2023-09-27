using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class NotificationManager : Sinagleton_CanDestroy<NotificationManager>
    {
        [SerializeField] private GameObject m_noti_element;
        [SerializeField] private GameObject m_list_element;
        
        private List<Tuple<Notification, GameObject>> _notifications = new ();

        public void AddNotification(Notification _notification)
        {
            _notifications.Add(new Tuple<Notification, GameObject>(_notification, null));
        }

        public void RemoveNotification(Tuple<Notification, GameObject> _notification)
        {
            _notifications.Remove(_notification);
        }

        private void Start()
        {
            Notification notification = new Notification();
            notification.text = "Load Scene With Save Data";
            notification.icon = Resources.Load<Sprite>("Icon/settings_icon");
            notification.iconColor = Color.white;
            notification.duration = 5.0f;
            notification.isWaitTrigger = false;
            AddNotification(notification);
        }

        private void Update()
        {
            if (_notifications.Count > 0)
            {
                for (int i = 0; i < _notifications.Count; i++)
                {
                    if (_notifications[i].Item2 == null)
                    {
                        GameObject element = Instantiate(m_noti_element, m_list_element.transform);
                        element.GetComponent<Notification_Element>().notification = _notifications[i].Item1;

                        _notifications[i] = new Tuple<Notification, GameObject>(_notifications[i].Item1, element);
                    }
                }
            }
        }
    }
}