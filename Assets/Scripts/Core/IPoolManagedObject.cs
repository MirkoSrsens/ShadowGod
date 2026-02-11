namespace Razorhead.Core
{
    public interface IPoolManagedObject
    {
        void ReleasedToPool();

        void SpawnFromPool();
    }
}