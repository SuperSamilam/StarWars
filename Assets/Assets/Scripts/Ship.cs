using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Ship : MonoBehaviour
{
    static float minSpeed = 30;
    static float maxSpeed = 40;
    static float straightSpeed = 0.005f;
    public Transform target;
    public float orbitSpeed = 20;
    public bool orbiting = true;
    public Owner owner;

    float time = 0;

    void Start()
    {
        orbitSpeed = Random.Range(minSpeed, maxSpeed);
    }
    void Update()
    {
        if (target == null)
            return;

        if (orbiting)
        {
            transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
            Debug.Log("orbitin");
        }
        else
        {
            Debug.Log("traveling");
            transform.LookAt(target.position);
            transform.Translate(Vector3.Normalize(target.position - transform.position) * straightSpeed);
            if (Vector3.Distance(transform.position, target.position) <= 0.02f)
            {
                orbiting = true;
                transform.localPosition = Vector3.right * 0.02f;
            }
        }
    }

}
