using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerWaterShooter: MonoBehaviour
{
    public VisualEffect waterGunParticleSystem;

    List<OilPipeGameplay> oilPipeGameplays = new List<OilPipeGameplay>();

    private void Start()
    {
        waterGunParticleSystem.Stop();
        InputManager.Instance.RegisterToFireEventStarted(OnFireStarted);
        InputManager.Instance.RegisterToFireEventCanceled(OnFireCanceled);
    }

    private void OnDisable()
    {
        InputManager.Instance?.UnRegisterToFireEventStarted(OnFireStarted);
        InputManager.Instance?.UnRegisterToFireEventCanceled(OnFireCanceled);
    }
    private void OnFireStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Launch water
        waterGunParticleSystem.Play();
        waterGunParticleSystem.GetComponent<Collider>().enabled = true;
    }

    private void OnFireCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Stop water
        waterGunParticleSystem.Stop();
        waterGunParticleSystem.GetComponent<Collider>().enabled = false;
        foreach(OilPipeGameplay oil in oilPipeGameplays)
        {
            oil.WaterGunDisabled();
        }

        oilPipeGameplays.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out OilPipeGameplay oil)) {
            oilPipeGameplays.Add(oil);
        }
    }
}
