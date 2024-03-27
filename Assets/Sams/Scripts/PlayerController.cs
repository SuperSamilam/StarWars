using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using UnityEngine.XR;
using MixedReality.Toolkit.Subsystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public XRNode righthand;
    public XRNode leftHand;
    public LayerMask mask;

    public Material greenMat;
    public Material redMat;


    bool pinching;
    GameObject focusedPlanet;
    Material focusedMaterial;

    GameObject enemyPlanet;
    Material enemyMat;

    bool firstPinch = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Gamemanager.GetPointerPos(out GameObject hit, mask))
        {
            if (Gamemanager.IsPinching(righthand) && !pinching && Gamemanager.playerPlanets.Contains(hit) && !firstPinch)
            {
                
                if (focusedPlanet != null && focusedPlanet != hit)
                {
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
                    focusedPlanet = null;
                    pinching = Gamemanager.IsPinching(righthand);
                    return;
                }

                if (focusedPlanet != null && focusedPlanet == hit)
                {
                    print(focusedMaterial.name);
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
                    focusedPlanet = null;
                    firstPinch = false;
                    pinching = Gamemanager.IsPinching(righthand);
                    return;
                }

                Renderer renderer = hit.gameObject.GetComponent<Renderer>();
                focusedPlanet = hit;
                focusedMaterial = renderer.material;
                renderer.materials = new Material[] { renderer.material, greenMat };
                firstPinch = true;
            }

            if (Gamemanager.noOwnerPlanets.Contains(hit) && firstPinch || Gamemanager.enemyPlanets.Contains(hit) && firstPinch)
            {
                Renderer renderer = hit.gameObject.GetComponent<Renderer>();
                enemyPlanet = hit;
                enemyMat = renderer.material;
                renderer.materials = new Material[] { renderer.material, redMat };
                
                if (Gamemanager.IsPinching(righthand) && !pinching)
                {
                    Gamemanager.Attack(focusedPlanet, enemyPlanet);
                    
                    if (enemyPlanet != null)
                        enemyPlanet.GetComponent<Renderer>().materials = new Material[] { enemyMat };
                    enemyPlanet = null;
                }
            }
        }
        else if (Gamemanager.IsPinching(righthand) && !pinching)
        {
            Debug.Log("notpiching anymore");
            firstPinch = false;
            if (focusedPlanet != null)
                focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
            focusedPlanet = null;
            pinching = Gamemanager.IsPinching(righthand);
            return;
        }
        else
        {
            if (enemyPlanet != null)
                enemyPlanet.GetComponent<Renderer>().materials = new Material[] { enemyMat };
            enemyPlanet = null;
        }


        pinching = Gamemanager.IsPinching(righthand);
    }
}
