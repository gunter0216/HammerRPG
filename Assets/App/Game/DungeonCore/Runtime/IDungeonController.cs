using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.DungeonCore.External
{
    public interface IDungeonController
    {
        Optional<Vector2> GetSpawnPoint();
    }
}