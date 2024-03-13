using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour
{

    bool leftActivated = false;
    [SerializeField] GameObject leftSaber;
    [SerializeField] GameObject leftBlade;

    bool rightActivated = false;
    [SerializeField] GameObject rightSaber;
    [SerializeField] GameObject rightBlade;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     leftActivated = true;
        // }

        if (Input.GetMouseButtonDown(1))
        {
            rightActivated = false;
        }


        // if (leftActivated)
        // {
        //     leftBlade.SetActive(true);
        // }
        // else
        //     leftBlade.SetActive(false);


        if (rightActivated)
        {
            rightBlade.SetActive(true);
        }
        else
            rightBlade.SetActive(false);
    }
}
