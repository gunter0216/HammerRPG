using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using App.Game.DungeonCreator.Runtime.Rooms;

namespace App.Generation.DungeonCreator.Runtime.Chest
{
    public class Chest
    {
        private readonly ChestData _doorData;
        private readonly IModuleItem _moduleItem;
        private readonly List<IModuleItem> _items;
        private readonly Room _room;

        public Chest(ChestData tileData, IModuleItem tileModuleItem, Room room, List<IModuleItem> items)
        {
            _doorData = tileData;
            _moduleItem = tileModuleItem;
            _room = room;
            _items = items;
        }

        public ChestData Data => _doorData;
        public IModuleItem ModuleItem => _moduleItem;
        public Room Room => _room;
        public List<IModuleItem> Items => _items;
    }
}