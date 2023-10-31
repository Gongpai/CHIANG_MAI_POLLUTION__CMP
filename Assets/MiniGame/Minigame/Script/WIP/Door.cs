using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Door : MonoBehaviour
{
    [Header("RoomSpawn")]
    public GameObject roomToSpawn;
    public Transform spawnPoint; // กำหนดจุดที่วัตถุจะเกิดขึ้น
    [Header("ObjectsSpawn")]
    public GameObject objectToSpawn; // กำหนดวัตถุที่จะเกิดขึ้น
    public int numberOfObjectsToSpawn = 5;
    public Transform spawnArea;// จำนวนวัตถุที่ต้องการสร้าง
    private List<GameObject> objectPool = new List<GameObject>();


    
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnRoom();
            SpawnObjects();
            Destroy(this);
        }
    }

    private void SpawnRoom()
    {
        Instantiate(roomToSpawn, spawnPoint.position, spawnPoint.rotation);
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            // หาวัตถุที่ไม่ถูกใช้งานใน Object Pool
            GameObject spawnedObject = GetPooledObject();

            if (spawnedObject != null)
            {
                // สุ่มตำแหน่งในพื้นที่สุ่มการเกิด
                Vector3 randomSpawnPosition = new Vector3(
                    Random.Range(spawnArea.position.x - spawnArea.localScale.x / 1, spawnArea.position.x + spawnArea.localScale.x / 1),
                    Random.Range(spawnArea.position.y - spawnArea.localScale.y / 1, spawnArea.position.y + spawnArea.localScale.y / 1),
                    Random.Range(spawnArea.position.z - spawnArea.localScale.z / 1, spawnArea.position.z + spawnArea.localScale.z / 1)
                );

                // เปิดใช้งานวัตถุและตั้งตำแหน่ง
                spawnedObject.transform.position = randomSpawnPosition;
                spawnedObject.SetActive(true);
            }
            else
            {
                // ถ้า Object Pool ว่างเปล่า สร้างวัตถุใหม่และเพิ่มลงใน Object Pool
                spawnedObject = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);
                spawnedObject.SetActive(false);
                objectPool.Add(spawnedObject);

                // สุ่มตำแหน่งในพื้นที่สุ่มการเกิด
                Vector3 randomSpawnPosition = new Vector3(
                    Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                    Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2),
                    Random.Range(spawnArea.position.z - spawnArea.localScale.z / 2, spawnArea.position.z + spawnArea.localScale.z / 2)
                );

                // เปิดใช้งานวัตถุและตั้งตำแหน่ง
                spawnedObject.transform.position = randomSpawnPosition;
                spawnedObject.SetActive(true);
            }
        }
    }
    private GameObject GetPooledObject()
    {
        // ค้นหาวัตถุใน Object Pool ที่ไม่ถูกใช้งาน
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null; // ถ้าไม่มีวัตถุใน Object Pool ที่ไม่ถูกใช้งาน
    }
}
