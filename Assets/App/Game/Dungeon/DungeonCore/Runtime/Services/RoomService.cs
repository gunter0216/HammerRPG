using System.Collections.Generic;
using App.Common.Logger.Runtime;
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
        private readonly Dictionary<string, IModuleItem> _objects = new();

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

        public bool TryResolveObject(string objectId, string configId, out IModuleItem item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(objectId))
                return false;

            if (_objects.TryGetValue(objectId, out item))
                return true;

            var hasSavedObject = _room.Data.TryGetObjectReference(objectId, out var reference);
            if (hasSavedObject && reference == null)
                return false;

            var result = hasSavedObject ? _moduleItemsManager.Create(reference) : _moduleItemsManager.Create(configId);
            if (!result.HasValue)
                return false;

            item = result.Value;
            if (!hasSavedObject && !_room.Data.TryAddObjectReference(objectId, item.ReferenceSelf))
            {
                if (!_moduleItemsManager.Destroy(item))
                    HLogger.LogError($"Could not discard module item '{objectId}' in room {_room.Data.UID}.");
                item = null;
                return false;
            }

            _objects.Add(objectId, item);
            return true;
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
