using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Lightsaber : MonoBehaviour
{
    public static AudioClip ignite;
    public static AudioClip humming;

    AudioSource audioSource;
    [SerializeField] int activeButton;

    bool activated = false;
    [SerializeField] GameObject saber;
    BoxCollider saberCollider;
    [SerializeField] Color lightSaberColor;
    List<ParticleSystem> blades = new List<ParticleSystem>();
    List<float> lightSaberLenght = new List<float>();
    List<float> lightSaberSpeed = new List<float>();


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
                Debug.Log("Found Blades");
                blades.Add(saber.transform.GetChild(i).GetComponent<ParticleSystem>());
                lightSaberLenght.Add(blades[i].startLifetime);
                lightSaberSpeed.Add(blades[i].startSpeed);
                blades[i].startColor = lightSaberColor;
            }
        }
        saberCollider = saber.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (saber == null)
            return;
        if (Input.GetMouseButtonDown(activeButton))
        {
            Debug.Log("HERE");
            activated = !activated;
            //Turn on ligthsaber
            if (activated)
            {
                for (int i = 0; i < blades.Count; i++)
                {
                    blades[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < blades.Count; i++)
                {
                    StartCoroutine(DeActivate());
                }
            }
        }
    }

    IEnumerator DeActivate()
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < blades.Count; j++)
                {
                    blades[j].startLifetime -= Mathf.Lerp(0, lightSaberLenght[j], i / 5);
                }
                yield return new WaitForSeconds(0.3f);
            }
            for (int j = 0; j < blades.Count; j++)
            {
                blades[j].gameObject.SetActive(false);
                blades[j].startLifetime = lightSaberLenght[j];
            }
            break;
        }
    }
}
