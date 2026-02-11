#pragma warning disable CS0649
using LitMotion;
using Noo.Tools;
using Sirenix.OdinInspector;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Razorhead.Core
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = "Razorhead/Custom Tween Ease")]
    public class TweenEase : ScriptableObject
    {
        [SerializeField, HideLabel, PropertySpace(SpaceAfter = 20)]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [FoldoutGroup("Generate")]
        [SerializeField, HideLabel]
        [SerializeReference, PolymorphicDrawerSettings(ShowBaseType = false)]
        ITweenEaseGenerator generator;

#if UNITY_EDITOR
        [FoldoutGroup("Generate")]
        [Button]
        void Generate()
        {
            if (generator != null)
            {
                curve.ClearKeys();
                generator.Generate(curve);
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

    public interface ITweenEaseGenerator
    {
        void Generate(AnimationCurve curve);
    }

    public static class TweenEaseUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionBuilder<TValue, TOptions, TAdapter> WithEase<TValue, TOptions, TAdapter>(this MotionBuilder<TValue, TOptions, TAdapter> builder, TweenEase ease)
            where TValue : unmanaged
            where TOptions : unmanaged, IMotionOptions
            where TAdapter : unmanaged, IMotionAdapter<TValue, TOptions>
        {
            if (ease) return builder.WithEase(ease.curve);
            return builder;
        }

        [Serializable]
        internal class SecondOrderSystem : ITweenEaseGenerator
        {
            static SODFloat state;

            public SODCurve sodCurve = SODCurve.Default;
            public int keyframes = 6;
            public float powDistribution = 1f;

            public void Generate(AnimationCurve curve)
            {
                var maxTime = FindMaxTime(sodCurve);

                for (int i = 0; i < keyframes; i++)
                {
                    var t = Mathf.Pow(i * (1f / (keyframes - 1)), powDistribution);
                    var value = FindValue(sodCurve, t * maxTime);
                    curve.AddKey(t, value);
                }
            }

            float FindValue(SODCurve curve, float time)
            {
                state ??= new SODFloat();
                state.Curve = curve;
                state.Reset(0f);
                state.Target = 1f;
                state.Update(time);
                return state.Value;
            }

            float FindMaxTime(SODCurve curve)
            {
                state ??= new SODFloat();
                state.Curve = curve;
                state.Reset(0f);
                state.Target = 1f;
                var t = 0f;

                while (t < 10f)
                {
                    t += SecondOrderDynamics.DeltaTime;
                    state.Update(SecondOrderDynamics.DeltaTime);

                    if (Mathf.Abs(state.PreviousValue - state.Target) < 0.001f && Mathf.Abs(state.Value - state.Target) < 0.001f)
                    {
                        break;
                    }
                }

                return t;
            }
        }

        [Serializable]
        internal class CommonEaseLibrary : ITweenEaseGenerator
        {
            public Ease ease;
            public int keyframes = 6;
            public float powDistribution = 1f;

            public void Generate(AnimationCurve curve)
            {
                for (int i = 0; i < keyframes; i++)
                {
                    var t = Mathf.Pow(i * (1f / (keyframes - 1)), powDistribution);
                    curve.AddKey(t, EaseUtility.Evaluate(t, ease));
                }
            }
        }
    }
}
