using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nameFollow : MonoBehaviour
{
    //private Transform camera1;
    void Start()
    {
        //camera1 = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
