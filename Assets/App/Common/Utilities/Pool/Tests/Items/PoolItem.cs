using System;
using App.Common.Utility.Pool.Runtime;

namespace App.Common.Utility.Pool.Tests.Items
{
    internal class PoolItem : IPoolItem
    {
        public Action ReturnInPool { get; set; }
    }
}