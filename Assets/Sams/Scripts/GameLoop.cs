using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public LevelSystem level;

    public List<GameObject> playerPlanets = new List<GameObject>();
    public List<GameObject> enemyPlanets = new List<GameObject>();
    public List<GameObject> noOwnerPlanets = new List<GameObject>();

    void Start()
    {
        level = FindObjectOfType<LevelMaster>().level;
        LoadLevel();
        Ship.SetGamemanager();
    }

    //Loads teh level in
    void LoadLevel()
    {
        for (int i = 0; i < level.planets.Count; i++)
        {
            GameObject planet = Instantiate(level.planets[i].planet, level.planets[i].position, Quaternion.identity);
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
                else if (level.planets[i].owner == Owner.Player && !level.playingAsJedi)
                {
                    planetScript.owner = Owner.Player;
                    planetScript.shipPrefab = level.sithShip;
                    playerPlanets.Add(planet);
                }
                else if (level.playingAsJedi)
                {
                    planetScript.owner = Owner.Enemy;
                    planetScript.shipPrefab = level.sithShip;
                    enemyPlanets.Add(planet);
                }
                else
                {
                    planetScript.owner = Owner.Enemy;
                    planetScript.shipPrefab = level.jediShip;
                    enemyPlanets.Add(planet);
                }
                for (int j = 0; j < level.startShipAmount - 1; j++)
                {
                    planetScript.SpawnShip();
                }
            }
            else
            {
                noOwnerPlanets.Add(planet);
            }
        }
    }

    void Update()
    {   
        //Checking win conditions
        if (enemyPlanets.Count == 0)
        {
            SceneLoader.LoadScene(0);
        }
        if (playerPlanets.Count == 0)
        {
            SceneLoader.LoadScene(0);
        }

        //Trying to get enmenyships to attack
        for (int i = 0; i < enemyPlanets.Count; i++)
        {
            //It cant attack to often
            if (enemyPlanets[i].GetComponent<Planet>().haveAttacked)
            {
                continue;
            }

            //Attack a planet with no owner first as that is the smarter move to do
            if (noOwnerPlanets.Count >= 1)
            {
                enemyPlanets[i].GetComponent<Planet>().haveAttacked = true;
                int amountOfShips = (int)(enemyPlanets[i].transform.childCount * 0.5f);
                for (int s = 0; s < amountOfShips; s++)
                {
                    Ship ship = enemyPlanets[s].transform.GetChild(0).GetComponent<Ship>();
                    enemyPlanets[s].transform.GetChild(1).transform.parent = null;
                    ship.orbiting = false;
                    ship.target = noOwnerPlanets[0].transform;
                }
                continue;
            }

            //attack player planets if it is allowed to
            for (int j = 0; j < playerPlanets.Count; j++)
            {
                if (enemyPlanets[i].transform.childCount > playerPlanets[j].transform.childCount * 1.5f)
                {
                    enemyPlanets[i].GetComponent<Planet>().haveAttacked = true;
                    int amountOfShips = (int)(enemyPlanets[i].transform.childCount * 0.5f);
                    for (int s = 0; s < amountOfShips; s++)
                    {
                        Ship ship = enemyPlanets[s].transform.GetChild(0).GetComponent<Ship>();
                        enemyPlanets[s].transform.GetChild(0).transform.parent = null;
                        ship.LeaveOrbit();
                        ship.target = playerPlanets[j].transform;
                    }
                }
            }
        }
    }

    //CHanges owenr of a planet
    public void ModifyPlanetOwnership(GameObject planet, Owner owner)
    {
        playerPlanets.Remove(planet);
        noOwnerPlanets.Remove(planet);
        enemyPlanets.Remove(planet);

        if (owner == Owner.Enemy)
        {
            enemyPlanets.Add(planet);
        }
        else if (owner == Owner.Player)
        {
            playerPlanets.Add(planet);
        }
    }
}
