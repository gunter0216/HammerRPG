using System.Collections.Generic;

namespace App.Common.Utility.Runtime.Pool
{
    public interface IListPool<T> : IPool<T>
    {
        IReadOnlyList<T> ActiveItems { get; }
    }
}