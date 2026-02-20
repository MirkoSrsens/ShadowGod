using Cysharp.Threading.Tasks;
using Nodes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace ChainOfCommand
{
    public class Entity : MonoBehaviour
    {
        public Dictionary<Type, NodeData> nodes;
        public CancellationTokenSource ctx;
        public Command startCommand;
        public Command activeCommand;

        public async void Start()
        {
            await startCommand.Initialize(this);
            activeCommand = startCommand;
            ctx = new CancellationTokenSource();
            await RunUpdate();
        }

        private async Task RunUpdate()
        {
            while(true)
            {
                using var _ = ListPool<UniTask>.Get(out var tasks);

                tasks.Add(activeCommand.Pass(this, ctx));
                tasks.Add(UpdateState(ctx));

                await UniTask.WhenAll(tasks);
                await UniTask.WaitForEndOfFrame();
            }
        }

        public async UniTask UpdateState(CancellationTokenSource ctx)
        {
            while(true)
            {
                if (activeCommand.IsCompleted(this) || activeCommand.CanEnd(this))
                {
                    var result = activeCommand.GetNextAvailable(this);

                    if(result != null)
                    {
                        activeCommand = result;
                        ctx.Cancel();
                        ctx = new CancellationTokenSource();
                    }
                }

                await UniTask.WaitForEndOfFrame();
            }
        }

        internal T GetData<T>() where T : NodeData, new()
        {
            if (nodes.TryGetValue(typeof(T), out var data))
            {
                return (T)data;
            }

            data = new T();
            data.Setup(this);

            nodes.Add(typeof(T), data);
            return (T)data;
        }
    }
}
