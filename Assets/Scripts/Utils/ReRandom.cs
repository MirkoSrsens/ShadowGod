using Newtonsoft.Json;
using Noo.Tools;
using Sirenix.OdinInspector;
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Razorhead.Core
{
    [Serializable, InlineProperty]
    public class ReRandom
    {
        public readonly static ReRandom Default = new((uint)Environment.TickCount);

        const uint C1 = 842502087, C2 = 3579807591, C3 = 273326509;

        [JsonProperty]
        [SerializeField, LabelWidth(30f)]
        uint seed;

        [JsonIgnore]
        [NonSerialized]
        uint4? rng;

        [JsonIgnore]
        public uint Seed
        {
            get
            {
                return seed;
            }
            set
            {
                seed = value;
                ResetCounter();
            }
        }

        public ReRandom()
        {
        }

        public ReRandom(uint seed)
        {
            this.seed = seed;
            ResetCounter();
        }

        public uint NextUint()
        {
            rng ??= new uint4(seed, C1, C2, C3);
            var state = rng.Value;
            uint t = (state.x ^ (state.x << 11));
            state.x = state.y; state.y = state.z; state.z = state.w;
            state.w = (state.w ^ (state.w >> 19)) ^ (t ^ (t >> 8));
            rng = state;
            return state.w;
        }

        /// <summary>float value in full range</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Next() => (float)(NextUint() / (double)uint.MaxValue);

        /// <summary>float value in range [0(inclusive)-1(exclusive)]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Range01() => math.frac(Next());

        /// <summary>float value in range [min(inclusive)-max(exclusive)]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Range(float min, float max)
        {
            return min + (max - min) * Range01();
        }

        public Sdouble SdNext()
        {
            return Sdouble.FromRaw((long)(((ulong)NextUint() << 32) | NextUint()));
        }

        /// <summary>Sdouble value in range [0(inclusive)-1(exclusive)]</summary>
        public Sdouble SdRange01() => Sdouble.Fract(SdNext());

        /// <summary>Sdouble value in range [0(inclusive)-max(exclusive)]</summary>
        public Sdouble SdRange(Sdouble max) => SdRange01() * max;

        /// <summary>Sdouble value in range [min(inclusive)-max(exclusive)]</summary>
        public Sdouble SdRange(Sdouble min, Sdouble max)
        {
            return min + Sdouble.FloorToInt((max - min) * SdRange01());
        }

        /// <summary>Sdouble value in [-<paramref name="range"/> to <paramref name="range"/>] from <paramref name="origin"/></summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sdouble SdSpread(Sdouble origin, Sdouble range) => SdRange(origin - range, origin + range);

        /// <summary>float value in range [0(inclusive)-max(exclusive)]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Range(float max)
        {
            return Range01() * max;
        }

        /// <summary>float value in [-<paramref name="range"/> to <paramref name="range"/>]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Spread(float range) => Range(-range, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2 Spread(float2 range) => new(Range(-range.x, range.x), Range(-range.y, range.y));

        /// <summary>float value in [-<paramref name="range"/> to <paramref name="range"/>] from <paramref name="origin"/></summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Spread(float origin, float range) => Range(origin - range, origin + range);

        /// <summary>float value in range [min(inclusive)-max(exclusive)]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Range(int min, int max)
        {
            return min + Mathf.FloorToInt((max - min) * Range01());
        }

        /// <summary>float value in range [0(inclusive)-max(exclusive)]</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Range(int max)
        {
            return Mathf.FloorToInt((max) * Range01());
        }

        /// <summary>True in 50% cases</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NextBool()
        {
            return Range01() < 0.5f;
        }

        /// <param name="chance">Expected range [0-1]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NextBool(float chance)
        {
            return Range01() < chance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float NextSign()
        {
            return Range01() < 0.5f ? 1f : -1f;
        }

        public void ResetCounter()
        {
            rng = new uint4(seed, C1, C2, C3);
        }

        internal void NewSeed()
        {
            var rnd = new System.Random();
            seed = (uint)rnd.Next(0, int.MaxValue);
            ResetCounter();
        }

        internal void NewSeed(int seed)
        {
            this.seed = (uint)seed;
            ResetCounter();
        }

        public static ReRandom Create()
        {
            var rng = new ReRandom();
            rng.NewSeed();
            rng.ResetCounter();
            return rng;
        }

        public static ReRandom Create(ref ReRandom seed)
        {
            if (seed == null)
            {
                seed = new ReRandom();
                seed.NewSeed();
            }

            seed.ResetCounter();

            return seed;
        }
    }
}
