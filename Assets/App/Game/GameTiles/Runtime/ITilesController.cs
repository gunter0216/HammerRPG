using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.Algorithms.Runtime;
using Assets.App.Common.ModuleItem.Runtime;

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