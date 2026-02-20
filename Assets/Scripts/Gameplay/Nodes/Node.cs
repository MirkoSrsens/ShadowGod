using ChainOfCommand;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nodes
{
    public abstract class Node
    {
        public virtual UniTask Initialize(Entity entity)
        {
            return UniTask.CompletedTask;
        }

        public abstract UniTask Execute(Entity entity, CancellationTokenSource ctx);
        public abstract float GetCompletionProgress(Entity entity);
        public abstract bool IsCompleted(Entity entity);
    }
}
