using System.Collections.Generic;
using UnityEngine;

//Author : Jordy Gelb
public class ObjectPooler : Singleton<ObjectPooler>
{
    public enum PoolObject
    {
        Sound = 0,
        Arrow = 1,
        PoisonBubble = 2
    }

    [System.Serializable]
    public class PoolPrefabSetUp
    {
        public PoolObject tag;
        public GameObject prefab;
    }

    //Quick solution to non Serializable Dictionary 
    [SerializeField] private List<PoolPrefabSetUp> poolPrefabSetUps  = new List<PoolPrefabSetUp>();
    
    private Dictionary<PoolObject, GameObject> poolPrefab            = new Dictionary<PoolObject, GameObject>();
    private Dictionary<PoolObject, List<GameObject>> poolDictionnary = new Dictionary<PoolObject, List<GameObject>>();

    protected override void Awake()
    {
        base.Awake();

        foreach (PoolPrefabSetUp poolPrefabSetUpObject in poolPrefabSetUps)
        {
            poolPrefab.Add(poolPrefabSetUpObject.tag, poolPrefabSetUpObject.prefab);
            poolDictionnary.Add(poolPrefabSetUpObject.tag, new List<GameObject>());
        }
    }

    public GameObject GetOrCreateGameObjectFromPool(PoolObject tag)
    {
        GameObject result = null;
        List<GameObject> gameObjectList = poolDictionnary[tag];

        if(gameObjectList == null)
        {
            Debug.LogError("PoolObject wasn't setted up in the ObjectPooler. Please add an entry to the poolprefab");
            return null;
        }

        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (gameObjectList[i].activeSelf == false)
            {
                result = gameObjectList[i];
                gameObjectList.RemoveAt(i);
                gameObjectList.Add(result);
            }
        }

        if(result == null)
        {
            result = Instantiate(poolPrefab[tag]);
            result.transform.parent = transform;
            gameObjectList.Add(result);
        }

        result.SetActive(true);
        return result;
    }
}
