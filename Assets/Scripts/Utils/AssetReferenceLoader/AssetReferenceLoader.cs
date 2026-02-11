using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Razorhead.Core
{
    public abstract class AssetReferenceLoader<TType> : MonoBehaviour, IPoolManagedObject where TType : UnityEngine.Object
    {
        public AssetReferenceT<TType> assetRef;
        public TType currentGo;
        public bool useSize = false;

        private UniTaskCompletionSource<TType> tcs;
        private CancellationTokenSource cts;
        private AsyncOperationHandle<TType> operationHandle;
        private DynamicCallback<TType> callback;

        public AssetReferenceT<TType> go
        {
            set
            {
                SetObject(value, null);
            }
        }

        private void Release(bool clearRef = true)
        {
            if (currentGo)
            {
                Destroy(currentGo);
                currentGo = null;
            }

            if (operationHandle.IsValid())
            {
                operationHandle.Release();
                operationHandle = default;
            }

            if (tcs != null)
            {
                Complete(null);
            }

            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }

            if (clearRef)
            {
                assetRef = null;
            }
        }

        public void SetObject(AssetReferenceT<TType> assRef, Action<TType> onComplete = null)
        {
            callback = DynamicCallback<TType>.Create(onComplete);

            SetObjectAsync(assRef).Forget();
        }

        public void SetObject(AssetReferenceT<TType> assRef, DynamicCallback<TType> onComplete = default)
        {
            callback = onComplete;

            SetObjectAsync(assRef).Forget();
        }

        public void SetObject<T>(AssetReferenceT<TType> assRef, T data, Action<TType, T> onComplete)
        {
            callback = DynamicCallback<TType>.Create(data, onComplete);

            SetObjectAsync(assRef).Forget();
        }

        /// <summary>
        /// Returns NULL if the operation is canceled or overwritten with new reference before it finishes loading.
        /// </summary>
        public async UniTask<TType> SetObjectAsync(AssetReferenceT<TType> assRef)
        {
            if (assRef.IsNull())
            {
                Release();
                return null;
            }

            if (assetRef != assRef)
            {
                Release();

                assetRef = assRef;

                if (assetRef.IsNull())
                {
                    return null;
                }

                operationHandle = Addressables.LoadAssetAsync<TType>(assetRef);

                // If we call this in edit mode we need to handle it synchronously
                if (!Application.isPlaying) operationHandle.WaitForCompletion();

                // Asset is already loaded, we can skip actual loading
                if (operationHandle.IsValid() && operationHandle.Result)
                {
                    Complete(operationHandle.Result);
                    return currentGo;
                }
                else
                {
                    Load().Forget();
                    return await tcs.Task;
                }
            }
            else if (tcs != null)
            {
                return await tcs.Task;
            }

            return currentGo;
        }

        private async UniTaskVoid Load()
        {
            cts = new CancellationTokenSource();
            tcs = new UniTaskCompletionSource<TType>();

            try
            {
                await operationHandle.ToUniTask(cancellationToken: cts.Token);

                // Some objects like Animators and Spine Animations needs to be instantiated during Update loop
                // so they have time to update their animations before rendering otherwise they will show invalid frames
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cts.Token);

                Complete(operationHandle.Result);
            }
            catch (OperationCanceledException)
            {
                // No need to do anything if we cancelled manually
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);

                Complete(null);
            }
        }

        public virtual void Complete(TType prefab)
        {
            if (tcs != null)
            {
                tcs.TrySetResult(currentGo);
                tcs = null;
            }

            callback.Invoke(currentGo);
        }

        public void ReleasedToPool()
        {
            Release(false);
        }

        public void SpawnFromPool()
        {
            if (assetRef != null)
            {
                var tmp = assetRef;
                assetRef = null;
                SetObject(tmp, null);
            }
        }

        void OnDestroy()
        {
            Release();
        }
    }
}
