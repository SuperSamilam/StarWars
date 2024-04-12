using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Manager manager;
    [SerializeField] GameLoop gameLoop;
    [SerializeField] LayerMask planetLayer;
    [SerializeField] Material selectionMat;
    [SerializeField] Material enemySelectionMat;

    bool planetSelected = false;
    GameObject planet;
    Material planetMat;

    GameObject enemyPlanet;
    Material enemyPlanetMat;

    void Update()
    {
        GameObject tempPlanet;
        if (Manager.GetPointerPos(out tempPlanet, planetLayer, true))
        {
            if (!gameLoop.playerPlanets.Contains(tempPlanet))
            {
                CheckEnemyPlanet(tempPlanet);
                CheckForAttack();
                return;
            }
            else
            {
                ResetPlanet(enemyPlanet);
            }

            if (tempPlanet != planet)
            {
                planetSelected = false;
                ResetPlanet(planet);
            }

            planet = tempPlanet;
            Renderer renderer = planet.gameObject.GetComponent<Renderer>();
            planetMat = renderer.material;
            renderer.materials = new Material[] { renderer.material, selectionMat };

            if (manager.pinchingRight != manager.pinchingRightLastFrame)
            {
                planetSelected = true;
            }
        }
        else if (!planetSelected)
        {
            ResetPlanet(planet);
        }
        ResetPlanet(enemyPlanet);

    }

    void CheckForAttack()
    {
        if (manager.pinchingRight && manager.pinchingRightLastFrame && planetSelected == true)
        {
            int amountOfShips = (int)(planet.transform.childCount * 0.5f);
            for (int i = 0; i < amountOfShips; i++)
            {
                Ship ship = planet.transform.GetChild(0).GetComponent<Ship>();
                planet.transform.GetChild(0).transform.parent = null;
                ship.LeaveOrbit();
                ship.target = enemyPlanet.transform;
            }
        }
    }

    void CheckEnemyPlanet(GameObject tempPlanet)
    {
        Debug.Log("planet");
        if (gameLoop.enemyPlanets.Contains(tempPlanet) || gameLoop.noOwnerPlanets.Contains(tempPlanet))
        {
            enemyPlanet = tempPlanet;
            Renderer renderer = enemyPlanet.gameObject.GetComponent<Renderer>();
            enemyPlanetMat = renderer.material;
            renderer.materials = new Material[] { renderer.material, enemySelectionMat };
        }
    }
    void ResetPlanet(GameObject planet)
    {
        if (planet == null)
            return;
        if (planet == this.planet)
        {
            planet.GetComponent<Renderer>().materials = new Material[] { planetMat };
            this.planet = null;
        }
        else
        {
            planet.GetComponent<Renderer>().materials = new Material[] { enemyPlanetMat };
            enemyPlanet = null;
        }
    }
}
