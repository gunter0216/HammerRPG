using App.Common.Utilities.Utility.Runtime;
using App.Game.GameTiles.Runtime;
using App.Generation.DungeonCreator.Runtime.Tiles;

namespace App.Game.DungeonCreator.Runtime.Tiles
{
    public class Tile
    {
        private readonly TileData m_TileData;
        private readonly ITileModuleItem m_TileModuleItem;

        public Tile(TileData tileData, ITileModuleItem tileModuleItem)
        {
            m_TileData = tileData;
            m_TileModuleItem = tileModuleItem;
        }

        public TileData Data => m_TileData;

        public ITileModuleItem ModuleItem => m_TileModuleItem;
    }
}