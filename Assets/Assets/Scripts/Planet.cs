using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject shipPrefab;

    [SerializeField] float timeBetweenShipSpawn;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnShip", 1f, 30f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnShip()
    {
        if (shipPrefab == null)
            return;

        GameObject ship = Instantiate(shipPrefab);
        ship.GetComponent<Ship>().target = this.transform;

        
    }   
}
