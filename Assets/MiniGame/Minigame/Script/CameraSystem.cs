using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float roDirection = 0f;
        if (Input.GetKey(KeyCode.Q)) roDirection -= 1f;
        if (Input.GetKey(KeyCode.E)) roDirection += 1f;

        float roSpeed = 100f;
        transform.eulerAngles += new Vector3(0, roDirection * roSpeed * Time.deltaTime, 0);
    }
}
