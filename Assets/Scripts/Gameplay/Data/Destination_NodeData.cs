using ChainOfCommand;
using UnityEngine;
using UnityEngine.AI;

namespace Nodes.Data
{
    public class Destination_NodeData : NodeData
    {
        public Vector3 destination;
        public NavMeshAgent agent;

        public override void Setup(Entity entity)
        {
            base.Setup(entity);
            destination = Vector3.zero;
            agent = entity.GetComponent<NavMeshAgent>();
        }
    }
}
