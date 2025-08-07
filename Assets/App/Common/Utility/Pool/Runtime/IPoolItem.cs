using System;

namespace App.Common.Utility.Pool.Runtime
{
    public interface IPoolItem
    {
        Action ReturnInPool { get; set; }
    }
}