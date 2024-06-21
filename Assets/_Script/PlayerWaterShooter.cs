using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[Serializable]
public struct WaterJetParameters
{
    public string name;
    public float WashingRadius;
    public float MaxWashingDistance;
    public float MinRadius;
    public int CircleSpawnIterationNumber;
    public float TimeToDestroyPipe;
}

public class PlayerWaterShooter : MonoBehaviour
{
    [SerializeField]
    private VisualEffect waterGunParticleSystem;

    [SerializeField]
    private Rigidbody playerRb;
    [SerializeField]
    private Transform visualTransform;
    [SerializeField]
    private float KnockBackForce = 2;

    [SerializeField]
    private float DistanceFromPlayer;
    [SerializeField]
    private float EraseFeather;

    [SerializeField]
    private int currentWaterJetParameterIndex = 0;

    [SerializeField]
    private List<WaterJetParameters> WaterJetParameters = new List<WaterJetParameters>();

    [Header("Fooling around with jet knockback")]

    [SerializeField]
    private PlayerController playerController;
    private float baseMovespeed;

    bool isShooting = false;

    List<OilPipeGameplay> oilPipeGameplays = new List<OilPipeGameplay>();

    [SerializeField]
    private Transform AimDirection;
    [SerializeField]
    private GameObject Planet;

    private AudioSource soundEffect;

    public float TimeToDestroyPipe = 2f;

    private void Start()
    {
        soundEffect = GetComponent<AudioSource>();
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
            playerRb.AddForce(direction * KnockBackForce, ForceMode.Force);


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

        isShooting = true;
        soundEffect.Play();
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
        soundEffect.Stop();
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
            
            float minRadius = WaterJetParameters[currentWaterJetParameterIndex].MinRadius;
            float currentRadius;
            int n = WaterJetParameters[currentWaterJetParameterIndex].CircleSpawnIterationNumber;
            float WashingRadius = WaterJetParameters[currentWaterJetParameterIndex].WashingRadius;
            float MaxWashingDistance = WaterJetParameters[currentWaterJetParameterIndex].MaxWashingDistance;

            Debug.Log("HERE " + minRadius + " " + WashingRadius + " " + MaxWashingDistance);

            for (int i =0 ; i < n; i++)
            {

                currentRadius = Mathf.Lerp(minRadius, WashingRadius, i / (float)n);

                PaintManager.Instance.Erase(paintableObject,
                    collisionPoint + visualTransform.forward * (DistanceFromPlayer + (i * (MaxWashingDistance / n))),
                    currentRadius,
                    EraseFeather);

            }
        }
    }

    public void ChangeCurrentWaterJetParameterIndex(int change)
    {
        int index = currentWaterJetParameterIndex;
        index += change;
        if (index < 0) index = 0;
        else if (index >= WaterJetParameters.Count) index = WaterJetParameters.Count - 1;

        currentWaterJetParameterIndex = index;
        TimeToDestroyPipe = WaterJetParameters[currentWaterJetParameterIndex].TimeToDestroyPipe;
    }
}