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

    Gamemanager gamemanager;
    [SerializeField] XRNode righthand;
    [SerializeField] XRNode leftHand;
    [SerializeField] LayerMask mask;

    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;


    bool pinching;
    bool firstPinch = false;

    //using to rember the planets states
    GameObject focusedPlanet;
    Material focusedMaterial;
    GameObject enemyPlanet;
    Material enemyMat;


    void Start()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Running");
        //Get the middle pos of the screen and make sure the pinch is this frame, first time and on your planet
        if (Gamemanager.GetPointerPos(out GameObject hit, mask))
        {
            Debug.Log("Yes");
            if (Gamemanager.IsPinching(righthand) && !pinching && gamemanager.WhoOwnsPlanet(Owner.Player, hit) && !firstPinch)
            {
                Debug.Log("Pinched");
                
                //Make sure focusplanet gets reset and assign a new one with the correct value
                if (focusedPlanet != null && focusedPlanet != hit)
                {
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
                    focusedPlanet = null;
                    pinching = Gamemanager.IsPinching(righthand);
                    return;
                }
                else if (focusedPlanet != null && focusedPlanet == hit)
                {
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

            //If another plaanet is hit, and a planet is selected
            if (gamemanager.WhoOwnsPlanet(Owner.None, hit) && firstPinch || gamemanager.WhoOwnsPlanet(Owner.Enemy, hit) && firstPinch)
            {
                //set up the vissulazation
                Renderer renderer = hit.gameObject.GetComponent<Renderer>();
                enemyPlanet = hit;
                enemyMat = renderer.material;
                renderer.materials = new Material[] { renderer.material, redMat };

                //attack and reset values
                if (Gamemanager.IsPinching(righthand) && !pinching)
                {
                    Gamemanager.Attack(focusedPlanet, enemyPlanet);
                    
                    enemyPlanet.GetComponent<Renderer>().materials = new Material[] { enemyMat };
                    enemyPlanet = null;
                }
            }
        } //pitch in the air is resetting everything
        else if (Gamemanager.IsPinching(righthand) && !pinching)
        {
            firstPinch = false;
            if (focusedPlanet != null)
                focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
            focusedPlanet = null;
        }
        else
        {
            if (enemyPlanet != null)
                enemyPlanet.GetComponent<Renderer>().materials = new Material[] { enemyMat };
            enemyPlanet = null;
        }

        //checking if im pinching this frame
        pinching = Gamemanager.IsPinching(righthand);
    }
}
