using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 20;
    
    void Start()
    {
        
    }
    void Update()
    {
        if (target == null)
            return;
            
        transform.RotateAround(target.position, Vector3.up, rotateSpeed*Time.deltaTime);
    }
}
