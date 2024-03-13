using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaberController : MonoBehaviour
{
    [SerializeField] float ignitionTime = 3f;
    [SerializeField] AudioClip lightSaberStart;
    [SerializeField] AudioClip lightSaberHumming;

    public float time = 0;


    bool rightActivated = false;
    [SerializeField] AudioSource rightAudioSource;
    [SerializeField] GameObject rightSaber;
    List<GameObject> rightParticels = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < rightSaber.transform.childCount; i++)
        {
            if (rightSaber.transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                rightParticels.Add(rightSaber.transform.GetChild(i).gameObject);
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
                    rightParticels[i].SetActive(true);
                    //rightAudioSource.clip = lightSaberStart;
                    //rightAudioSource.Play();
                    rightParticels[i].GetComponent<ParticleSystem>().startLifetime = 0.34f;
                    StartCoroutine(Timer(0.7f));
                }
            }
            else
            {
                //rightAudioSource.clip = lightSaberStart;
                //rightAudioSource.Play();
                StartCoroutine(TurnOffSaber());
            }
        }


    }

    private IEnumerator TurnOffSaber()
    {
        int itteration = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            bool done = false;
            for (int i = 0; i < rightParticels.Count; i++)
            {
                rightParticels[i].GetComponent<ParticleSystem>().startLifetime -= 0.05f;
                if (rightParticels[i].GetComponent<ParticleSystem>().startLifetime <= 0.01)
                {
                    done = true;
                }
            }

            itteration++;
            if (itteration == 10 || done)
            {
                for (int i = 0; i < rightParticels.Count; i++)
                {
                    rightParticels[i].SetActive(false);
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
            //rightAudioSource.clip = lightSaberHumming;
            //rightAudioSource.Play();
            break;
        }

    }

}
