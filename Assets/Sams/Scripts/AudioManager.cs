using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips;

    void Start()
    {
        source.volume = PlayerPrefs.GetFloat("Audio");
        StartCoroutine("PlayMusic");
    }

    //Creates a loop that will play music endlessly
    IEnumerator PlayMusic()
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.clip = clip;
        source.Play();
        yield return new WaitForSeconds(clip.length);
        StartCoroutine("PlayMusic");
    }
}
