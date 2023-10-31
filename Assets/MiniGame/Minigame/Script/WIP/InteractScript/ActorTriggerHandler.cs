using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ActorTriggerHandler : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> m_TriggeredGameObjects = new List<GameObject>();
        [SerializeField] protected GameObject m_Actor;

        protected virtual void Start()
        {
            if (m_Actor == null)
            {
                m_Actor = transform.parent.gameObject;
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var interactableComponents = other.GetComponents<IInteractable>();

            if (interactableComponents.Length > 0)
            {
                foreach (var ic in interactableComponents)
                {
                    if (ic is IActorEnterExitHandler enterExitHandler)
                    {
                        enterExitHandler.ActorEnter(m_Actor);
                    }
                }

                m_TriggeredGameObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // เพิ่มโค้ดสำหรับการอยู่ในสถานะตัวตรวจสอบ (on stay) ตามที่คุณต้องการ
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            var interactableComponents = other.GetComponents<IInteractable>();

            if (interactableComponents.Length > 0)
            {
                foreach (var ic in interactableComponents)
                {
                    if (ic is IActorEnterExitHandler enterExitHandler)
                    {
                        enterExitHandler.ActorExit(m_Actor);
                    }
                }

                m_TriggeredGameObjects.RemoveAll(gameObj => gameObj == other.gameObject);
            }
        }

        public virtual IInteractable[] GetInteractables()
        {
            // ลบ GameObject ที่เป็น null ออกจากรายการ
            m_TriggeredGameObjects.RemoveAll(gameObj => gameObj == null);

            if (m_TriggeredGameObjects.Count == 0)
            {
                return null;
            }

            var interactables = new List<IInteractable>();
            foreach (var gameObject in m_TriggeredGameObjects)
            {
                var interactableComponents = gameObject.GetComponents<IInteractable>();
                if (interactableComponents.Length > 0)
                {
                    interactables.AddRange(interactableComponents);
                }
            }

            return interactables.ToArray();
        }
    }
}
