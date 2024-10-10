using System.Collections.Generic;

namespace App.Common.Utility.Runtime.Pool
{
    public interface IPool<T>
    {
        T Get();
        void Release(T item);
        void ReleaseAll();
        int Capacity { get; }
        IReadOnlyCollection<T> ActiveItems { get; }
    }
}