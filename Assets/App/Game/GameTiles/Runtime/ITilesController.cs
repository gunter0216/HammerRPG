using App.Common.Algorithms.Runtime;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.Runtime
{
    public interface ITilesController
    {
        Optional<ITileModuleItem> CreateTileByGenerationID(string generationID, Vector2Int position);
        Optional<ITileModuleItem> Create(string id);
        Optional<ITileModuleItem> Create(IModuleItemData data);
        bool Destroy(ITileModuleItem data);
    }
}