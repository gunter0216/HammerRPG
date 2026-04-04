using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Door
{
    public class Door
    {
        private readonly DoorData _doorData;
        private readonly IModuleItem _doorModuleItem;
        private readonly Room _room;

        public Door(DoorData tileData, IModuleItem tileModuleItem, Room room)
        {
            _doorData = tileData;
            _doorModuleItem = tileModuleItem;
            _room = room;
        }

        public DoorData Data => _doorData;
        public IModuleItem ModuleItem => _doorModuleItem;

        public Room Room => _room;
    }
}