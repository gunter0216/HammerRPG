using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Dungeon.DungeonCore.Runtime
{
    public interface IDungeonController
    {
        Optional<Vector2> GetSpawnPoint();
    }
}