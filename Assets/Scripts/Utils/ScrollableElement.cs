using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Razorhead.Core
{
    public abstract class ScrollableElement : MonoBehaviour
    {
        public RectTransform RectTransform => GetComponent<RectTransform>();

        public Vector2 rectSize;
        public abstract void Setup(object data);
    }
}