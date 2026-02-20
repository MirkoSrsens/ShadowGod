using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChainOfCommand
{
    [Serializable]
    public abstract class CommandCondition
    {
        public abstract bool IsValid(Entity entity);
    }
    
    
    public static class CommandConditionUtility
    {
        public static bool Check(this List<CommandCondition> list, Entity entity)
        {
            foreach(var item in list)
            {
                if(!item.IsValid(entity)) return false;
            }
    
            return true;
        }
    }
}