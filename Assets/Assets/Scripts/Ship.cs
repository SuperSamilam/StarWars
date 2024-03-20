using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    static float minSpeed = 30;
    static float maxSpeed = 40;
    public Transform target;
    public float speed = 20;

    float time = 0;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    void Update()
    {
        if (target == null)
            return;

        transform.RotateAround(target.position, Vector3.up, speed*Time.deltaTime);
    }

}
