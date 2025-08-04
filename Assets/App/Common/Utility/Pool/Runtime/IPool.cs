using System.Collections.Generic;
using App.Common.Utility.Runtime;

namespace App.Common.Utility.Pool.Runtime
{
    public interface IPool<T>
    {
        Optional<PoolItemHolder<T>> Get();
        bool Release(PoolItemHolder<T> item);
        void ReleaseAll();
        int Capacity { get; }
        IReadOnlyList<PoolItemHolder<T>> ActiveItems { get; }
    }
}