using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaberController : MonoBehaviour
{
    [SerializeField] AudioSource lightSaberAudio;
    [SerializeField] AudioClip lightSaberStart;
    [SerializeField] AudioClip lightSaberHumming;



    bool rightActivated = false;
    [SerializeField] GameObject rightSaber;
    BoxCollider rightBoxCollider;
    List<GameObject> rightParticels = new List<GameObject>();
    // List<float> rightSpeed = new List<float>();
    List<float> rightDistance = new List<float>();
    List<Color> rightColor;

    bool leftActivated = false;
    [SerializeField] GameObject leftSaber;
    BoxCollider leftBoxCollider;
    List<GameObject> leftParticels = new List<GameObject>();
    // List<float> leftSpeed = new List<float>();
    List<float> leftDistance = new List<float>();
    List<Color> leftColor;

    void Start()
    {
        rightBoxCollider = rightSaber.GetComponent<BoxCollider>();
        for (int i = 0; i < rightSaber.transform.childCount; i++)
        {
            if (rightSaber.transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                rightParticels.Add(rightSaber.transform.GetChild(i).gameObject);
                // rightSpeed.Add(rightSaber.transform.GetChild(i).GetComponent<ParticleSystem>().startSpeed);
                rightDistance.Add(rightSaber.transform.GetChild(i).GetComponent<ParticleSystem>().startLifetime);
                // rightColor.Add(rightSaber.transform.GetChild(i).GetComponent<ParticleSystem>().startColor);
            }
        }


        leftBoxCollider = leftSaber.GetComponent<BoxCollider>();
        for (int i = 0; i < leftSaber.transform.childCount; i++)
        {
            if (leftSaber.transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                leftParticels.Add(leftSaber.transform.GetChild(i).gameObject);
                // leftSpeed.Add(leftSaber.transform.GetChild(i).GetComponent<ParticleSystem>().startSpeed);
                leftDistance.Add(leftSaber.transform.GetChild(i).GetComponent<ParticleSystem>().startLifetime);
                // leftColor.Add(leftSaber.transform.GetChild(i).GetComponent<ParticleSystem>().startColor);
            }
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightActivated = !rightActivated;
            if (rightActivated)
            {
                for (int i = 0; i < rightParticels.Count; i++)
                {
                    lightSaberAudio.clip = lightSaberStart;
                    lightSaberAudio.Play();
                    rightParticels[i].SetActive(true);
                    rightParticels[i].GetComponent<ParticleSystem>().startLifetime = rightDistance[i];
                    StartCoroutine(Timer(0.7f));
                }
            }
            else
            {
                lightSaberAudio.clip = lightSaberStart;
                lightSaberAudio.Play();
                StartCoroutine(TurnOffSaber(rightParticels, rightDistance));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            leftActivated = !leftActivated;
            if (leftActivated)
            {
                for (int i = 0; i < leftParticels.Count; i++)
                {
                    lightSaberAudio.clip = lightSaberStart;
                    lightSaberAudio.Play();
                    leftParticels[i].SetActive(true);
                    leftParticels[i].GetComponent<ParticleSystem>().startLifetime = leftDistance[i];
                    StartCoroutine(Timer(0.7f));
                }
            }
            else
            {
                lightSaberAudio.clip = lightSaberStart;
                lightSaberAudio.Play();
                StartCoroutine(TurnOffSaber(leftParticels, leftDistance));
            }
        }




    }

    private IEnumerator TurnOffSaber(List<GameObject> particels, List<float> distances)
    {
        int itteration = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            bool done = false;
            for (int i = 0; i < particels.Count; i++)
            {
                particels[i].GetComponent<ParticleSystem>().startLifetime -= distances[i]/5;
                if (particels[i].GetComponent<ParticleSystem>().startLifetime <= 0.01)
                {
                    done = true;
                }
            }

            itteration++;
            if (itteration == 10 || done)
            {
                for (int i = 0; i < particels.Count; i++)
                {
                    particels[i].SetActive(false);
                }
                break;
            }
        }
    }

    private IEnumerator Timer(float time)
    {
        while (true)
        {

            yield return new WaitForSeconds(time);
            lightSaberAudio.clip = lightSaberHumming;
            lightSaberAudio.Play();
            break;
        }

    }

}
