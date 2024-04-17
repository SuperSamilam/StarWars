using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //Static as everyship can use the same
    static GameLoop gameLoop;
    static float minSpeed = 30;
    static float maxSpeed = 40;
    static float straightSpeed = 0.025f;

    float orbitSpeed;
    [SerializeField] AudioSource source;
    [SerializeField] GameObject blasters;

    //public
    public bool orbiting = true;
    public Owner owner;
    public Transform target;

    public static void SetGamemanager()
    {
        gameLoop = FindObjectOfType<GameLoop>();
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
            transform.LookAt(target.position);
            if (Vector3.Distance(transform.position, target.position) <= 1f)
            {
                orbiting = true;
                transform.parent = target;
                transform.localPosition = Vector3.right * 0.02f;
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                CancelInvoke();
                TryTakeoverPlanet();
            }
        }
    }
    
    //Leaving the orbit of a planet to attack another
    public void LeaveOrbit()
    {
        orbiting = false;
        InvokeRepeating("Shoot", 0.5f, 1f);
    }

    void Shoot()
    {
        source.Play();
        StartCoroutine("ShootShot");
    }

    IEnumerator ShootShot()
    {
        blasters.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        blasters.SetActive(false);
    }


    //Looks at the amount of ships for a planet and decide wich team has the most
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

        // //The planet can change owner
        gameLoop.ModifyPlanetOwnership(target.gameObject, owner);

        // //Make sure the player uses the right ship prefab
        target.gameObject.GetComponent<Planet>().owner = owner;
        if (owner == Owner.Player && gameLoop.level.playingAsJedi)
            target.gameObject.GetComponent<Planet>().shipPrefab = gameLoop.level.jediShip;
        else if (owner == Owner.Player)
            target.gameObject.GetComponent<Planet>().shipPrefab = gameLoop.level.sithShip;
        else if (owner == Owner.Enemy && gameLoop.level.playingAsJedi)
            target.gameObject.GetComponent<Planet>().shipPrefab = gameLoop.level.sithShip;
        else if (owner == Owner.Enemy)
            target.gameObject.GetComponent<Planet>().shipPrefab = gameLoop.level.jediShip;
    }

}
