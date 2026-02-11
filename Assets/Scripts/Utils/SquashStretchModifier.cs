using Sirenix.OdinInspector;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    [AddComponentMenu("UI/Effects/Squash Stretch Modifer")]
    [ExecuteAlways]
    [HideMonoScript]
    public sealed class SquashStretchModifier : MonoBehaviour, IMeshModifier
    {
        [LabelText("Squash-Stretch")]
        [SerializeField, Range(-1f, 1f)]
        float scale;

        [SerializeField]
        [Unit(Units.Degree)]
        float angle;

        [NonSerialized]
        float3x3? matrix;

        public float Scale
        {
            get => scale;
            set
            {
                scale = Mathf.Clamp(value, -1f, 1f);
                SetDirty();
            }
        }

        public Vector2 Direction
        {
            get => new(math.cos(math.radians(angle)), math.sin(math.radians(angle)));
            set
            {
                if (value.x == 0 && value.y == 0) return;
                angle = math.degrees(math.atan2(value.y, value.x));
                SetDirty();
            }
        }

        public float Angle
        {
            get => angle;
            set
            {
                angle = value;
                SetDirty();
            }
        }

        [Obsolete("use IMeshModifier.ModifyMesh (VertexHelper verts) instead", false)]
        public void ModifyMesh(Mesh mesh) { }

        public void ModifyMesh(VertexHelper vh)
        {
            if (!isActiveAndEnabled) return;

            AssertMatrix();

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                var v = default(UIVertex);
                vh.PopulateUIVertex(ref v, i);
                v.position = matrix.Value.TransformPoint(v.position);
                vh.SetUIVertex(v, i);
            }
        }

        void AssertMatrix()
        {
            if (matrix == null)
            {
                var modifier = 1f + Mathf.Abs(-scale);
                if (scale < 0f) modifier = 1f / modifier;

                matrix = MatrixUtility
                    .Rotate2D(angle * math.TORADIANS)
                    .Scale2D(new Vector2(1f / modifier, modifier))
                    .Rotate2D(-angle * math.TORADIANS);
            }
        }

        void SetDirty()
        {
            matrix = null;

            if (TryGetComponent<Graphic>(out var graphic))
            {
                graphic.SetVerticesDirty();
            }
        }

        void OnValidate()
        {
            scale = Mathf.Clamp(scale, -1f, 1f);
            SetDirty();
        }
    }
}
