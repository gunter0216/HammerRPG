using UnityEngine;

namespace App.Game.GameManagers.External.View
{
    public interface IRoomView
    {
        Transform Content { get; }
        void SetParent(Transform parent);
    }
}