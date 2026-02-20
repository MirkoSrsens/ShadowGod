using ChainOfCommand;
using Cysharp.Threading.Tasks;
using Nodes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Nodes
{
    public class MoveToLocation_Node : Node
    {
        public float stoppingDistance = 0.1f;

        public override UniTask Initialize(Entity entity)
        {
            return base.Initialize(entity);
        }
        public override async UniTask Execute(Entity entity, CancellationTokenSource ctx)
        {
            var dat = entity.GetData<Destination_NodeData>();
            dat.agent.SetDestination(dat.destination);
            await UniTask.WaitUntil(() => WaitForDestinationReached(dat), cancellationToken: ctx.Token);
            dat.agent.ResetPath();
        }

        public bool WaitForDestinationReached(Destination_NodeData data)
        {
            return data.agent.remainingDistance < stoppingDistance;
        }

        public override float GetCompletionProgress(Entity entity)
        {
            throw new NotImplementedException();
        }

        public override bool IsCompleted(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
