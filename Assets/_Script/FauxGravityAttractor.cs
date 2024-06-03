using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{

    public static FauxGravityAttractor instance;

    private MeshCollider col;

    void Awake()
    {
        instance = this;
        col = GetComponent<MeshCollider>();
    }

    public float gravity = -10f;

    public void Attract(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        body.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
        //RotateBody(body);
    }

    public void PlaceOnSurface(Rigidbody body, float distanceToSurface)
    {
        body.MovePosition((body.position - transform.position).normalized * (transform.localScale.x * 0.5000004f /*sphere radius col.radius*/));

        RotateBody(body);

        body.transform.localPosition += new Vector3(0, distanceToSurface, 0);
    }

    void RotateBody(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, gravityUp) * body.rotation;
        body.MoveRotation(Quaternion.Slerp(body.rotation, targetRotation, 500f * Time.deltaTime));
    }

}
