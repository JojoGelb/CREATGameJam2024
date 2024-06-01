using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public GameObject OilPipePrefab;

    Rigidbody rb;
    Collider colliderComponent;

    public bool started = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliderComponent = rb.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started) return;
        if(other.GetComponent<FauxGravityAttractor>() != null)
        {
            rb.isKinematic = true;
            colliderComponent.isTrigger = false;
            Instantiate(OilPipePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
