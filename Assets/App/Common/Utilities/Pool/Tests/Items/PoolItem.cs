using System;
using App.Common.Utilities.Pool.Runtime;

namespace App.Common.Utilities.Pool.Tests.Items
{
    internal class PoolItem : IPoolItem
    {
        public Action ReturnInPool { get; set; }
    }
}