using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointToOil : MonoBehaviour
{
    private Transform oilPipeToPointTo;
    private float distanceToCenter;

    public void Setup(Transform oilPipeToPointTo, float distanceToCenter)
    {
        this.oilPipeToPointTo = oilPipeToPointTo;
        this.distanceToCenter = distanceToCenter;
    }

    private void Update()
    {
        if(oilPipeToPointTo != null)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            Vector3 direction = oilPipeToPointTo.position - transform.parent.position;
            transform.forward = direction.normalized;
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -90);
            transform.position = transform.position + transform.forward * distanceToCenter;
        }

    }
}
