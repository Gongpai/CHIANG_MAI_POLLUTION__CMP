using UnityEngine;
using TMPro;

public class FoodTimer : MonoBehaviour
{
    public TMP_InputField foodInputField; // ใช้สำหรับป้อนค่าตัวเลข
    public TMP_Text timeResult; // ใช้แสดงผลลัพธ์
    public GameData gameData;
    private void Start()
    {
        // เพิ่ม Listener เมื่อค่าใน Input Field เปลี่ยน
        foodInputField.onValueChanged.AddListener(UpdateResult);
    }

    private void UpdateResult(string newValue)
    {
        if (int.TryParse(newValue, out int number))
        {
            int result = number * 2;
            gameData.playerTime = result;
            timeResult.text = ("Time:"+ result.ToString()+"Sec");
        }
        else
        {
            timeResult.text = "Time:0";
        }
    }
}