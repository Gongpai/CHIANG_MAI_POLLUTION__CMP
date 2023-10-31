using UnityEngine;

namespace GDD
{
    [RequireComponent(typeof(ItemTypeComponent))]
    public class Pickupable : MonoBehaviour, IInteractable, IActorEnterExitHandler
    {
        public void Interact(GameObject actor)
        {
            //myself
            var itemTypeComponent = GetComponent<ItemTypeComponent>();
            //actor
            var inventory = actor.GetComponent<IInventory>();
            inventory.AddItem(itemTypeComponent.Type.ToString(), 1);

            Destroy(gameObject);
        }

        public void ActorEnter(GameObject actor)
        {

        }

        public void ActorExit(GameObject actor)
        {

        }

        public void OnPush(GameObject actor)
        {
            
        }
    }
}