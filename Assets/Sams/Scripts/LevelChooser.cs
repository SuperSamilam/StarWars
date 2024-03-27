using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour
{

    public Material greenMat;
    public XRNode rightHand;
    public LayerMask mask;
    public TextMeshProUGUI levelName;
    bool pinching;

    GameObject focusedPlanet;
    Material focusedMaterial;

    void Start()
    {

    }

    void Update()
    {
        if (Gamemanager.GetPointerPos(out GameObject hit, mask))
        {
            Renderer renderer = hit.gameObject.GetComponent<Renderer>();
            focusedPlanet = hit;
            focusedMaterial = renderer.material;
            renderer.materials = new Material[] { renderer.material, greenMat };
            levelName.text = hit.gameObject.name;
            levelName.gameObject.SetActive(true);


            if (Gamemanager.IsPinching(rightHand) && !pinching)
            {
                Gamemanager.level = hit.GetComponent<LevelHolder>().level;
                SceneManager.LoadScene (sceneBuildIndex:3);
            }
        }
        else
        {
            if (focusedPlanet != null)
                focusedPlanet.GetComponent<Renderer>().materials = new Material[] { focusedMaterial };
            focusedPlanet = null;
            levelName.gameObject.SetActive(false);
        }

    }




}
