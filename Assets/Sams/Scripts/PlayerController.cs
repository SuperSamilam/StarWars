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

    public Gamemanager manager;

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

        if (GetPointerPos(out GameObject hit))
        {
            if (IsPinching(righthand) && !pinching && manager.playerPlanets.Contains(hit) && !firstPinch)
            {
                Debug.Log("first");
                if (focusedPlanet != null && focusedPlanet != hit)
                {
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
                    focusedPlanet = null;
                    pinching = IsPinching(righthand);
                    return;
                }

                if (focusedPlanet != null && focusedPlanet == hit)
                {
                    print(focusedMaterial.name);
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
                    focusedPlanet = null;
                    firstPinch = false;
                    pinching = IsPinching(righthand);
                    return;
                }

                Renderer renderer = hit.gameObject.GetComponent<Renderer>();
                focusedPlanet = hit;
                focusedMaterial = renderer.material;
                renderer.materials = new Material[] { renderer.material, greenMat };
                firstPinch = true;
            }

            if (manager.noOwnerPlanets.Contains(hit) && firstPinch || manager.enemyPlanets.Contains(hit) && firstPinch)
            {
                Renderer renderer = hit.gameObject.GetComponent<Renderer>();
                enemyPlanet = hit;
                enemyMat = renderer.material;
                renderer.materials = new Material[] { renderer.material, redMat };
                
                if (IsPinching(righthand) && !pinching)
                {
                    manager.Attack(focusedPlanet, enemyPlanet);
                    
                    if (enemyPlanet != null)
                        enemyPlanet.GetComponent<Renderer>().materials = new Material[] { enemyMat };
                    enemyPlanet = null;
                }
            }
        }
        else if (IsPinching(righthand) && !pinching)
        {
            Debug.Log("notpiching anymore");
            firstPinch = false;
            if (focusedPlanet != null)
                focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
            focusedPlanet = null;
            pinching = IsPinching(righthand);
            return;
        }
        else
        {
            if (enemyPlanet != null)
                enemyPlanet.GetComponent<Renderer>().materials = new Material[] { enemyMat };
            enemyPlanet = null;
        }

        if (firstPinch)
        {

        }


        pinching = IsPinching(righthand);
    }


    bool IsPinching(XRNode hand)
    {
        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        aggregator.TryGetJoint(TrackedHandJoint.IndexTip, hand, out HandJointPose pose);

        bool handIsValid = aggregator.TryGetPinchProgress(hand, out bool isReadyToPinch, out bool isPinching, out float pinchAmount);
        return isPinching;
    }

    bool GetPointerPos(out GameObject hitObj)
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

    void OnDrawGizmos()
    {
        if (firstPinch)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(focusedPlanet.transform.position, 0.3f);
            Gizmos.DrawSphere(Camera.main.transform.position + Camera.main.transform.forward * 100f, 0.3f);
            Gizmos.DrawLine(focusedPlanet.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 100f);
        }
    }
}
