using Unity.Mathematics;
using UnityEngine;

namespace Razorhead.Core
{
    public static class MatrixUtility
    {
        public static float3x3 Rotate2D(float degrees)
        {
            float c = math.cos(degrees);
            float s = math.sin(degrees);

            return new float3x3(
                new float3(c, s, 0),
                new float3(-s, c, 0),
                new float3(0, 0, 1)
            );
        }

        public static float3x3 Rotate2D(this float3x3 matrix, float degrees)
        {
            return math.mul(matrix, Rotate2D(degrees));
        }

        public static float3x3 Skew2D(Vector2 skew)
        {
            return new float3x3(
                new float3(1, skew.y, 0),
                new float3(skew.x, 1, 0),
                new float3(0, 0, 1)
            );
        }

        public static float3x3 Skew2D(this float3x3 matrix, Vector2 skew)
        {
            return math.mul(matrix, Skew2D(skew));
        }

        public static float3x3 Scale2D(Vector2 scale)
        {
            return new float3x3(
                new float3(scale.x, 0, 0),
                new float3(0, scale.y, 0),
                new float3(0, 0, 1)
            );
        }

        public static float3x3 Scale2D(this float3x3 matrix, Vector2 scale)
        {
            return math.mul(matrix, Scale2D(scale));
        }

        public static float3x3 Inverse(this float3x3 matrix)
        {
            return math.inverse(matrix);
        }

        public static Vector2 TransformPoint(this float3x3 matrix, Vector2 point)
        {
            return math.mul(matrix, new float3(point.x, point.y, 1f)).xy;
        }
    }
}
