using LitMotion;
using System;
using UnityEngine;

namespace Razorhead.Core
{
    public static class TweenCallbackUtility
    {
        public static MotionSequenceBuilder AppendCallback<T>(this MotionSequenceBuilder sequence, Action callback) where T : class
        {
            if (callback == null) return sequence;
            return sequence.Append(LMotion.Create(0, 0, 0).Bind(callback, (t, x) => x?.Invoke()));
        }

        public static MotionHandle BindToLocalScale<TOptions, TAdapter>(this MotionBuilder<float, TOptions, TAdapter> builder, Transform transform)
            where TOptions : unmanaged, IMotionOptions
            where TAdapter : unmanaged, IMotionAdapter<float, TOptions>
        {
            return builder.Bind(transform, static (x, t) =>
            {
                t.localScale = x * Vector3.one;
            });
        }
    }

}
