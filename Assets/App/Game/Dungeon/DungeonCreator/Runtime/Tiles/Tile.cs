using App.Common.ModuleItem.Runtime;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Tiles
{
    public class Tile
    {
        private readonly TileData _tileData;
        private readonly IModuleItem _moduleItem;

        public Tile(TileData tileData, IModuleItem tileModuleItem)
        {
            _tileData = tileData;
            _moduleItem = tileModuleItem;
        }

        public TileData Data => _tileData;
        public IModuleItem ModuleItem => _moduleItem;
    }
}