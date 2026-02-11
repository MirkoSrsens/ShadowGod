using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public enum UILineTextureMode
    {
        Stretch,    // Entire line = 0..1
        Tile,       // Tile by world distance
        Distribute  // Evenly per segment
    }

    [AddComponentMenu("UI/UILineGraphic")]
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineGraphic : MaskableGraphic
    {
        public float thickness = 5f;
        public Vector2 textureScale = Vector2.one;
        public UILineTextureMode textureMode = UILineTextureMode.Stretch;
        public List<Vector2> points = new();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            int count = points.Count;
            if (count < 2)
                return;

            float half = thickness * 0.5f;
            Color32 col = color;

            // --- Length for UVs ---
            float totalLength = 0f;
            if (textureMode == UILineTextureMode.Stretch)
            {
                for (int i = 0; i < count - 1; i++)
                    totalLength += Vector2.Distance(points[i], points[i + 1]);
            }

            float accumulatedLength = 0f;

            Vector2[] dirs = null;
            Vector2[] norms = null;

            try
            {
                int segCount = count - 1;

                dirs = ArrayPool<Vector2>.Shared.Rent(segCount);
                norms = ArrayPool<Vector2>.Shared.Rent(segCount);

                // --- Precompute directions & normals ---
                for (int i = 0; i < segCount; i++)
                {
                    Vector2 d = (points[i + 1] - points[i]).normalized;
                    dirs[i] = d;
                    norms[i] = new Vector2(-d.y, d.x);
                }

                // --- Generate vertices ---
                for (int i = 0; i < count; i++)
                {
                    Vector2 normal;

                    if (i == 0)
                    {
                        normal = norms[0];
                    }
                    else if (i == count - 1)
                    {
                        normal = norms[segCount - 1];
                    }
                    else
                    {
                        Vector2 n0 = norms[i - 1];
                        Vector2 n1 = norms[i];

                        Vector2 miter = (n0 + n1).normalized;
                        float denom = Vector2.Dot(miter, n1);

                        if (denom < 0.1f)
                            normal = n1; // sharp angle fallback
                        else
                            normal = miter * (1f / denom);
                    }

                    Vector2 offset = normal * half;
                    var u = textureMode switch
                    {
                        UILineTextureMode.Stretch => accumulatedLength / totalLength,
                        UILineTextureMode.Tile => accumulatedLength,
                        // Distribute
                        _ => i,
                    };
                    u *= textureScale.x;

                    vh.AddVert(points[i] - offset, col, new Vector2(u, 0f));
                    vh.AddVert(points[i] + offset, col, new Vector2(u, textureScale.y));

                    if (i < count - 1)
                        accumulatedLength += Vector2.Distance(points[i], points[i + 1]);
                }

                // --- Build triangles ---
                for (int i = 0; i < count - 1; i++)
                {
                    int idx = i * 2;
                    vh.AddTriangle(idx + 0, idx + 1, idx + 3);
                    vh.AddTriangle(idx + 3, idx + 2, idx + 0);
                }
            }
            finally
            {
                if (dirs != null)
                    ArrayPool<Vector2>.Shared.Return(dirs);
                if (norms != null)
                    ArrayPool<Vector2>.Shared.Return(norms);
            }
        }

        public void SetPosition(int index, Vector2 position)
        {
            if (index >= 0 && index < points.Count)
            {
                points[index] = position;
                SetVerticesDirty();
            }
        }
    }
}
