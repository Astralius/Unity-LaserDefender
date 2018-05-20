using System.Collections.Generic;
using UnityEngine;

namespace LaserDefender.Assets.Scripts.Interfaces
{
    public interface IObjectPooler
    {
        void AddPool(GameObject prefabToRegister);
        void AddPool(GameObject prefabToRegister, int poolSize);
        void ExpandPool(GameObject registeredPrefab, int targetSize);
        void RemovePool(GameObject registeredPrefab);
        void RemoveAllPools();
        void ClearPool(GameObject registeredPrefab);
        void ClearAllPools();

        void SpawnInstance(GameObject registeredPrefab, Vector3 position, Quaternion rotation);
        GameObject GetNextInactiveInstance(GameObject registeredPrefab);
        List<GameObject> GetAllInactiveInstances(GameObject registeredPrefab);
        List<GameObject> GetAllActiveInstances(GameObject registeredPrefab);
        List<GameObject> GetAllInstances(GameObject registeredPrefab);
    }
}
