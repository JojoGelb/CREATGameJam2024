using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class PlayerWaterShooter : MonoBehaviour
{
    public VisualEffect waterGunParticleSystem;

    public Rigidbody playerRb;
    public Transform visualTransform;
    public float force = 2;
    public float WashingRadius;
    public float DistanceFromPlayer;
    public float MaxWashingDistance;
    public float EraseFeather;

    [Header("Fooling around with jet knockback")]
    public bool removeMoveSpeedWhenShooting = false;

    public PlayerController playerController;
    private float baseMovespeed;

    bool isShooting = false;

    List<OilPipeGameplay> oilPipeGameplays = new List<OilPipeGameplay>();

    public Transform AimDirection;
    public GameObject Planet;

    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        baseMovespeed = playerController.moveSpeed;
        waterGunParticleSystem.Stop();
        InputManager.Instance.RegisterToFireEventStarted(OnFireStarted);
        InputManager.Instance.RegisterToFireEventCanceled(OnFireCanceled);
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            Vector3 direction = -visualTransform.forward;
            playerRb.AddForce(direction * force, ForceMode.Force);


            // Use raycasting to measure distance between nozzle and impact
            if (Physics.Raycast(AimDirection.position, AimDirection.up, out RaycastHit hit, 25) && hit.collider.gameObject != Planet && hit.collider.transform != transform)
            {
                waterGunParticleSystem.SetFloat("Obstacle distance", hit.distance);
            }
            else
            {
                waterGunParticleSystem.SetFloat("Obstacle distance", -100f);
            }
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

        if (removeMoveSpeedWhenShooting)
            playerController.moveSpeed = 0;
        isShooting = true;
        audio.Play();
    }

    private void OnFireCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Stop water
        waterGunParticleSystem.Stop();
        waterGunParticleSystem.GetComponent<Collider>().enabled = false;
        foreach (OilPipeGameplay oil in oilPipeGameplays)
        {
            oil.WaterGunDisabled();
        }

        isShooting = false;
        playerController.moveSpeed = baseMovespeed;
        oilPipeGameplays.Clear();
        audio.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out OilPipeGameplay oil))
        {
            oilPipeGameplays.Add(oil);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent(out Paintable paintableObject))
        {
            var collisionPoint = collider.ClosestPoint(visualTransform.position);
            for (int i = 1; i < 10; i++)
            {
                PaintManager.Instance.Erase(paintableObject,
                    collisionPoint + visualTransform.forward * (DistanceFromPlayer + (i * (MaxWashingDistance / 10))),
                    WashingRadius,
                    EraseFeather);
            }
        }
    }
}