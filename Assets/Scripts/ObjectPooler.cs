using LaserDefender.Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour, IObjectPooler
{
    [Tooltip("Factor by which pool size will be expanded (multiplied) if the entire pool is already active.")]
    [Range(0, 100)]
    public int ExpansionFactor = 2;

    private readonly Dictionary<GameObject, List<GameObject>> objectPools =
        new Dictionary<GameObject, List<GameObject>>();

    private new Transform transform;

    private void Awake()
    {
        this.transform = GetComponent<Transform>();
    }

    public void AddPool(GameObject prefabToRegister)
    {
        if (objectPools.ContainsKey(prefabToRegister))
        {
            Debug.LogWarning("The selected prefab already has its objects pool!");
            return;
        }
        objectPools.Add(prefabToRegister, new List<GameObject>());
        var container = new GameObject(prefabToRegister.name);
        container.transform.parent = transform;
    }

    public void AddPool(GameObject prefabToRegister, int increment)
    {
        if (!objectPools.ContainsKey(prefabToRegister))
        {
            AddPool(prefabToRegister);
        }
        ExpandPool(prefabToRegister, increment);
    }

    public void ClearAllPools()
    {
        foreach (var pool in objectPools)
        {
            pool.Value.Clear();
        }
    }

    public void ClearPool(GameObject registeredPrefab)
    {
        List<GameObject> pool;
        if (objectPools.TryGetValue(registeredPrefab, out pool))
        {
            pool.Clear();
        }
    }

    public List<GameObject> GetAllActiveInstances(GameObject registeredPrefab)
    {
        try
        {
            return GetPool(registeredPrefab).Where(instance => instance.activeSelf).ToList();
        }
        catch (ArgumentException)
        {
            throw;
        }
    }

    public List<GameObject> GetAllInactiveInstances(GameObject registeredPrefab)
    {
        try
        {
            return GetPool(registeredPrefab).Where(instance => !instance.activeSelf).ToList();
        }
        catch (ArgumentException)
        {
            throw;
        }     
    }

    public List<GameObject> GetAllInstances(GameObject registeredPrefab)
    {
        try
        {
            return GetPool(registeredPrefab);
        }
        catch (ArgumentException)
        {
            throw;
        }
    }

    public GameObject GetNextInactiveInstance(GameObject registeredPrefab)
    {
        try
        {
            var pool = GetPool(registeredPrefab);
            var nextInstance = pool.FirstOrDefault(instance => !instance.activeSelf);
            if (nextInstance == null)
            {
                ExpandPool(registeredPrefab, pool);
            }
            return pool.First(instance => !instance.activeSelf);
        }
        catch (ArgumentException)
        {
            throw;
        }
    }

    public void SpawnInstance(GameObject registeredPrefab, Vector3 position, Quaternion rotation)
    {
        var instance = GetNextInactiveInstance(registeredPrefab);
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.SetActive(true);
    }

    public void RemovePool(GameObject registeredPrefab)
    {
        if (objectPools.ContainsKey(registeredPrefab))
        {
            objectPools.Remove(registeredPrefab);
            Destroy(transform.Find(registeredPrefab.name));
        }
    }

    public void RemoveAllPools()
    {
        foreach (var entry in objectPools)
        {
            objectPools.Remove(entry.Key);
            Destroy(transform.Find(entry.Key.name));
        }
    }

    public void ExpandPool(GameObject registeredPrefab, int increment)
    {
        if (increment < 1)
        {
            throw new ArgumentException("Increment cannot be zero or negative!");
        }

        List<GameObject> pool;
        try
        {
            pool = GetPool(registeredPrefab);
        }
        catch (ArgumentException)
        {
            throw;
        }
       
        for (var i = 0; i < increment; i++)
        {
            var go = GameObject.Instantiate(registeredPrefab) as GameObject;
            go.SetActive(false);
            go.transform.parent = transform.Find(registeredPrefab.name);
            pool.Add(go);
        }      
    }

    private void ExpandPool(GameObject registeredPrefab, List<GameObject> pool)
    {
        int currentSize = objectPools[registeredPrefab].Count;
        int targetSize = (currentSize > 0) ? (currentSize * ExpansionFactor) : 1;

        for (var i = 0; i < targetSize - currentSize ; i++)
        {
            var go = GameObject.Instantiate(registeredPrefab) as GameObject;
            go.SetActive(false);
            go.transform.parent = transform.Find(registeredPrefab.name);
            pool.Add(go);
        }
    }

    private List<GameObject> GetPool(GameObject registeredPrefab)
    {
        List<GameObject> pool = null;
        if (!objectPools.TryGetValue(registeredPrefab, out pool))
        {
            throw new ArgumentException(
                "The provided prefab has no corresponding pool or is not valid!");
        }
        return pool;
    }
}
