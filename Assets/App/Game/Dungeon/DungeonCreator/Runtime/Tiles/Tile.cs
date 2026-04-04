using App.Game.GameTiles.Runtime;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Tiles
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