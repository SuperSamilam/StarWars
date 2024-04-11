using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour
{
    [SerializeField] Material heighLightMat;
    [SerializeField] XRNode hand;
    [SerializeField] LayerMask mask;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] LevelKepper levelKeeper;
    [SerializeField] AudioSource source;

    //Varibels that will be used inbetween 
    GameObject focusedPlanet;
    Material focusedMaterial;
    bool audioPlayed = false;
    bool pinching;



    void Update()
    {
        //Get the planet looked at and if clicked load it
        if (Gamemanager.GetPointerPos(out GameObject hit, mask))
        {
            Renderer renderer = hit.gameObject.GetComponent<Renderer>();
            focusedPlanet = hit;
            focusedMaterial = renderer.material;
            renderer.materials = new Material[] { renderer.material, heighLightMat };

            levelName.text = hit.gameObject.name;

            levelName.gameObject.SetActive(true);

            if (!audioPlayed && hit.GetComponent<LevelHolder>().clip != null)
            {
                source.clip = hit.GetComponent<LevelHolder>().clip;
                source.Play();
                audioPlayed = true;
            }

            if (Gamemanager.IsPinching(hand) && !pinching)
            {
                if (hit.GetComponent<LevelHolder>().level != null)
                    levelKeeper.level = hit.GetComponent<LevelHolder>().level;
                SceneManager.LoadScene(sceneBuildIndex: hit.GetComponent<LevelHolder>().index);
            }
        }
        else
        {
            if (focusedPlanet != null)
                focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
            focusedPlanet = null;
            levelName.gameObject.SetActive(false);
            audioPlayed = false;
        }
        pinching = Gamemanager.IsPinching(hand);
    }
}
