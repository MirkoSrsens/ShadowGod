using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class AssetReferenceImage : AssetReferenceLoader<Sprite>
    {
        [SerializeField]
        private Image image;

        public override void Complete(Sprite prefab)
        {
            image.sprite = prefab;
            base.Complete(prefab);
        }
    }
}