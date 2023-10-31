using System;
using UnityEngine;
using TMPro; // นำเข้าไลบรารี TextMeshPro

namespace GDD
{
    public class InventoryUI : MonoBehaviour
    {
        public TextMeshProUGUI inventoryText; // ให้แทนค่านี้ด้วย TextMeshPro Text ที่คุณสร้างขึ้น

        private Ch9Inventory inventory; // ให้แทนค่านี้ด้วยคลาสที่มีการจัดการกับอินเวนทอรี

        private void Start()
        {
            // หากอินเวนทอรีอยู่ในอินสแตนซ์เดียวกับสคริปต์นี้
            // คุณสามารถใช้คำสั่งด้านล่างเพื่อหาคลาสอินเวนทอรี
            inventory = GetComponent<Ch9Inventory>();

            // หากอินเวนทอรีไม่อยู่ในอินสแตนซ์เดียวกับสคริปต์นี้
            // คุณสามารถเชื่อมโยงมันใน Inspector และกำหนดค่าให้กับตัวแปร inventory ที่นี่

            // เรียกใช้ฟังก์ชัน UpdateUI เพื่ออัปเดต UI ของคุณ
            UpdateUI();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // ตรวจสอบว่ามี TextMeshPro Text ที่ต้องการใช้งาน
            if (inventoryText != null)
            {
                // อ่านข้อมูลจากอินเวนทอรี
                string inventoryInfo = "Inventory:\n";
                foreach (var key in inventory.GetKeys())
                {
                    int amount = inventory.GetItemAmount(key);
                    inventoryInfo += $"{key}: {amount}\n";
                }

                // กำหนดข้อความ UI
                inventoryText.text = inventoryInfo;
            }
        }
    }
}