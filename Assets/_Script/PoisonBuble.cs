using _Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBuble : MonoBehaviour
{
    public float PoisonRadiusOnSpawn;
    public Color PaintColor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Paintable p)) {
            Vector3 point = other.ClosestPointOnBounds(transform.position);

            PaintManager.Instance.Paint(p, point, PoisonRadiusOnSpawn, 1, 1, PaintColor);

            Invoke(nameof(DestroyObject),1);
        }
    }

    void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}
