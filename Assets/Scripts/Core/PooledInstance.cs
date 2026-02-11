using Sirenix.OdinInspector;
using UnityEngine;

namespace Razorhead.Core
{
    public class PooledInstance : MonoBehaviour
    {
        [ReadOnly]
        public bool inPool;

        [ReadOnly]
        public GameObject prefab;
    }

    public static class PooledInstanceUtility
    {
        public static bool TryGetSourcePrefab(this GameObject source, out GameObject prefab)
        {
            prefab = default;
            if (!source) return false;
            if (!source.TryGetComponent<PooledInstance>(out var pooledInstance)) return false;
            prefab = pooledInstance.prefab;
            return prefab;
        }
    }
}