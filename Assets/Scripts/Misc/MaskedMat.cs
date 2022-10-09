using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedMat : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }
}
