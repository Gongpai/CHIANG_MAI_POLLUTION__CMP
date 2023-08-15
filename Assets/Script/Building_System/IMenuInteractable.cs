using System.Collections.Generic;
using UnityEngine.Events;

namespace GDD
{
    public interface IMenuInteractable
    {
        public List<Menu_Data> GetInteractAction();
        public void Interact();
        public void ChangeEnableBuilding();
        public void RemoveBuilding();
    }
}