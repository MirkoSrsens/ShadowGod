using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class RecycleScrollingList : BaseScrollingList
    {
        public RectTransform RectTransform => GetComponent<RectTransform>();

        public ScrollRect scroller;

        public ScrollDirection direction;
        public TextAnchor alignment = TextAnchor.MiddleCenter;

        public Vector2 padding;
        public Vector2 spacing;
        public Vector2 viewRectSizeAddup;

        public Vector2? originalContainerSize;
        public Vector2? scrollOffset;

        [Button]
        public void TryAgain()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                Pool.Inst.Return(entity.spawnedElement);
                entity.spawnedElement = null;
                entity.rect = null;
            }

            SetLayoutDirty();
            Refresh();
        }

        public override void Restart()
        {
            base.Restart();

            if (scroller && scroller.content)
            {
                scroller.normalizedPosition = new Vector2(0f, 1f);
            }
        }

        public void Refresh()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            var viewportSize = RectTransform.rect.size;
            var containerDirty = !originalContainerSize.HasValue || originalContainerSize.Value != viewportSize;

            originalContainerSize = viewportSize;
            scrollOffset = scroller ? scroller.normalizedPosition : default;

            if (layoutDirty || containerDirty)
            {
                RecalculateLayout();
            }

            if (scroller)
            {
                scroller.horizontal = direction == ScrollDirection.Horizontal;
                scroller.vertical = direction == ScrollDirection.Vertical;
            }

            var localViewportRect = new Rect(-viewRectSizeAddup / 2f, originalContainerSize.Value + viewRectSizeAddup);

            if (container)
            {
                localViewportRect.position -= Vector2.Scale(container.anchoredPosition, new Vector2(1, -1));
            }

            foreach (var entity in entities)
            {
                var isVisible = entity.rect.HasValue && localViewportRect.Overlaps(entity.rect.Value);

                if (isVisible)
                {
                    if (entity.spawnedElement == null)
                    {
                        entity.spawnedElement = SpawnItem(entity.prefab, default);
                        entity.spawnedElement.Setup(entity.data);
                    }

                    var rt = entity.spawnedElement.GetComponent<RectTransform>();
                    rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
                    rt.anchoredPosition3D = Vector2.Scale(entity.rect.Value.position, new Vector2(1, -1));
                }
                else
                {
                    if (entity.spawnedElement != null)
                    {
                        Pool.Inst.Return(entity.spawnedElement);
                        entity.spawnedElement = null;
                    }
                }
            }
        }

        private void RecalculateLayout()
        {
            var containerSize = originalContainerSize.Value - padding * 2;
            var offset = Vector2.zero;
            var contentSize = Vector2.zero;

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var entityRect = new Rect(Vector2.zero, entity.prefab.rectSize);
                var remainingSize = containerSize - offset;

                if (direction == ScrollDirection.Vertical)
                {
                    if (remainingSize.x > entityRect.size.x)
                    {
                        entityRect.position = offset;
                        offset.x += entityRect.size.x + spacing.x;
                    }
                    else
                    {
                        offset = new Vector2(entityRect.size.x + spacing.x, contentSize.y);
                        entityRect.position = new Vector2(0f, contentSize.y);
                    }
                }
                else if (direction == ScrollDirection.Horizontal)
                {
                    if (remainingSize.y > entityRect.size.y)
                    {
                        entityRect.position = offset;
                        offset.y += entityRect.size.y + spacing.y;
                    }
                    else
                    {
                        offset = new Vector2(contentSize.x, entityRect.size.y + spacing.y);
                        entityRect.position = new Vector2(contentSize.x, 0f);
                    }
                }

                contentSize = Vector2.Max(contentSize, entityRect.position + entityRect.size + spacing);
                entity.rect = entityRect;
            }

            contentSize = Vector2.Max(Vector2.zero, contentSize - spacing);

            var parentOffset = padding;
            var margin = Vector2.Max(Vector2.zero, containerSize - contentSize);

            if (margin.x > 0f)
            {
                switch (alignment)
                {
                    case TextAnchor.UpperCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.LowerCenter:
                        parentOffset.x += margin.x / 2f;
                        break;

                    case TextAnchor.UpperRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.LowerRight:
                        parentOffset.x += margin.x;
                        break;
                }
            }

            if (margin.y > 0f)
            {
                switch (alignment)
                {
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.MiddleRight:
                        parentOffset.y += margin.y / 2f;
                        break;

                    case TextAnchor.LowerLeft:
                    case TextAnchor.LowerCenter:
                    case TextAnchor.LowerRight:
                        parentOffset.y += margin.y;
                        break;
                }
            }

            if (container)
            {
                container.anchorMin = container.anchorMax = container.pivot = new Vector2(0f, 1f);
                container.sizeDelta = contentSize + margin + padding * 2f;
            }

            foreach (var e in entities)
            {
                var rect = e.rect.Value;
                rect.position += parentOffset;
                e.rect = rect;
            }

            layoutDirty = false;
        }

        private void LateUpdate()
        {
            if (IsDirty())
            {
                Refresh();
            }
        }

        private bool IsDirty()
        {
            if (layoutDirty)
            {
                return true;
            }

            if (!originalContainerSize.HasValue || (originalContainerSize.Value != RectTransform.rect.size))
            {
                return true;
            }

            if (!scrollOffset.HasValue || (scroller && scrollOffset.Value != scroller.normalizedPosition))
            {
                return true;
            }

            return false;
        }

        private void Reset()
        {
            scroller = GetComponent<ScrollRect>();

            if (scroller)
            {
                container = scroller.content;
            }

            if (container)
            {
                container.anchorMin = container.anchorMax = container.pivot = new Vector2(0f, 1f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (TryGetComponent<RectTransform>(out var rt) && viewRectSizeAddup.sqrMagnitude > 0f)
            {
                Gizmos.color = new Color(0.3f, 1f, 0.6f, 0.1f);
                var size = Vector2.Scale(transform.lossyScale, rt.rect.size + viewRectSizeAddup);
                Gizmos.DrawCube(transform.position, size);
                Gizmos.DrawWireCube(transform.position, size);
            }
        }
    }
}
