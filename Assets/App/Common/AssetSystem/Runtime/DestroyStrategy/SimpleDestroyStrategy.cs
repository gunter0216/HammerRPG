using UnityEngine;

namespace App.Common.AssetSystem.Runtime.DestroyStrategy
{
    public class SimpleDestroyStrategy : IDestroyStrategy
    {
        public void Destroy(Object item)
        {
            Object.Destroy(item);
        }
    }
}