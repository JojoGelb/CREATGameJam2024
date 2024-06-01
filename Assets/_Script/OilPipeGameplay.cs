using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OilPipeGameplay : MonoBehaviour
{
    public float WateringTimeToDestroy = 2f;
    private float wateredTime = 0f;

    private bool isBeingWatered = false;

    private void Awake()
    {
        OilPipeManager.Instance?.AddOilPipe(this);
    }

    private void OnDestroy()
    {
        OilPipeManager.Instance?.RemoveOilPipe(this);
    }

    private void Update()
    {
        if(isBeingWatered)
        {
            wateredTime += Time.deltaTime;
            if(wateredTime > WateringTimeToDestroy )
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isBeingWatered=true;
    }

    public void WaterGunDisabled()
    {
        isBeingWatered = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isBeingWatered=false;
    }

    /*private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "WaterGun")
        {
            isBeingWatered = true;
        }
    }*/
}
