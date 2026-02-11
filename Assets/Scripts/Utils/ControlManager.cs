using System;
using System.Collections.Generic;
using UnityEngine;

namespace Razorhead.Core
{
    /// <summary>
    /// Provides static methods for managing exclusive control relationships between controllers and target objects.
    /// </summary>
    /// <remarks>The ControlManager class enables a controller to take or release exclusive control over a
    /// target object. It is designed for scenarios where for example, one object can control position
    /// of some Gameobject transform.</remarks>
    public static class ControlManager
    {
        readonly static Dictionary<object, object> controlMap = new();
        readonly static object syncRoot = new();

        public static bool IsControlled(object target)
        {
            return IsControlled(target, out _);
        }

        public static bool IsControlled(object target, out object controller)
        {
            controller = default;

            if (target == null) return false;

            lock (syncRoot)
            {
                return controlMap.TryGetValue(target, out controller);
            }
        }

        public static ControlHandle TakeControl(object controller, object target)
        {
            if (target == null || controller == null)
            {
                return default;
            }

            lock (syncRoot)
            {
                if (controlMap.TryGetValue(target, out var currentController))
                {
                    Debug.LogWarning($"[ControlManager] Target ({target}) is already controlled by another object ({currentController}).");
                    return default;
                }

                controlMap[target] = controller;
                return new ControlHandle(controller, target);
            }
        }

        public static void Release(object controller, object target)
        {
            if (target == null || controller == null) return;

            lock (syncRoot)
            {
                if (controlMap.TryGetValue(target, out var currentController) && currentController == controller)
                {
                    controlMap.Remove(target);
                }
            }
        }
    }

    public readonly struct ControlHandle : IDisposable
    {
        readonly object controller;
        readonly object target;

        public ControlHandle(object controller, object target)
        {
            this.controller = controller;
            this.target = target;
        }

        public void Dispose()
        {
            ControlManager.Release(controller, target);
        }
    }
}
