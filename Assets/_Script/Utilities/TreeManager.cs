using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeManager : Singleton<TreeManager>
{

    public float PlanetRadius;
    public float TreeSize;

    public uint TreeNumber;

    public List<TreePool> TreePools;
    private List<int> RandomMatch = new();

    public int UpdatePoolNumber = 10;
    private List<HashSet<TreeVisualUpdate>> updatePools = new();
    private int poolTurnNumber = 0;

    void Start()
    {
        for (int n = 0; n < UpdatePoolNumber; n++)
        {
            updatePools.Add(new());
        }

        Shuffle();

        for (int i = 0; i < TreeNumber; i++)
        {
            float lat = 0;
            float lon = 0;

            GameObject tree = null;

            while (tree == null)
            {
                lat = Random.Range(0f, 360f);
                lon = Random.Range(-90f, 90f);
                lon *= Mathf.Cos(lon);

                tree = ChooseTree(lat, lon);
            }

            var treeComponent = PlaceTree(tree, lat, lon).GetComponent<TreeVisualUpdate>();

            treeComponent.StartTree();

            updatePools[i % UpdatePoolNumber].Add(treeComponent);
        }
        
    }

    private void Shuffle()
    {
        List<int> ints = new();
        for (int i = 0; i < TreePools.Count; i++)
            ints.Add(i);

        while (ints.Count > 0)
        {
            int n = Random.Range(0, ints.Count - 1);
            RandomMatch.Add(ints[n]);
            ints.RemoveAt(n);
        }
    }

    private GameObject PlaceTree(GameObject tree, float lat, float lon)
    {
        var newTree = Instantiate(tree, new(0, 0, 0), new(0, 0, 0, 0), transform);

        newTree.transform.localRotation = Quaternion.Euler(new(lat, 0, lon));
        newTree.transform.Rotate(new(0, Random.Range(0f, 360f), 0));
        newTree.transform.position = newTree.transform.up * PlanetRadius;

        newTree.transform.parent = null;
        newTree.transform.localScale = Vector3.one * TreeSize * Mathf.Clamp(TreePerlineNoise(lat, lon), 0f, .4f);
        newTree.transform.parent = transform;

        return newTree;
    }

    private GameObject ChooseTree(float lat, float lon)
    {
        if (TreePerlineNoise(lat, lon) < .2)
            return null;

        int pool = Mathf.RoundToInt(Mathf.PerlinNoise(lat / 40, lon / 40 + 10000) * TreePools.Count);
        pool = Mathf.Clamp(pool, 0, TreePools.Count - 1);

        return TreePools[RandomMatch[pool]].RandomTree();
    }

    private float TreePerlineNoise(float lat, float lon)
    {
        return Mathf.PerlinNoise(lat / 10, lon / 10) * Mathf.PerlinNoise(lat / 20, lon / 20) * (Mathf.PerlinNoise(lat / 5, lon / 5) + .5f);
    }

    private void Update()
    {
        var pool = updatePools[poolTurnNumber];

        foreach (var tree in pool)
        {
            tree.GetComponent<TreeVisualUpdate>().TreeUpdate();
        }

        poolTurnNumber = (poolTurnNumber + 1) % UpdatePoolNumber;
    }
}



[Serializable]
public class TreePool
{
    public List<GameObject> Trees;

    public GameObject RandomTree()
    {
        return Trees[Random.Range(0, Trees.Count)];
    }
}