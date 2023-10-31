using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GDD
{
    public class Ch9Inventory : MonoBehaviour, IInventory
    {
        protected Dictionary<string, int> m_ItemInventory = new();

        public void AddItem(string keyItemName, object item)
        {
            //Check the existence of the key itemName
            if (m_ItemInventory.ContainsKey(keyItemName))
            {
                m_ItemInventory[keyItemName] += 1;
                Debug.Log(
                    $"Existing item {keyItemName} amount increased by 1. (Current = {m_ItemInventory[keyItemName]})");
            }
            else
            {
                m_ItemInventory.Add(keyItemName, 1);
                Debug.Log($"New item added. ({keyItemName})");
            }
        }

        public void RemoveItem(string keyItemName)
        {
            m_ItemInventory.Remove(keyItemName);
            Debug.Log($"{keyItemName} has been removed.");
        }

        public void IncreaseItemAmount(string keyItemName, int amount)
        {
            if (!m_ItemInventory.ContainsKey(keyItemName)) return;

            m_ItemInventory[keyItemName] += amount;

            Debug.Log($"{keyItemName} has been increased by {amount}. (Current = {m_ItemInventory[keyItemName]})");
        }

        public void DecreaseItemAmount(string keyItemName, int amount)
        {
            if (!m_ItemInventory.ContainsKey(keyItemName)) return;

            m_ItemInventory[keyItemName] -= amount;

            Debug.Log($"{keyItemName} has been decreased by {amount}. (Current = {m_ItemInventory[keyItemName]})");
        }

        public int GetItemAmount(string keyItemName)
        {
            return m_ItemInventory[keyItemName];
        }

        public List<string> GetKeys()
        {
            return m_ItemInventory.Keys.ToList();
        }
/*
        private void OnGUI()
        {
            DrawItemInventory();
        }

        private void DrawItemInventory()
        {
            int y = 0;
            int ySpacing = 15;
            int height = 20;
            GUI.color = Color.black;
            foreach (var s in m_ItemInventory)
            {
                GUI.Label(new Rect(5, y += ySpacing, 150, height), s.Key);
            }
        }*/
        
    }

}