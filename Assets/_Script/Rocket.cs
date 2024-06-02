using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public GameObject OilPipePrefab;
    public Color paintColor;

    Rigidbody rb;
    Collider colliderComponent;

    public float PoisonRadiusOnSpawn= 5;

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
            Vector3 point = other.ClosestPointOnBounds(transform.position);

            Paintable p = other.GetComponent<Paintable>();
            if (p != null)
            {
                PaintManager.Instance.Paint(p, point, PoisonRadiusOnSpawn, 1, 1, paintColor);
            }

            Destroy(gameObject);
        }
    }
}
