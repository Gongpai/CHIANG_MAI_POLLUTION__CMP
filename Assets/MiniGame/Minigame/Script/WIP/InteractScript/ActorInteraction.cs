using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GDD
{
    public class ActorInteraction : MonoBehaviour
    {
        [SerializeField] protected ActorTriggerHandlerV2 m_ActorTriggerHandler;

        [SerializeField] protected Key m_InteractionKey;

        [SerializeField] protected UnityEvent m_OnStartInteract = new();

        // Start is called before the first frame update
        void Start()
        {
            if (m_ActorTriggerHandler == null)
            {
                m_ActorTriggerHandler = GetComponentInChildren<
                    ActorTriggerHandlerV2>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard[m_InteractionKey].wasPressedThisFrame)
            {
                PerformInteraction();
            }
        }

        protected virtual void PerformInteraction()
        {
            var interactables = m_ActorTriggerHandler.GetInteractables();

            if (interactables?.Length > 0)
            {
                //Call registered UnityAction(s)
                m_OnStartInteract.Invoke();

                foreach (var interactable in interactables)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}