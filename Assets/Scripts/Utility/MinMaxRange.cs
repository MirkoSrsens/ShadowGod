using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.Utility
{

    [Serializable]
    [HideLabel]
    public struct MinMaxRange<T>
    {
        public T min;
        public T max;

        public MinMaxRange(T min, T max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
