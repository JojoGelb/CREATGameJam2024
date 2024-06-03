using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeVisualUpdate : MonoBehaviour
{
    public Material BaseMaterial;
    private Material localMaterial;

    public float Polluted;

    private int remainingBeforeUpdate = 15;

    void Start()
    {
        localMaterial = new Material(BaseMaterial);
        GetComponent<MeshRenderer>().material = localMaterial;
        remainingBeforeUpdate = Random.Range(15, 30);
    }

    void Update()
    {
        if (remainingBeforeUpdate == 0)
        {
            localMaterial.SetFloat("_Polluted", Polluted);
            remainingBeforeUpdate = Random.Range(30, 60);
        }
        else
            remainingBeforeUpdate--;


    }
}
