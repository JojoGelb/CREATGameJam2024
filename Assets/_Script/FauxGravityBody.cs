using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FauxGravityBody : MonoBehaviour
{

    private FauxGravityAttractor attractor;
    private Rigidbody rb;

    public float distanceToSurface = 0.2f;

    public bool placeOnSurface = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        attractor = FauxGravityAttractor.instance;

        if (placeOnSurface)
            attractor.PlaceOnSurface(rb,distanceToSurface);
    }

    void FixedUpdate()
    {
        attractor.Attract(rb);
    }

}