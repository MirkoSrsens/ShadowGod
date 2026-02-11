using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Razorhead.Core
{
    public static class EnumUtility
    {
        public static bool HasFlagNonAlloc<TEnum>(this TEnum value, TEnum flag) where TEnum : unmanaged, Enum
        {
            var a = UnsafeUtility.EnumToInt(value);
            var b = UnsafeUtility.EnumToInt(flag);
            return (a & b) == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum Set<TEnum>(this TEnum enumValue, TEnum flag) where TEnum : unmanaged, Enum
        {
            var value = UnsafeUtility.EnumToInt(enumValue);
            var flagValue = UnsafeUtility.EnumToInt(flag);
            value |= flagValue;
            return UnsafeUtility.As<int, TEnum>(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum Unset<TEnum>(this TEnum enumValue, TEnum flag) where TEnum : unmanaged, Enum
        {
            var value = UnsafeUtility.EnumToInt(enumValue);
            var flagValue = UnsafeUtility.EnumToInt(flag);
            value &= ~flagValue;
            return UnsafeUtility.As<int, TEnum>(ref value);
        }
    }
}
