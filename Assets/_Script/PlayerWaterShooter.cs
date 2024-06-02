using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerWaterShooter: MonoBehaviour
{
    public VisualEffect waterGunParticleSystem;

    public Rigidbody playerRb;
    public Transform visualTransform;
    public float force = 2;

    [Header("Fooling around with jet knockback")]
    public bool removeMoveSpeedWhenShooting = false;
    public PlayerController playerController;
    private float baseMovespeed;

    bool isShooting = false;

    List<OilPipeGameplay> oilPipeGameplays = new List<OilPipeGameplay>();

    private void Start()
    {
        baseMovespeed = playerController.moveSpeed;
        waterGunParticleSystem.Stop();
        InputManager.Instance.RegisterToFireEventStarted(OnFireStarted);
        InputManager.Instance.RegisterToFireEventCanceled(OnFireCanceled);
    }

    private void FixedUpdate()
    {
        if(isShooting)
        {
            Vector3 direction = -visualTransform.forward;
            playerRb.AddForce(direction * force, ForceMode.Force);
        }
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

        if(removeMoveSpeedWhenShooting)
            playerController.moveSpeed = 0;
        isShooting = true;
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

        isShooting = false;
        playerController.moveSpeed = baseMovespeed;
        oilPipeGameplays.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out OilPipeGameplay oil)) {
            oilPipeGameplays.Add(oil);
        }
    }
}
