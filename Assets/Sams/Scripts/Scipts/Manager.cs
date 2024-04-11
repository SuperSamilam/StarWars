using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using UnityEngine.Video;
public class Manager : MonoBehaviour
{
    static Transform rightHand;
    static Transform leftHand;

    bool checkingPinch = false;
    public bool pinchingRight = false;
    public bool pinchingLeft = false;
    public bool pinchingRightLastFrame = false;
    public bool pinchingLeftLastFrame = false;

    float time = 0;

    void Start()
    {
        rightHand = GameObject.FindGameObjectWithTag("RightHand").transform;
        leftHand = GameObject.FindGameObjectWithTag("LeftHand").transform;
        Invoke("ChangePinch", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkingPinch == true)
        {
            pinchingRightLastFrame = pinchingRight;
            pinchingLeftLastFrame = pinchingLeft;
            pinchingRight = IsPinching(XRNode.RightHand);
            pinchingLeft = IsPinching(XRNode.LeftHand);
        }
        ResetEverything();
    }

    void ChangePinch()
    {
        checkingPinch = true;
    }

    void ResetEverything()
    {
        if (pinchingLeft)
        {
            time += Time.deltaTime;
            if (time >= 3f)
            {
                SceneLoader.LoadScene(0);
            }
        }
    }

    public void Back()
    {
        SceneLoader.LoadScene(0);
    }

    public static bool IsPinching(XRNode hand)
    {
        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        aggregator.TryGetJoint(TrackedHandJoint.IndexTip, hand, out HandJointPose pose);

        aggregator.TryGetPinchProgress(hand, out bool isReadyToPinch, out bool isPinching, out float pinchAmount);
        return isPinching;
    }

    public static bool GetPointerPos(out GameObject hitObj, LayerMask mask, bool handRight)
    {
        Transform hand;
        if (handRight)
            hand = rightHand;
        else
            hand = leftHand;


        hitObj = null;

        Ray ray = new Ray(hand.position, hand.transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.3f, out hit, 1000000, mask))
        {
            hitObj = hit.transform.gameObject;
            return true;
        }

        return false;
    }
}
