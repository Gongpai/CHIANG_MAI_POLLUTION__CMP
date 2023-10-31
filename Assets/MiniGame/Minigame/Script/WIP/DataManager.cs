using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance => _instance;

    // สร้างตัวแปรสำหรับเก็บค่า timeResult
    public TMP_Text timeResultText;

    private void Awake()
    {
        // ตรวจสอบว่ามี Instance แล้วหรือยัง ถ้ายังไม่มีให้สร้าง Instance
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // อย่าทำลาย GameObject นี้เมื่อโหลด Scene ใหม่
        }
        else
        {
            Destroy(gameObject);
        }
    }
}