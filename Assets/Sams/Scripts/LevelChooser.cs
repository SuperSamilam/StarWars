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

    //Varibels that will be used inbetween 
    GameObject focusedPlanet;
    Material focusedMaterial;
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

            if (Gamemanager.IsPinching(hand) && !pinching)
            {
                if (hit.GetComponent<LevelHolder>().level != null)
                    Gamemanager.level = hit.GetComponent<LevelHolder>().level;
                SceneManager.LoadScene(sceneBuildIndex: hit.GetComponent<LevelHolder>().index);
            }
        }
        else
        {
            if (focusedPlanet != null)
                focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
            focusedPlanet = null;
            levelName.gameObject.SetActive(false);
        }
        pinching = Gamemanager.IsPinching(hand);
    }
}
