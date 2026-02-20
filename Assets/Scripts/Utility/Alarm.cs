using System;
using System.Collections.Generic;
using UnityEngine.Pool;

public static class Alarm<T>
{
    public static List<Action<T>> callbacks = new();
    public static Dictionary<object, Action<T>> callbacksWithSource = new();

    public static void Subscribe(Action<T> action)
    {
        callbacks.Add(action);
    }

    public static void Unsubscribe(Action<T> action)
    {
        callbacks.Remove(action);
    }

    public static void Subscribe(object owner, Action<T> action)
    {
        callbacksWithSource.Add(owner, action);
    }

    public static void Unsubscribe(object owner)
    {
        callbacksWithSource.Remove(owner);
    }

    internal static void Raise()
    {
        Raise(default(T));
    }

    public static void Raise(T data)
    {
        using var _ = ListPool<Action<T>>.Get(out var temp);
        temp.AddRange(callbacks);

        foreach (var item in temp)
        {
            item(data);
        }

        foreach (var item in callbacksWithSource)
        {
            item.Value(data);
        }
    }
}
