using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float z;

    void Start()
    {
        transform.position = new Vector3(0, 0, z);
    }

    
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, z);
    }
}
