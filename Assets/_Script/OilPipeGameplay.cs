using System.Collections;
using System.Collections.Generic;
using _Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class OilPipeGameplay : MonoBehaviour
{
    public float WateringTimeToDestroy = 2f;
    private float wateredTime = 0f;

    private bool isBeingWatered = false;

    public VisualEffect Explosion;
    public VisualEffect Watered;

    bool removedFromOilManager = false;

    private float timeBetweenBurstOfPoiSon = 2f;
    public Vector3 maxForceBubble;
    public Vector3 minForceBubble;

    public Transform BubbleSpawnPosition;

    public AudioClip explosionClip;
    public AudioClip landingClip;
    private AudioSource audiosource;

    private void Awake()
    {
        OilPipeManager.Instance?.AddOilPipe(this);
        Watered.Stop();
    }

    private void Start()
    {
        Invoke(nameof(SpawnBubble), timeBetweenBurstOfPoiSon);
        audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(landingClip);
    }

    void SpawnBubble()
    {
        if (removedFromOilManager) return;
        GameObject g = ObjectPooler.Instance.GetOrCreateGameObjectFromPool(ObjectPooler.PoolObject.PoisonBubble);
        g.transform.parent = transform;
        g.transform.localRotation = Quaternion.identity;
        g.transform.position = BubbleSpawnPosition.position;
        Vector3 force = new Vector3(Random.Range(minForceBubble.x, maxForceBubble.x), Random.Range(minForceBubble.y, maxForceBubble.y), Random.Range(minForceBubble.z, maxForceBubble.z));
        Rigidbody rbBubble = g.GetComponent<Rigidbody>();
        rbBubble.velocity = Vector3.zero;
        rbBubble.AddForce(force, ForceMode.Impulse);
        Invoke(nameof(SpawnBubble), timeBetweenBurstOfPoiSon);
    }

    private void Update()
    {
        timeBetweenBurstOfPoiSon = DifficultyManager.Instance.GetTimeBetweenBurstOfPoiSon();
        if(isBeingWatered)
        {
            wateredTime += Time.deltaTime;
            if(wateredTime > WateringTimeToDestroy )
            {
                transform.Find("Visual").gameObject.SetActive(false);
                transform.Find("RocketLandingEffect").GetComponent<VisualEffect>().SetBool("SubExplosion", true);
                Explosion.enabled = true;
                GetComponent<BoxCollider>().enabled = false;
                Watered.Stop();
                DestroyAfterTime(20f);
                if (removedFromOilManager)
                    return;
                removedFromOilManager=true;
                audiosource.PlayOneShot(explosionClip);
                OilPipeManager.Instance?.RemoveOilPipe(this);
            }
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "WaterGun") return;
        isBeingWatered=true;
        Watered.Play();
    }

    public void WaterGunDisabled()
    {
        isBeingWatered = false;
        Watered.Stop();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "WaterGun") return;
        isBeingWatered =false;
        Watered.Stop();
    }

    /*private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "WaterGun")
        {
            isBeingWatered = true;
        }
    }*/
}
