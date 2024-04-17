using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] Manager manager;
    [SerializeField] LevelMaster levelMaster;
    [SerializeField] LayerMask planetLayer;
    [SerializeField] Material selectionMat;

    GameObject planet;
    Material planetMat;

    //Gets if the player is pointing to any planet and is pinching to start the level
    void Update()
    {
        GameObject tempPlanet;
        if (Manager.GetPointerPos(out tempPlanet, planetLayer, true))
        {
            if (tempPlanet != planet)
            {
                ResetPlanet();
            }

            planet = tempPlanet;
            Renderer renderer = planet.gameObject.GetComponent<Renderer>();
            planetMat = renderer.material;
            renderer.materials = new Material[] { renderer.material, selectionMat };

            if (manager.pinchingRight != manager.pinchingRightLastFrame)
            {
                LevelLoader levelLoader = planet.GetComponent<LevelLoader>();
                if (levelLoader == null)
                    return;
                if (levelLoader.level != null)
                    levelMaster.level = levelLoader.level;
                SceneLoader.LoadScene(levelLoader.sceneIndex);
            }
        }
        else
        {
            ResetPlanet();
        }
    }

    void ResetPlanet()
    {
        if (planet == null)
            return;
        planet.GetComponent<Renderer>().materials = new Material[] { planetMat };
        planet = null;
    }
}
