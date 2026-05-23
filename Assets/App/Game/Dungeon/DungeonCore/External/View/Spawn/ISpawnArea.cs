using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.View.Spawn
{
    public interface ISpawnArea
    {
        float Weight { get; }

        bool TryGetSpawnPoint(out Vector3 point);
    }
}