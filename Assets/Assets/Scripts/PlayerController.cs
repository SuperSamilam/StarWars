using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using UnityEngine.XR;
using MixedReality.Toolkit.Subsystems;

public class PlayerController : MonoBehaviour
{
    public XRNode hand;
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
        if (focusedPlanet != null)
            focusedPlanet.gameObject.GetComponent<Renderer>().material = focusedMaterial;
        if (GetPointerPos(out GameObject hit))
        {
            // if (focusedPlanet != null)
            //     if (focusedPlanet != hit)
            //     {
            //         focusedPlanet.gameObject.GetComponent<Renderer>().material = focusedMaterial;
            //     }


            Renderer renderer = hit.gameObject.GetComponent<Renderer>();
            focusedPlanet = hit;
            focusedMaterial = renderer.material;
            renderer.materials = new Material[] { renderer.material, greenMat };

        }
        else
        {
            if (focusedPlanet != null)
                focusedPlanet.gameObject.GetComponent<Renderer>().material = focusedMaterial;

        }

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
        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        bool sucsess = aggregator.TryGetJoint(TrackedHandJoint.IndexTip, hand, out HandJointPose pose);
        if (sucsess == false)
            return false;
        Ray ray = new Ray(pose.Position, (-pose.Up + pose.Forward) / 2 * 10000f);


        Debug.DrawRay(pose.Position, (-pose.Up + pose.Forward) / 2 * 10000f, Color.red, Time.deltaTime);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000000, mask))
        {
            hitObj = hit.transform.gameObject;
            return true;
        }
        return false;
    }
}
