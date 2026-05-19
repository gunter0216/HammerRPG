using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCore.Runtime.Services;
using App.Game.Inventory.External;
using App.Game.Modules.Doors.Runtime;
using Assets.App.Game.Interactions.Runtime;
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
            
            var interactableView = _doorView.gameObject.AddComponent<InteractableView>();
            interactableView.OnClickCallback += OnButtonClick;
            interactableView.OnHoverEnterCallback += OnButtonEnter;
            interactableView.OnHoverExitCallback += OnButtonExit;

            _doorService = _roomService.CreateDoor();
            _door = _doorService.Module;
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
            {
                return;
            }

            if (!_doorService.CanOpen())
            {
                return;
            }
            
            _door.Open();
            OpenDoor();
        }

        private void OpenDoor()
        {
            _animator.SetBool(_opened, true);

            var collider = _animator.GetComponentInChildren<Collider>();
            collider.enabled = false;
        }
    }
}