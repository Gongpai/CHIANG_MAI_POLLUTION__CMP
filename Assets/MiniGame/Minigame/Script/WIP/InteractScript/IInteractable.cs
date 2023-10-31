using UnityEngine;

namespace GDD
{
    public interface IInteractable
    {
        public void Interact(GameObject actor);
        public void OnPush(GameObject actor);
    }
}