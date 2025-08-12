using UnityEngine;

namespace App.Game.GameManagers.External.Room
{
    public interface IRoomView
    {
        Transform Content { get; }
        void SetParent(Transform parent);
    }
}