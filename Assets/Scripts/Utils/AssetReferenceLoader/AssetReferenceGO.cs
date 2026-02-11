using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Razorhead.Core
{
    public class AssetReferenceGO : AssetReferenceLoader<GameObject>
    {
        private Transform Parent => gameObject.transform;

        public override void Complete(GameObject prefab)
        {
            if (this && prefab)
            {
                currentGo = Instantiate(prefab, Parent.transform.position, Quaternion.identity, Parent);

                if (useSize && currentGo.TryGetComponent<RectTransform>(out var childRect))
                {
                    childRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                }
            }

            base.Complete(prefab);
        }
    }
}
