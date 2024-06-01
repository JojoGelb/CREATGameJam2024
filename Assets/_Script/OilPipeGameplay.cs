using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OilPipeGameplay : MonoBehaviour
{
    public float WateringTimeToDestroy = 2f;
    private float wateredTime = 0f;

    private bool isBeingWatered = false;

    private void Update()
    {
        if(isBeingWatered)
        {
            wateredTime += Time.deltaTime;
            if(wateredTime > WateringTimeToDestroy )
            {
                Destroy(gameObject);
            }
            isBeingWatered = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "WaterGun")
        {
            isBeingWatered = true;
        }
    }


    /*private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enter");
        if(collision.transform.tag == "WaterGun")
        {
            Vector3 direction = transform.position 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, distance))
            {
                // Check if the hit object is not the target object itself
                if (hit.transform != targetObject)
                {
                    Debug.Log("Obstacle detected between objects: " + hit.transform.name);
                    // Handle the obstacle detection here
                }
            }
            isBeingWatered =true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Exit");
        if (collision.transform.tag == "WaterGun")
        {
            isBeingWatered = false;
        }
    }*/
}
