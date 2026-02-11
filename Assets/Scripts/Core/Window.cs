using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class Window<T> : WindowBase where T : MonoBehaviour
    {
        public static T Inst;

        public override UniTask OnLoad()
        {
            if (this is T ins)
            {
                Inst = ins;
            }
            return base.OnLoad();
        }

        public override async UniTask OnUnload()
        {
            await base.OnUnload();
            Inst = null;
        }
    }
}