using App.Common.Utility.Runtime;

namespace App.Common.Utility.Pool.Runtime
{
    public interface IPool<T>
    {
        Optional<T> Get();
        bool Release(T item);
        int Capacity { get; }
    }
}