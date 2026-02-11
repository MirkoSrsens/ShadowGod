using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Razorhead.Core
{
    [RequireComponent(typeof(AssetReferenceGO))]
    public class AssetReferenceGOAwaiter : MonoBehaviour
    {
        public AssetReferenceGO go;

        public AssetReferenceGameObject assetRef;

        public Queue<Func<UniTask>> waitQueue = new();
        public DynamicCallback<GameObject> callback;

        public void OnEnable()
        {
            OnUpdate().AttachExternalCancellation(destroyCancellationToken).Forget();
        }

        public void SetObject(Func<UniTask> wait, AssetReferenceGameObject assetRef)
        {
            this.assetRef = assetRef;
            this.callback = default;
            waitQueue.Enqueue(wait);
        }

        public void SetObject(AssetReferenceGameObject assetRef)
        {
            this.assetRef = assetRef;
            this.callback = default;
        }

        public void SetObject<T>(AssetReferenceGameObject assRef, T data, Action<GameObject, T> onComplete)
        {
            this.assetRef = assRef;
            callback = DynamicCallback<GameObject>.Create(data, onComplete);
        }

        private async UniTask OnUpdate()
        {
            while (gameObject && isActiveAndEnabled)
            {
                while(waitQueue.Count > 0)
                {
                    await waitQueue.Dequeue().Invoke();
                }

                go.SetObject(assetRef, callback);

                await UniTask.Yield();
            }
        }
    }
}
