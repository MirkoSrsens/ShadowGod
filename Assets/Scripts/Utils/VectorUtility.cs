using System.Runtime.CompilerServices;
using UnityEngine;

namespace Razorhead.Core
{
    public static class VectorUtility
    {
        public static float VectorToAngle(Vector2 a)
        {
            if (a.x == 0 && a.y == 0) return default;
            return Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        }

        /// <summary>Angle in degrees</summary>
        public static Vector2 AngleToVector(float angle)
        {
            angle *= Mathf.Deg2Rad;
            return new(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}
