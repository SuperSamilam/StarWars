using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject shipPrefab;

    public float timeBetweenShipSpawn;

    void Start()
    {
        InvokeRepeating("SpawnShip", 0, timeBetweenShipSpawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnShip()
    {
        if (shipPrefab == null)
            return;

        GameObject ship = Instantiate(shipPrefab);
        ship.transform.parent = this.transform;
        ship.transform.localPosition = Vector3.right*0.02f;
        
        ship.GetComponent<Ship>().target = this.transform;
    }   
}
