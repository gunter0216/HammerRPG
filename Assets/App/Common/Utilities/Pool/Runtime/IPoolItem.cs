using System;

namespace App.Common.Utilities.Pool.Runtime
{
    public interface IPoolItem
    {
        Action ReturnInPool { get; set; }
    }
}