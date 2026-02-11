using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonImage : Image
    {
        private PolygonCollider2D _collider;

        private static readonly List<Vector2> _path = new();

        private void CacheCollider()
        {
            if (!_collider) _collider = GetComponent<PolygonCollider2D>();
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            CacheCollider();

            if (!_collider) return false;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, screenPoint, eventCamera, out Vector2 uiLocal))
                return false;

            var world = rectTransform.TransformPoint(uiLocal);

            var colliderLocal = _collider.transform.InverseTransformPoint(world) - (Vector3)_collider.offset;

            var pathCount = _collider.pathCount;

            for (var i = 0; i < pathCount; i++)
            {
                _path.Clear();
                _collider.GetPath(i, _path);
                if (PointInPolygon(colliderLocal, _path)) return true;
            }

            return false;
        }

        private static bool PointInPolygon(Vector2 p, List<Vector2> poly)
        {
            var inside = false;

            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                var a = poly[i];
                var b = poly[j];

                var intersect =
                    ((a.y > p.y) != (b.y > p.y)) &&
                    (p.x < (b.x - a.x) * (p.y - a.y) / (b.y - a.y) + a.x);

                if (intersect)
                    inside = !inside;
            }

            return inside;
        }
    }
}
