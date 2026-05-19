using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Inventory.External;
using Assets.App.Game.Interactions.Runtime;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class DoorController
    {
        private static readonly int _opened = Animator.StringToHash("Opened");

        private readonly IAssetManager _assetManager;
        private readonly InventoryController _inventoryController;
        private DoorInteractiveView _doorView;
        private readonly IModuleItemsManager _moduleItemsManager;
        
        private Animator _animator;

        public DoorController(IAssetManager assetManager,
            IModuleItemsManager moduleItemsManager,
            InventoryController inventoryController, 
            DoorInteractiveView doorView)
        {
            _assetManager = assetManager;
            _moduleItemsManager = moduleItemsManager;
            _inventoryController = inventoryController;
            _doorView = doorView;
        }

        public void Initialize()
        {
            _animator = _doorView.GetComponent<Animator>();
            
            var interactableView = _doorView.gameObject.AddComponent<InteractableView>();
            interactableView.OnClickCallback += OnButtonClick;
            interactableView.OnHoverEnterCallback += OnButtonEnter;
            interactableView.OnHoverExitCallback += OnButtonExit;

            // if (_door.DoorModule.IsOpen)
            // {
            //     OpenDoor();
            // }
        }
        
        private void OnButtonEnter()
        {
        }

        private void OnButtonExit()
        {
        }

        private void OnButtonClick()
        {
            OpenDoor();
            // var doorModule = _door.DoorModule;
            // if (doorModule.IsOpen)
            // {
            //     return;
            // }
            //
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

        private void OpenDoor()
        {
            _animator.SetBool(_opened, true);

            var collider = _animator.GetComponentInChildren<Collider>();
            collider.enabled = false;
        }
    }
}