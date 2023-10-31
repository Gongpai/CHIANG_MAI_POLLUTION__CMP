using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class RoomGenerated : MonoBehaviour
{
    [SerializeField] public GameObject lRoom;
    [SerializeField] public GameObject fRoom;
    [SerializeField] public GameObject rRoom;
    enum RoomDir
    {
        Left,
        Forward,
        Right
    };

    public int x, y, z, r;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    RoomDir RoomGen(RoomDir roomDir)
    {
        if (roomDir == RoomDir.Left)
        {
            
            r = 90;
            Instantiate(lRoom, new(x,y,z), new Quaternion(0,r,0,0));
        }
        else if (roomDir == RoomDir.Forward)
        {
            
            
            Instantiate(fRoom, new(x,y,z), new Quaternion(0,r,0,0));
        }
        else if (roomDir == RoomDir.Right)
        {
            Instantiate(rRoom, new(x,y,z), new Quaternion(0,r,0,0));
        }

        return roomDir;
    }

    private void SpawnNewRoom(Vector3 spawnPos)
    {
        
    }
}
