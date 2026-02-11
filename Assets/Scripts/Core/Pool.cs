using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Razorhead.Core
{
    public class Pool : SingletonBehaviour<Pool>
    {
        static Scene parentScene;

        public Dictionary<GameObject, Stack<GameObject>> elementsInPool = new();
        public List<IPoolManagedObject> managedSpawn = new();
        public Transform inactiveParent; //obsolete, we are using parent scene for this now

        private void Awake()
        {
            parentScene = SceneManager.CreateScene("Pool");
        }

        internal void Return(GameObject obj, bool returnToPoolScene = true) => Return(obj.transform, returnToPoolScene);

        internal void Return<T>(T item, bool returnToPoolScene = true) where T : Component
        {
            if(item == null) return;

            if (!item.TryGetComponent<PooledInstance>(out var comp))
            {
                Destroy(item.gameObject);
                return;
            }

            if (comp.inPool)
            {
                Debug.LogWarning($"Trying to release an object ({item.gameObject}) that has already been released to the pool.", item.gameObject);
                return;
            }

            if (!elementsInPool.ContainsKey(comp.prefab))
            {
                elementsInPool.Add(comp.prefab, new Stack<GameObject>());
            }

            item.GetComponentsInChildren(true, managedSpawn);

            if (!managedSpawn.IsEmpty())
            {
                foreach (var tR in managedSpawn)
                {
                    tR.ReleasedToPool();
                }
            }

            item.gameObject.SetActive(false);

            if (returnToPoolScene)
            {
                item.transform.SetParent(null, false);
                SceneManager.MoveGameObjectToScene(item.gameObject, parentScene);
            }

            elementsInPool[comp.prefab].Push(item.gameObject);
            comp.inPool = true;
        }

        internal GameObject Spawn(GameObject obj, Transform parent, Vector3 position, bool activate = true)
        {
            return Spawn(obj.transform, parent, position, activate).gameObject;
        }

        internal T Spawn<T>(T obj, Vector3 position, bool activate = true) where T : Component
        {
            return Spawn(obj, default(Transform), position, activate);
        }

        internal T Spawn<T>(T obj, Transform parent, Vector3 position, bool activate = true) where T : Component
        {
            T inst;

            if (obj && elementsInPool.ContainsKey(obj.gameObject)
               && elementsInPool[obj.gameObject].Count > 0)
            {
                inst = elementsInPool[obj.gameObject].Pop().GetComponent<T>();
                inst.transform.SetParent(parent, false);
                inst.transform.position = position;
                inst.gameObject.SetActive(activate);
            }
            else
            {
                var wasActive = obj.gameObject.activeSelf;
                obj.gameObject.SetActive(activate);
                inst = Instantiate(obj, position, Quaternion.identity, parent);
                obj.gameObject.SetActive(wasActive);
            }

            inst.GetComponentsInChildren(true, managedSpawn);

            if (!managedSpawn.IsEmpty())
            {
                foreach (var tR in managedSpawn)
                {
                    tR.SpawnFromPool();
                }
            }

            if (!inst.TryGetComponent<PooledInstance>(out var pooledInfo))
            {
                pooledInfo = inst.gameObject.AddComponent<PooledInstance>();
                pooledInfo.prefab = obj.gameObject;
            }

            pooledInfo.inPool = false;

            return inst;
        }

        internal T Spawn<T>(T obj, RectTransform parent, Vector3 position) where T : Component
        {
            T inst;

            if (obj && elementsInPool.ContainsKey(obj.gameObject)
               && elementsInPool[obj.gameObject].Count > 0)
            {
                inst = elementsInPool[obj.gameObject].Pop().GetComponent<T>();
                inst.transform.SetParent(parent, false);
                inst.GetComponent<RectTransform>().anchoredPosition3D = position;
                inst.gameObject.SetActive(true);
            }
            else
            {
                inst = Instantiate(obj, position, Quaternion.identity, parent);
            }

            inst.GetComponentsInChildren(true, managedSpawn);

            if (!managedSpawn.IsEmpty())
            {
                foreach (var tR in managedSpawn)
                {
                    tR.SpawnFromPool();
                }
            }

            if (!inst.TryGetComponent<PooledInstance>(out var pooledInfo))
            {
                pooledInfo = inst.gameObject.AddComponent<PooledInstance>();
                pooledInfo.prefab = obj.gameObject;
            }

            pooledInfo.inPool = false;

            return inst;
        }
    }
}
