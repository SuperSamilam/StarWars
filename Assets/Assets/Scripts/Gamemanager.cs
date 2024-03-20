using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    [Header("General")]
    public LevelSystem level;

    public List<GameObject> playerPlanets;
    public List<GameObject> enemyPlanets;
    public List<GameObject> noOwnerPlanets;

    void Start()
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
                    planetScript.shipPrefab = level.jediShip;
                    playerPlanets.Add(planet);
                }
                else
                {
                    planetScript.shipPrefab = level.sithShip;
                    enemyPlanets.Add(planet);
                }
                for (int j = 0; j < level.startShipAmount; j++)
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

    }
}
