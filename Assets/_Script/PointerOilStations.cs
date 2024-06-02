using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerOilStations : MonoBehaviour
{

    public GameObject arrowPrefab;
    private ArrowPointToOil arrow;

    public float refreshRate = 0.2f;

    public float distanceToCenter = 4.8f;

    List<OilPipeGameplay> oilsInGame;

    WaitForSecondsRealtime w;

    // Start is called before the first frame update
    void Start()
    {
        OilPipeManager.Instance.OnOilPipeCountChanged.AddListener(ResetPointers);
        w = new WaitForSecondsRealtime(refreshRate);
        StartCoroutine(UpdateArrow());
    }

    IEnumerator UpdateArrow()
    {
        while (true)
        {
            if (arrow != null && arrow.gameObject.activeSelf)
                arrow.Setup(oilsInGame[GetIndexClosestOilPipe()].transform, distanceToCenter);
            yield return w;
        }
    }

    public int GetIndexClosestOilPipe()
    {
        int closestOilIndex = 0;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < oilsInGame.Count; i++)
        {
            float distance = Vector3.Distance(oilsInGame[i].transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestOilIndex = i;
                closestDistance = distance;
            }
        }

        return closestOilIndex;
    }

    private void ResetPointers(int count)
    {
        oilsInGame = OilPipeManager.Instance.GetOliPipesInGame();

        if (count == 0)
        {
            arrow.gameObject.SetActive(false);
        }
        else
        {
            if (arrow == null || !arrow.gameObject.activeSelf)
            {
                GameObject g = ObjectPooler.Instance.GetOrCreateGameObjectFromPool(ObjectPooler.PoolObject.Arrow);
                arrow = g.GetComponent<ArrowPointToOil>();
                g.transform.parent = transform;
            }
            arrow.Setup(oilsInGame[GetIndexClosestOilPipe()].transform, distanceToCenter);
        }
    }
}
