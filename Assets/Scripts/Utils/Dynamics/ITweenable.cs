using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Razorhead.Core
{
    public interface ITweenable
    {
        bool IsDone();
        void UpdateTween();
        void Finish();
        void RemoveObj(Transform transform);
    }
}