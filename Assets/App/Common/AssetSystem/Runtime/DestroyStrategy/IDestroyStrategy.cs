using UnityEngine;

namespace App.Common.AssetSystem.Runtime.DestroyStrategy
{
    public interface IDestroyStrategy
    {
        void Destroy(Object item);
    }
}