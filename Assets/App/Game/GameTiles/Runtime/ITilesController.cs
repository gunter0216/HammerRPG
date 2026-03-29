using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;

namespace App.Game.GameTiles.Runtime
{
    public interface ITilesController
    {
        Optional<ITileModuleItem> CreateTileByGenerationID(DungeonTile generationID, Vector2Int position);
        Optional<ITileModuleItem> Create(string id);
        Optional<ITileModuleItem> Create(DataReference dataReference);
        bool Destroy(ITileModuleItem data);
    }
}