using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelKepper : MonoBehaviour
{
    public LevelSystem level;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
