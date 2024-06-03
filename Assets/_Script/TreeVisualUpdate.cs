using _Script;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeVisualUpdate : MonoBehaviour
{
    public Material BaseMaterial;
    private Material localMaterial;

    public float Polluted;
    private bool polluted = false;

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
        localMaterial.SetFloat("_TimeStart", float.MinValue);
        localMaterial.SetFloat("_TimeStartScaling", float.MinValue);
    }

    public void TreeUpdate()
    {
        if (Time.time < 5f)
            return;

        Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 25);

        var color = PollutionManager.Instance.GetColorAtPosition(hit);

        if (polluted)
        {
            if (color.a < .1f)
            {
                polluted = false;
                UpdateTimes();
            }
        }
        else
        if (color.a > .1f)
        {
            polluted = true;
            UpdateTimes();
        }

        if (polluted)
            localMaterial.SetFloat("_Polluted", 1);
        else
            localMaterial.SetFloat("_Polluted", 0);

        
    }

    private void UpdateTimes()
    {
        localMaterial.SetFloat("_TimeStart", Time.time);

        float timeStartScaling = Time.time;
        float elapsedTime = Time.time - localMaterial.GetFloat("_TimeStartScaling");
        float remainingTime = localMaterial.GetFloat("_TimeScale") - elapsedTime;
        if (remainingTime > 0)
            timeStartScaling = Time.time - remainingTime;
        localMaterial.SetFloat("_TimeStartScaling", timeStartScaling);
    }
}
 