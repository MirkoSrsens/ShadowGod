using UnityEngine.Pool;

namespace Razorhead.Core
{
    public static class PoolGeneric<T> where T : class, new()
    {
        public static T Get()
        {
            return GenericPool<T>.Get();
        }

        public static PooledObject<T> Get(out T value)
        {
            return GenericPool<T>.Get(out value);
        }

        public static void Release(T toRelease)
        {
            if (toRelease is IClearable clearable) clearable.Clear();
            GenericPool<T>.Release(toRelease);
        }
    }

    public interface IClearable
    {
        void Clear();
    }

}
