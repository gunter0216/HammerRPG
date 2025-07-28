using App.Common.Algorithms.Runtime;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.Runtime
{
    public interface ITilesController
    {
        Optional<ITile> CreateTileByGenerationID(string generationID, Vector2Int position);
    }
}