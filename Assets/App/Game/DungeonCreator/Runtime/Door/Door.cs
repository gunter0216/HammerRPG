using App.Common.ModuleItem.Runtime;
using App.Game.DungeonCreator.Runtime.Door;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class Door
    {
        private readonly DoorData _doorData;
        private readonly IModuleItem _doorModuleItem;

        public Door(DoorData tileData, IModuleItem tileModuleItem)
        {
            _doorData = tileData;
            _doorModuleItem = tileModuleItem;
        }

        public DoorData Data => _doorData;
        public IModuleItem ModuleItem => _doorModuleItem;
    }
}