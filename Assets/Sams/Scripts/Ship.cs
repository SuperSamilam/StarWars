using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //Static as everyship can use the same
    static Gamemanager gamemanager;
    static float minSpeed = 30;
    static float maxSpeed = 40;
    static float straightSpeed = 0.005f;

    float orbitSpeed;

    //public
    public bool orbiting = true;
    public Owner owner;
    public Transform target;

    public static void SetGamemanager()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
    }
    void Start()
    {
        orbitSpeed = Random.Range(minSpeed, maxSpeed);
    }

    //Making sure the ship is doing the right movement at the right time
    void Update()
    {
        if (target == null)
            return;

        if (orbiting)
        {
            transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, straightSpeed);
            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
                orbiting = true;
                transform.parent = target;
                transform.localPosition = Vector3.right * 0.02f;
                TryTakeoverPlanet();
            }
        }
    }

    void TryTakeoverPlanet()
    {
        //Going backwars as children will be destroyed
        for (int i = target.childCount - 1; i >= 0; i--)
        {
            //If the planets on the same planet does not have the same parent, destory it
            if (target.GetChild(i).GetComponent<Ship>().owner != owner)
            {
                Destroy(target.GetChild(i).gameObject);
                Destroy(gameObject);
                return;
            }
        }

        //The planet can change owner
        gamemanager.ModifyPlanetOwnership(target.gameObject, owner);

        //Make sure the player uses the right ship prefab
        target.gameObject.GetComponent<Planet>().owner = owner;
        if (owner == Owner.Player && Gamemanager.level.playingAsJedi)
            target.gameObject.GetComponent<Planet>().shipPrefab = Gamemanager.level.jediShip;
        else if (owner == Owner.Player)
            target.gameObject.GetComponent<Planet>().shipPrefab = Gamemanager.level.sithShip;
        else if (owner == Owner.Enemy && Gamemanager.level.playingAsJedi)
            target.gameObject.GetComponent<Planet>().shipPrefab = Gamemanager.level.sithShip;
        else if (owner == Owner.Enemy)
            target.gameObject.GetComponent<Planet>().shipPrefab = Gamemanager.level.jediShip;
    }

}
