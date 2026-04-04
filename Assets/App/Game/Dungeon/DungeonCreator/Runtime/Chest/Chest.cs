using App.Common.Algorithms.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;
using App.Game.Modules.Chest.Runtime;
using App.Game.Modules.ContainerModule.Runtime;
using App.Game.Modules.TilePosition.Runtime;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Chest
{
    public class Chest
    {
        private readonly IModuleItem _moduleItem;
        private readonly Room _room;
        private readonly TilePositionModuleData _tilePosition;
        private readonly ChestModule _chestModule;
        private readonly ContainerModule _containerModule;

        public Chest(IModuleItem tileModuleItem,
            Room room,
            TilePositionModuleData tilePosition,
            ChestModule chestModule,
            ContainerModule containerModule)
        {
            _moduleItem = tileModuleItem;
            _room = room;
            _tilePosition = tilePosition;
            _chestModule = chestModule;
            _containerModule = containerModule;
        }
        
        public IModuleItem ModuleItem => _moduleItem;
        public Room Room => _room;
        public ChestModule ChestModule => _chestModule;
        public ContainerModule ContainerModule => _containerModule;
        public Vector2Int LocalPosition => new Vector2Int(_tilePosition.PositionX, _tilePosition.PositionY);
    }
}