using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Razorhead.Core
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _inst;
    
        public static T Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = FindAnyObjectByType<T>();
                }
    
                return _inst;
            }
        }
    }
}