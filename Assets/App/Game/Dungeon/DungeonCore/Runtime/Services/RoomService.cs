using App.Common.ModuleItem.Runtime;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCore.Runtime.Services
{
    public class RoomService
    {
        private readonly IContainerWindowController _containerWindow;
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly Room _room;

        public Room Room => _room;

        public RoomService(IModuleItemsManager moduleItemsManager, IContainerWindowController containerWindow, Room room)
        {
            _moduleItemsManager = moduleItemsManager;
            _room = room;
            _containerWindow = containerWindow;
        }

        public void Initialize()
        {
            
        }
        
        public DoorService CreateDoor()
        {
            var service = new DoorService(_moduleItemsManager);
            service.Initialize();
            return service;
        }
        
        public ChestService CreateChest()
        {
            var service = new ChestService(_moduleItemsManager, _containerWindow);
            service.Initialize();
            return service;
        }
    }
}