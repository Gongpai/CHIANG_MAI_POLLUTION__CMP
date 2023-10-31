using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{


    public interface IInventory
    {
        void AddItem(string keyItemName, object item);
        void RemoveItem(string keyItemName);

        void IncreaseItemAmount(string keyItemName, int amount);
        void DecreaseItemAmount(string keyItemName, int amount);

        int GetItemAmount(string keyItemName);
    
        List<string> GetKeys();
    }
}




