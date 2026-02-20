using Assets.Scripts.Data.Utility;
using Cysharp.Threading.Tasks;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ChainOfCommand
{
    [CreateAssetMenu(fileName = "Command", menuName = "Data/Chain of command/Command", order = 100)]
    public class Command : ScriptableObject
    {
        public int priority;

        public List<Command> canMoveTo = new();

        [SerializeReference]
        public List<Node> sequences = new();

        [Range(0f, 1f)]
        public float interaptableAt = 1;

        public List<CommandCondition> canBecomeActive = new();

        public async UniTask Initialize(Entity entity)
        {
            foreach (var item in sequences)
            {
                await item.Initialize(entity);
            }

            foreach(var item in canMoveTo)
            {
                await item.Initialize(entity);
            }
        }

        public bool CanBecomeActive(Entity entity)
        {
            return canBecomeActive.Check(entity);
        }

        public async UniTask Pass(Entity entity, CancellationTokenSource ctx)
        {
            foreach(var item in sequences)
            {
                await item.Execute(entity, ctx);
            }
        }

        internal bool CanEnd(Entity entity)
        {
            var progress = 0f;
            foreach(var sequence in sequences)
            {
                progress += sequence.GetCompletionProgress(entity);
            }

            if(sequences.Count == 0)
            {
                return progress / sequences.Count > interaptableAt;
            }

            return false;
        }

        internal bool IsCompleted(Entity entity)
        {
            foreach (var sequence in sequences)
            {
                if(!sequence.IsCompleted(entity))
                {
                    return false;
                }
            }

            return true;
        }

        internal Command GetNextAvailable(Entity entity)
        {
            Command next = null;
            foreach(var item in canMoveTo)
            {
                if(item.CanBecomeActive(entity))
                {
                    if(next == null || next.priority < item.priority)
                    {
                        next = item;
                    }

                    if(next.priority == item.priority)
                    {
                        next = UnityEngine.Random.Range(0, 2) == 0 ? next : item;
                    }
                }
            }

            return next;
        }
    }
}
