using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using MixedReality.Toolkit.Input;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    [Header("General")]
    public static LevelSystem level;


    public static List<GameObject> playerPlanets =  new List<GameObject>();
    public static List<GameObject> enemyPlanets = new List<GameObject>();
    public static List<GameObject> noOwnerPlanets = new List<GameObject>();

    void Start()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        bool playerHasPlanet = false;
        bool enemyHasPlanet = false;
        if (level == null)
        {
            Debug.Log("No Level enetred");
            return;
        }

        if (level.sithShip == null)
        {
            Debug.Log("No sith ship entered");
            return;
        }

        if (level.jediShip == null)
        {
            Debug.Log("No jedi ship entered");
            return;
        }

        if (level.planets.Count < 2)
        {
            Debug.Log("Not enough planets");
            return;
        }

        for (int i = 0; i < level.planets.Count; i++)
        {
            if (level.planets[i].owner == Owner.Player)
            {
                playerHasPlanet = true;
            }
            if (level.planets[i].owner == Owner.Enemy)
                enemyHasPlanet = true;
        }

        if (!playerHasPlanet)
        {
            Debug.Log("Player does not have any planets to start with");
            return;
        }
        if (!enemyHasPlanet)
        {
            Debug.Log("Enemy does not have any planets to start with");
            return;
        }

        for (int i = 0; i < level.planets.Count; i++)
        {
            GameObject planet = Instantiate(level.planets[i].planet, level.planets[i].position, quaternion.identity);
            Planet planetScript = planet.GetComponent<Planet>();
            planetScript.timeBetweenShipSpawn = level.shipSpawnTime;
            if (level.planets[i].owner != Owner.None)
            {
                if (level.planets[i].owner == Owner.Player && level.playingAsJedi)
                {
                    planetScript.owner = Owner.Player;
                    planetScript.shipPrefab = level.jediShip;
                    playerPlanets.Add(planet);
                }
                else
                {
                    planetScript.owner = Owner.Enemy;
                    planetScript.shipPrefab = level.sithShip;
                    enemyPlanets.Add(planet);
                }
                for (int j = 0; j < level.startShipAmount - 1; j++)
                {
                    planetScript.SpawnShip();
                }
            }
            else
                noOwnerPlanets.Add(planet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyPlanets.Count == 0 || playerPlanets.Count == 0)
            Time.timeScale = 0;

        for (int i = 0; i < enemyPlanets.Count; i++)
        {
            if (enemyPlanets[i].GetComponent<Planet>().attacking)
                continue;

            for (int j = 0; j < noOwnerPlanets.Count; j++)
            {
                enemyPlanets[i].GetComponent<Planet>().attacking = true;
                Attack(enemyPlanets[i], noOwnerPlanets[j]);
                return;
            }
            for (int j = 0; j < playerPlanets.Count; j++)
            {
                if (enemyPlanets[i].transform.childCount > playerPlanets[j].transform.childCount * 1.5f)
                {
                    enemyPlanets[i].GetComponent<Planet>().attacking = true;
                    Attack(enemyPlanets[i], playerPlanets[j]);
                }
            }
        }
    }

    public static void Attack(GameObject planet, GameObject victim)
    {
        //Get all ships that will attack the victim planet
        List<GameObject> ships = new List<GameObject>();
        int i = 0;
        foreach (Transform child in planet.transform)
        {
            if (i % 2 == 0)
            {
                ships.Add(child.gameObject);
                child.parent = null;
                Ship ship = child.gameObject.GetComponent<Ship>();
                ship.orbiting = false;
                ship.target = victim.transform;
            }
            i++;
        }
    }

    public static bool IsPinching(XRNode hand)
    {
        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        aggregator.TryGetJoint(TrackedHandJoint.IndexTip, hand, out HandJointPose pose);

        bool handIsValid = aggregator.TryGetPinchProgress(hand, out bool isReadyToPinch, out bool isPinching, out float pinchAmount);
        return isPinching;
    }

    public static bool GetPointerPos(out GameObject hitObj, LayerMask mask)
    {
        hitObj = null;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * 1000f);
        RaycastHit hit;

        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.red, Time.deltaTime);

        if (Physics.Raycast(ray, out hit, 1000000, mask))
        {
            hitObj = hit.transform.gameObject;
            return true;
        }
        return false;
    }
}
