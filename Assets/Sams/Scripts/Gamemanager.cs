using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    [Header("General")]
    public LevelSystem level;


    public List<GameObject> playerPlanets;
    public List<GameObject> enemyPlanets;
    public List<GameObject> noOwnerPlanets;

    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
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

    public void Attack(GameObject planet, GameObject victim)
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
}
