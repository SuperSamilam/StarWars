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
    bool pinching;
    public LayerMask mask;

    public Material greenMat;
    public Material redMat;

    GameObject focusedPlanet;
    Material focusedMaterial;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GetPointerPos(out GameObject hit))
        {
            if (IsPinching(righthand) && !pinching)
            {
                if (focusedPlanet != null && focusedPlanet != hit)
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };

                if (focusedPlanet != null && focusedPlanet == hit)
                {
                    print(focusedMaterial.name);
                    focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
                    focusedPlanet = null;
                    pinching = IsPinching(righthand);
                    return;
                }

                Renderer renderer = hit.gameObject.GetComponent<Renderer>();
                focusedPlanet = hit;
                focusedMaterial = renderer.material;
                renderer.materials = new Material[] { renderer.material, greenMat };
            }
        }

        // if (focusedPlanet != null && focusedPlanet != hit)
        //     focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };


        // Renderer renderer = hit.gameObject.GetComponent<Renderer>();
        // focusedPlanet = hit.gameObject;
        // focusedMaterial = renderer.material;
        // renderer.materials = new Material[] { renderer.material, greenMat };
        // else
        // {
        //     if (focusedPlanet != null)
        //     {
        //         focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
        //     }
        // }

        pinching = IsPinching(righthand);


    }

    // public void OnSourceDetected(SourceStateEventData eventData)
    // {
    //     var handVisualizer = eventData.Controller.Visualizer as IMixedRealityHandVisualizer;
    //     if (handVisualizer != null)
    //     {
    //         if (handVisualizer.TryGetJointTransform(TrackedHandJoint.IndexTip, out Transform jointTransform)
    //         {
    //             // ...
    //         }
    //     }
    // }

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
}
