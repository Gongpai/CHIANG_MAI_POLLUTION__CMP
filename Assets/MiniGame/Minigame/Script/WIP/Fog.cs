using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float fogspeed =5f;

    private Vector3 fogdir = new Vector3(0, 0, 1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += fogdir * fogspeed * Time.deltaTime;
    }
}
