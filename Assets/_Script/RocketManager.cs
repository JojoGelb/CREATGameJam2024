using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{

    public GameObject RocketPrefab;

    public float spawnRadius = 20f;

    //public AnimationCurve SpawnSpeedCurveMultiplier;

    public float InitialRocketSpawnInterval = 5;


    
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
        GameObject g = Instantiate(RocketPrefab,Vector3.zero, Random.rotation);
        g.transform.position = g.transform.up * spawnRadius;
        
        g.GetComponent<Rocket>().started = true;

        Invoke(nameof(SpawnRocket),InitialRocketSpawnInterval + InitialRocketSpawnInterval);
        
    }

}
