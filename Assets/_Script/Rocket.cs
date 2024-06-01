using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public GameObject OilPipePrefab;

    Rigidbody rb;
    Collider collider;

    public bool started = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = rb.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started) return;
        if(other.GetComponent<FauxGravityAttractor>() != null)
        {
            rb.isKinematic = true;
            collider.isTrigger = false;
            Instantiate(OilPipePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
