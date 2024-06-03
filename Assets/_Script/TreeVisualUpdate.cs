using _Script;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeVisualUpdate : MonoBehaviour
{
    public Material BaseMaterial;
    private Material localMaterial;

    public float Polluted;

    //private void Awake()
    //{
    //    StartTree();
    //}

    private void Update()
    {
        //TreeUpdate();
    }

    public void StartTree()
    {
        localMaterial = new Material(BaseMaterial);
        GetComponent<MeshRenderer>().material = localMaterial;
    }

    public void TreeUpdate()
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 25);

        var color = PollutionManager.Instance.GetColorAtPosition(hit);

        if (color.a > .1f)
            localMaterial.SetFloat("_Polluted", 1);
        else
            localMaterial.SetFloat("_Polluted", 0);
    }
}
