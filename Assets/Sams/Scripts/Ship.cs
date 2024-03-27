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
        }
        else
        {
            //transform.LookAt(target.position);
            transform.position = Vector3.MoveTowards(transform.position, target.position, straightSpeed);
            if (Vector3.Distance(transform.position, target.position) <= 0.1f)
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
        for (int i = target.childCount - 1; i >= 0; i--)
        {
            Debug.Log("running");
            if (target.GetChild(i).GetComponent<Ship>().owner != owner)
            {
                Debug.Log("In here");
                Destroy(target.GetChild(i).gameObject);
                Destroy(this.gameObject);
                return;
            }
        }
        Debug.Log("exited");
        //The planet can change owner



        if (owner == Owner.Enemy)
        {
            Gamemanager.noOwnerPlanets.Remove(target.gameObject);
            Gamemanager.playerPlanets.Remove(target.gameObject);
            Gamemanager.enemyPlanets.Add(target.gameObject);
        }
        else
        {
            Gamemanager.noOwnerPlanets.Remove(target.gameObject);
            Gamemanager.enemyPlanets.Remove(target.gameObject);
            Gamemanager.playerPlanets.Add(target.gameObject);
        }

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
