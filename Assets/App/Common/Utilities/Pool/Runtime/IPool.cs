using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Utilities.Pool.Runtime
{
    public interface IPool<T>
    {
        Optional<T> Get();
        bool Release(T item);
        int Capacity { get; }
    }
}