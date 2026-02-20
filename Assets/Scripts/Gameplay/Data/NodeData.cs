using ChainOfCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes.Data
{
    public class NodeData
    {
        public Entity entity;

        public virtual void Setup(Entity entity)
        {
            this.entity = entity;
        }
    }
}
