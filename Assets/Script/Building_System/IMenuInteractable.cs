using System.Collections.Generic;
using UnityEngine.Events;

namespace GDD
{
    public interface IMenuInteractable
    {
        public List<Button_Action_Data> GetInteractAction();
        public void Interact();
        public void ChangeEnableBuilding();
        public void OnRemoveBuilding(bool is_Cancel_Remove = false);
    }
}