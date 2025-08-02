using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Data;

namespace App.Game.GameTiles.Runtime
{
    public interface ITilesController
    {
        ModuleItemResult<ITileModuleItem> CreateTileByGenerationID(string generationID, Vector2Int position);
        ModuleItemResult<ITileModuleItem> Create(string id);
        ModuleItemResult<ITileModuleItem> Create(DataReference dataReference);
        bool Destroy(ITileModuleItem data);
    }
}