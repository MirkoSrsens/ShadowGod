using UnityEngine;

namespace Razorhead.Core
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent(out T result))
            {
                return result;
            }

            return gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if (component.TryGetComponent(out T result))
            {
                return result;
            }

            return component.gameObject.AddComponent<T>();
        }

        public static bool TryGetComponentInChildren<T>(this GameObject t, out T component) where T : Component
        {
            component = null;
            if (!t) return false;
            component = t.GetComponentInChildren<T>();
            return component;
        }

        public static bool TryGetComponentInChildren<T>(this Component t, out T component) where T : Component
        {
            component = null;
            if (!t) return false;
            component = t.GetComponentInChildren<T>();
            return component;
        }

        public static bool TryGetNextSibling<T>(this T t, out T sibling) where T : Component
        {
            sibling = null;
            if (t == null) return false;
            var parent = t.transform.parent;
            if (parent == null) return false;

            int index = t.transform.GetSiblingIndex();
            int next = index + 1;
            return next < parent.childCount && parent.GetChild(next).TryGetComponent<T>(out sibling);
        }

        public static bool TryGetPreviousSibling<T>(this T t, out T sibling) where T : Component
        {
            sibling = null;
            if (t == null) return false;
            var parent = t.transform.parent;
            if (parent == null) return false;

            int index = t.transform.GetSiblingIndex();
            int prev = index - 1;
            return prev >= 0 && parent.GetChild(prev).TryGetComponent<T>(out sibling);
        }

        public static void MatchWorldRect(this RectTransform target, RectTransform source)
        {
            if (!target || !source) return;

            var parent = target.parent as RectTransform;

            if (!parent) return;

            target.pivot = source.pivot;
            target.sizeDelta = source.sizeDelta;

            target.position = source.position;
        }
    }
}
