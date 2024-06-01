using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    Collider collider;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = rb.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FauxGravityAttractor>() != null)
        {
            rb.isKinematic = true;
            collider.isTrigger = false;
        }
    }
}
