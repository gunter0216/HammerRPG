using UnityEngine;

namespace App.Game.GameManagers.External.View
{
    public interface ITileView
    {
        void SetSprite(Sprite sprite);
        void SetPosition(Vector3 position);
        void SetParent(Transform parent);
        void DisableCollider();
    }
}