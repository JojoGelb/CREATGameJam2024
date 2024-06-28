using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketManager : Singleton<RocketManager>
{

    public GameObject RocketPrefab;

    public float spawnRadius = 20f;

    //public AnimationCurve SpawnSpeedCurveMultiplier;


    [HideInInspector] public float SpawnInterval;
    public float NumberofRocketSpawned = 1;
    private float counter = 0;
    
    private float EvaluateCurve(AnimationCurve curve, float position)
    {
        return curve.Evaluate(position);
    }

    private void Start()
    {
        SpawnRocket();
    }

    private void SpawnRocket()
    {   
        while(counter < NumberofRocketSpawned) {
            GameObject g = Instantiate(RocketPrefab,Vector3.zero, Random.rotation);
            g.transform.position = g.transform.up * spawnRadius;
            g.GetComponent<Rocket>().started = true;
            counter++;
        }

        counter = 0;

        Invoke(nameof(SpawnRocket),SpawnInterval + SpawnInterval);
        
    }

}
