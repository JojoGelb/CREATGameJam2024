using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpJet : MonoBehaviour
{

    [SerializeField]
    private int jetChange = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerController c))
        {
            c.waterShooter.ChangeCurrentWaterJetParameterIndex(jetChange);
            gameObject.SetActive(false);
        }
    }
}
