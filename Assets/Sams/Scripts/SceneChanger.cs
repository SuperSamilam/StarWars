using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Changes Sceane
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(sceneBuildIndex: index);
    }
}
