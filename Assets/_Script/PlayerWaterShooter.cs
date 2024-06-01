﻿using System;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerWaterShooter: MonoBehaviour
{
    public VisualEffect waterGunParticleSystem;

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
    }

    private void OnFireCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Stop water
        waterGunParticleSystem.Stop();
    }


}
