using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;
using App.Game.Modules.Door.Runtime;
using App.Game.Modules.Door.Runtime.Data;
using App.Game.Modules.TilePosition.Runtime;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Doors
{
    public class Door
    {
        private readonly IModuleItem _doorModuleItem;
        private readonly Room _room;
        private readonly TilePositionModuleData _tilePosition;
        private readonly DoorModule _doorModule;

        public Door(IModuleItem tileModuleItem, Room room, TilePositionModuleData tilePosition, DoorModule doorModule)
        {
            _doorModuleItem = tileModuleItem;
            _room = room;
            _doorModule = doorModule;
            _tilePosition = tilePosition;
        }

        public IModuleItem ModuleItem => _doorModuleItem;
        public Room Room => _room;
        public DoorModule DoorModule => _doorModule;
        public DataReference RequiredKey => _doorModule.RequiredKey;
        public Vector2Int LocalPosition => new Vector2Int(_tilePosition.PositionX, _tilePosition.PositionY);
        public TilePositionModuleData TilePosition => _tilePosition;
    }
}