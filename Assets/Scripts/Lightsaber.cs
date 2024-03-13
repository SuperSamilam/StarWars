using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MixedReality.Toolkit;
using UnityEngine.XR;
using MixedReality.Toolkit.Subsystems;

public class Lightsaber : MonoBehaviour
{
    public AudioClip ignite;
    public AudioClip humming;
    public XRNode hand;

    AudioSource audioSource;


    [SerializeField] GameObject saber;
    [SerializeField] Color lightSaberColor;
    bool activated = false;
    BoxCollider saberCollider;
    List<ParticleSystem> blades = new List<ParticleSystem>();
    List<float> lightSaberLenght = new List<float>();

    float time;
    [SerializeField] float ignitionTime;

    bool isPinching = false;


    void Start()
    {
        if (saber == null)
            return;

        audioSource = GetComponent<AudioSource>();

        saber = Instantiate(saber);
        saber.transform.parent = gameObject.transform;
        saber.transform.position = Vector3.zero;
        for (int i = 0; i < saber.transform.childCount; i++)
        {
            if (saber.transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                blades.Add(saber.transform.GetChild(i).GetComponent<ParticleSystem>());
                lightSaberLenght.Add(blades[blades.Count-1].startLifetime);
            }
        }
        saberCollider = saber.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (saber == null)
            return;

        saber.transform.position = saber.transform.parent.position;
        saber.transform.rotation = saber.transform.parent.rotation;

        if (Input.GetMouseButtonDown(2) || IsPinching(hand) && !isPinching)
        {
            activated = !activated;
            //Turn on ligthsaber
            if (activated)
            {
                audioSource.clip = ignite;
                audioSource.Play();
                audioSource.loop = false;
                for (int i = 0; i < blades.Count; i++)
                {
                    blades[i].gameObject.SetActive(true);
                    blades[i].startLifetime = lightSaberLenght[i];
                }
                StartCoroutine(Timer(0.7f));
            }
            else
            {
                audioSource.clip = ignite;
                audioSource.Play();
                audioSource.loop = false;
                time = 0;
            }
        }

        if (!activated && time <= ignitionTime)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / ignitionTime);
            for (int j = 0; j < blades.Count; j++)
            {
                blades[j].startLifetime = Mathf.Lerp(lightSaberLenght[j], 0, t);
            }

            if (t >= 0.95)
            {
                for (int j = 0; j < blades.Count; j++)
                {
                    blades[j].gameObject.SetActive(false);
                }

            }
        }
        isPinching = IsPinching(hand);

    }

    private IEnumerator Timer(float time)
    {
        while (true)
        {

            yield return new WaitForSeconds(time);
            audioSource.loop = true;
            audioSource.clip = humming;
            audioSource.Play();
            break;
        }

    }

    bool IsPinching(XRNode hand)
    {
        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        bool handIsValid = aggregator.TryGetPinchProgress(hand, out bool isReadyToPinch, out bool isPinching, out float pinchAmount);
        return isPinching;
    }

    void OnValidate()
    {
        for (int j = 0; j < blades.Count; j++)
        {
            blades[j].startColor = lightSaberColor;
        }

    }

}
