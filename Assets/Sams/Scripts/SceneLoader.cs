using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Loads the given sceane
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(sceneBuildIndex: index);
    }
}
