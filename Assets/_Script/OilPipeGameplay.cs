using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class OilPipeGameplay : MonoBehaviour
{
    public float WateringTimeToDestroy = 2f;
    private float wateredTime = 0f;

    private bool isBeingWatered = false;

    public VisualEffect Explosion;
    public VisualEffect Watered;

    bool removedFromOilManager = false;

    private void Awake()
    {
        OilPipeManager.Instance?.AddOilPipe(this);
        Watered.Stop();
    }

    private void Update()
    {
        if(isBeingWatered)
        {
            wateredTime += Time.deltaTime;
            if(wateredTime > WateringTimeToDestroy )
            {
                transform.Find("Visual").gameObject.SetActive(false);
                transform.Find("RocketLandingEffect").GetComponent<VisualEffect>().SetBool("SubExplosion", true);
                Explosion.enabled = true;
                GetComponent<BoxCollider>().enabled = false;
                Watered.Stop();
                DestroyAfterTime(20f);
                if (removedFromOilManager)
                    return;
                removedFromOilManager=true;
                OilPipeManager.Instance?.RemoveOilPipe(this);
            }
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        isBeingWatered=true;
        Watered.Play();
    }

    public void WaterGunDisabled()
    {
        isBeingWatered = false;
        Watered.Stop();
    }

    private void OnTriggerExit(Collider other)
    {
        isBeingWatered=false;
        Watered.Stop();
    }

    /*private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "WaterGun")
        {
            isBeingWatered = true;
        }
    }*/
}
