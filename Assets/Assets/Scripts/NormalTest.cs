using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTest : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.up * 100f);
    }
}
