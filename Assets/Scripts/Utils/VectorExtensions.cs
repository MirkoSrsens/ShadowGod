using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Razorhead.Core
{
    public static class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Rotate90(this Vector2Int vector) => new(vector.y, -vector.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Rotate180(this Vector2Int vector) => new(-vector.x, -vector.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Rotate270(this Vector2Int vector) => new(-vector.y, vector.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Rotate90X(this List<Vector2Int> vector, int rotations)
        {
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] = Rotate90X(vector[i], rotations);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Rotate90X(this Vector2Int vector, int rotations)
        {
            return (rotations % 4) switch
            {
                -3 => Rotate90(vector),
                -2 => Rotate180(vector),
                -1 => Rotate270(vector),
                0 => vector,
                1 => Rotate90(vector),
                2 => Rotate180(vector),
                3 => Rotate270(vector),
                _ => vector,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate90(this Vector2 vector) => new(vector.y, -vector.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate180(this Vector2 vector) => new(-vector.x, -vector.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate270(this Vector2 vector) => new(-vector.y, vector.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate90X(this Vector2 vector, int rotations)
        {
            return (rotations % 4) switch
            {
                -3 => Rotate90(vector),
                -2 => Rotate180(vector),
                -1 => Rotate270(vector),
                0 => vector,
                1 => Rotate90(vector),
                2 => Rotate180(vector),
                3 => Rotate270(vector),
                _ => vector,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this List<Vector2Int> positions)
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;

            for (int i = 0; i < positions.Count; ++i)
            {
                minX = Mathf.Min(minX, positions[i].x);
                minY = Mathf.Min(minY, positions[i].y);
            }

            for (int i = 0; i < positions.Count; ++i)
            {
                positions[i] -= new Vector2Int(minX, minY);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Offset(this List<Vector2Int> positions, Vector2Int offset)
        {
            for (int i = 0; i < positions.Count; ++i)
            {
                positions[i] += offset;
            }
        }

        /// <summary>
        /// Roates vectors in list. This will modify list values.
        /// </summary>
        /// <param name="positions">List of vectors.</param>
        /// <param name="rotation">x90 times item will be rotated.</param>
        /// <param name="offset">Offset that will applied after normalization.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Rotate90XNormalizedWithOffset(this List<Vector2Int> positions, int rotation, Vector2Int offset)
        {
            var shape = new List<Vector2Int>();
            var center = new Vector2Int(0, 0);

            positions.Normalize();
            positions.Rotate90X(rotation);
            positions.Normalize();
            positions.Offset(offset);
        }

        public static Vector2 Middle(this List<Vector2Int> positions)
        {
            return (Vector2)positions.Sum() / positions.Count;
        }

        public static Vector2Int Sum(this List<Vector2Int> positions)
        {
            var sum = default(Vector2Int);
            for (int i = 0; i < positions.Count; i++) sum += positions[i];
            return sum;
        }

        public static Vector2Int Sign(this Vector2 vector)
        {
            return Vector2Int.RoundToInt(vector).Normalize();
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        public static Vector2Int RoundToInt(this Vector2 vector)
        {
            return Vector2Int.RoundToInt(vector);
        }

        /// <summary>Normalizes X and Y to -1,0 or 1 values.</summary>
        public static Vector2Int Normalize(this Vector2Int v)
        {
            return new(v.x == 0 ? 0 : (v.x < 0 ? -1 : 1), v.y == 0 ? 0 : (v.y < 0 ? -1 : 1));
        }

        public static Vector2 GetCardinalDirection(this Vector2 vector)
        {
            if (vector == Vector2.zero) return Vector2.zero;
            var abs = vector.Abs();
            if (abs.x > abs.y) return new Vector2(Mathf.Sign(vector.x), 0);
            else return new Vector2(0, Mathf.Sign(vector.y));
        }
    }
}
