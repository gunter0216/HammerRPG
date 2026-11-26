using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCore.Runtime.Services;
using App.Game.Inventory.External;
using App.Game.Modules.Doors.Runtime;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class DoorController
    {
        private static readonly int _opened = Animator.StringToHash("Opened");

        private readonly IAssetManager _assetManager;
        private readonly InventoryController _inventoryController;
        private readonly DungeonService _dungeonService;
        private readonly RoomService _roomService;
        private readonly IModuleItemsManager _moduleItemsManager;
        
        private DoorInteractiveView _doorView;
        private DoorService _doorService;

        private Animator _animator;
        private DoorModule _door;

        public DoorController(IAssetManager assetManager,
            IModuleItemsManager moduleItemsManager,
            InventoryController inventoryController,
            DoorInteractiveView doorView,
            DungeonService dungeonService, 
            RoomService roomService)
        {
            _assetManager = assetManager;
            _moduleItemsManager = moduleItemsManager;
            _inventoryController = inventoryController;
            _doorView = doorView;
            _dungeonService = dungeonService;
            _roomService = roomService;
        }

        public void Initialize()
        {
            _animator = _doorView.GetComponent<Animator>();
            
            _doorView.OnClickCallback += OnButtonClick;
            _doorView.OnHoverEnterCallback += OnButtonEnter;
            _doorView.OnHoverExitCallback += OnButtonExit;

            _doorService = _roomService.CreateDoor();
            _door = _doorService.Module;
            ApplyDoorState();
        }
        
        private void OnButtonEnter()
        {
        }

        private void OnButtonExit()
        {
        }

        private void OnButtonClick()
        {
            if (_door.IsOpen)
                _door.Close();
            else if (_doorService.CanOpen())
                _door.Open();
            else
                return;

            ApplyDoorState();
        }

        private void ApplyDoorState()
        {
            bool isOpen = _door.IsOpen;
            if (_animator != null)
                _animator.SetBool(_opened, isOpen);

            if (_doorView.TryGetComponent<Collider2D>(out var collider2D))
            {
                collider2D.enabled = !isOpen;
                return;
            }

            var collider = _doorView.GetComponentInChildren<Collider>();
            if (collider != null)
                collider.enabled = !isOpen;
        }
    }
}
