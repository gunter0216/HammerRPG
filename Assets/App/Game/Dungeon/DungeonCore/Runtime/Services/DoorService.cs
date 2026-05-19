using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Doors.Runtime;

namespace App.Game.Dungeon.DungeonCore.Runtime.Services
{
    public class DoorService
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private IModuleItem _item;
        private DoorModule _doorModule;

        public DoorModule Module => _doorModule;

        public DoorService(IModuleItemsManager moduleItemsManager)
        {
            _moduleItemsManager = moduleItemsManager;
        }

        public void Initialize()
        {
            var item = _moduleItemsManager.Create("door");
            _item = item.Value;
            _doorModule = _item.GetModule<DoorModule>().Value;
        }

        public bool CanOpen()
        {
            return true;
            // if (_inventoryController.TryGetItem(_door.DoorModule.RequiredKey, out var inventoryItem))
            // {
            //     Debug.LogError("Open");
            //     
            //     _door.DoorModule.Open();
            //     _inventoryController.Remove(inventoryItem);
            //     _moduleItemsManager.Destroy(inventoryItem.Item);
            //
            //     OpenDoor();
            // }
            // else
            // {
            //     Debug.LogError("Cant open");
            // }
        }
    }
}