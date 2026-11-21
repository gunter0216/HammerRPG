using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.Dungeon.DungeonCore.External.View
{
    public static class RoomPlacement
    {
        public static void Apply(Transform room, Vector2Int position, Vector2Int prefabOrigin, float clockwiseRotation)
        {
            var rotation = Quaternion.Euler(0f, 0f, -clockwiseRotation);
            // The generator rotates cell indices, while Unity rotates their corners around the prefab origin.
            var cellOffset = clockwiseRotation switch
            {
                90f => Vector3.up,
                180f => new Vector3(1f, 1f, 0f),
                270f => Vector3.right,
                _ => Vector3.zero
            };
            var localPosition = new Vector3(position.X, position.Y, 0f)
                - rotation * new Vector3(prefabOrigin.X, prefabOrigin.Y, 0f) + cellOffset;
            room.SetLocalPositionAndRotation(localPosition, rotation);
        }
    }
}
