using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razorhead.Core
{
    [Serializable]
    public struct ComparableFloat
    {
        public enum OperationType
        {
            LessThan,
            MoreThan,
            Equals,
            LessAndEqualThan,
            MoreAndEqualThan,
        }
        public OperationType operation;
        public float value;

        public bool Compare(float other)
        {
            if (operation == OperationType.Equals)
            {
                return value == other;
            }
            else if(operation == OperationType.LessThan)
            {
                return value < other;
            }
            else if(operation == OperationType.MoreThan)
            {
                return other < value;
            }
            else if(operation == OperationType.LessAndEqualThan)
            {
                return value <= other;
            }
            else if(operation == OperationType.MoreAndEqualThan)
            {
                return value >= other;
            }

                throw new ArgumentException();
        }
    }
}
