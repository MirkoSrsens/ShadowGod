using Cysharp.Threading.Tasks;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class WindowBase : MonoBehaviour
    {
        public bool IsLoaded { get; protected set; }

        public bool invisible;
        public Canvas canvas;
        public string id;
        public EventReference backgroundMusic;

        [Tooltip("Interactable that will be selected when window opens")]
        public Selectable defaultSelectedObject;

        private CancellationTokenSource ctx;

        public virtual UniTask OnLoad()
        {
            Assert.IsFalse(IsLoaded, "Expected window to be unloaded before loading.");
            IsLoaded = true;
            ctx = new CancellationTokenSource();
            return UniTask.CompletedTask;
        }

        public async UniTask StartUpdateExecution()
        {
            while (gameObject && IsLoaded)
            {
                await UniTask.Yield();
                await OnUpdate().AttachExternalCancellation(ctx.Token);
            }
        }

        public virtual UniTask OnUnload()
        {
            IsLoaded = false;
            ctx?.Cancel();
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnUpdate()
        {
            return UniTask.CompletedTask;
        }
    }
}