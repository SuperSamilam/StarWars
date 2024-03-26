using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject shipPrefab;
    public float timeBetweenShipSpawn;
    public Owner owner;
    public bool attacking = false;

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
        attacking = false;
        if (shipPrefab == null)
            return;

        GameObject ship = Instantiate(shipPrefab);
        ship.transform.parent = this.transform;
        ship.transform.localPosition = Vector3.right * 0.02f;

        ship.GetComponent<Ship>().target = this.transform;
        ship.GetComponent<Ship>().owner = owner;
    }
}
