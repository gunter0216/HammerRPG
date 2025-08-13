using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.GameTiles.Runtime
{
    public interface ITilesController
    {
        Optional<ITileModuleItem> CreateTileByGenerationID(string generationID, Vector2Int position);
        Optional<ITileModuleItem> Create(string id);
        Optional<ITileModuleItem> Create(DataReference dataReference);
        bool Destroy(ITileModuleItem data);
    }
}