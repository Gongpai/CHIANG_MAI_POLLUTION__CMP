using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    

    public class GenericInteractable : MonoBehaviour , IInteractable , IActorEnterExitHandler
    {
        // Start is called before the first frame update
        [SerializeField] protected UnityEvent<GameObject> m_OnInteract = new();
        [SerializeField] protected UnityEvent<GameObject> m_OnActorEnter = new();
        [SerializeField] protected UnityEvent<GameObject> m_OnActorExit = new();

        public virtual void Interact(GameObject actor)
        {
            m_OnInteract.Invoke(actor);
        }
    
        public virtual void ActorEnter(GameObject actor)
        {
            m_OnActorEnter.Invoke(actor);
        }
        public virtual void ActorExit(GameObject actor)
        {
            m_OnActorExit.Invoke(actor);
        }

        public void OnPush(GameObject actor)
        {
            
        }
    }
}
