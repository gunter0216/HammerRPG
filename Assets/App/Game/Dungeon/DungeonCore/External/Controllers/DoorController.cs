using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Doors;
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
        private readonly Transform _root;
        private readonly Door _door;
        private readonly InventoryController _inventoryController;
        private readonly IModuleItemsManager _moduleItemsManager;
        
        private Animator _animator;

        public DoorController(
            IAssetManager assetManager,
            Transform root,
            Door door,
            IModuleItemsManager moduleItemsManager, 
            InventoryController inventoryController)
        {
            _assetManager = assetManager;
            _root = root;
            _door = door;
            _moduleItemsManager = moduleItemsManager;
            _inventoryController = inventoryController;
        }

        public void Initialize()
        {
            var prefab = GetTilePrefab("door");
            if (prefab == null)
            {
                return;
            }
            
            var localPosition = _door.LocalPosition;
            var worldPosition = _door.Room.LocalToWorld(localPosition);
            var positionX = worldPosition.X + 0.5f;
            var positionZ = worldPosition.Y + 0.5f;

            var model = Object.Instantiate(prefab, _root.transform);
            model.transform.position = new Vector3(positionX, 1, positionZ);

            _animator = model.GetComponent<Animator>();
            
            var interactableView = model.AddComponent<InteractableView>();
            interactableView.OnClickCallback += OnButtonClick;
            interactableView.OnHoverEnterCallback += OnButtonEnter;
            interactableView.OnHoverExitCallback += OnButtonExit;

            if (_door.DoorModule.IsOpen)
            {
                OpenDoor();
            }
        }
        
        private GameObject GetTilePrefab(string item)
        {
            var config = _moduleItemsManager.GetConfig(item);
            if (!config.HasValue)
            {
                HLogger.LogError($"{item} not found.");
                return null;
            }
            
            if (!config.Value.TryGetModule<FbxModuleConfig>(out var fbxModuleConfig))
            {
                HLogger.LogError($"FbxModuleConfig not found.");
                return null;
            }
            
            var prefab = _assetManager.LoadSync<GameObject>(fbxModuleConfig.AssetKey);
            if (!prefab.HasValue)
            {
                HLogger.LogError($"Cant create view.");
                return null;
            }

            return prefab.Value;
        }

        private void OnButtonEnter()
        {
        }

        private void OnButtonExit()
        {
        }

        private void OnButtonClick()
        {
            var doorModule = _door.DoorModule;
            if (doorModule.IsOpen)
            {
                return;
            }
            
            if (_inventoryController.TryGetItem(_door.DoorModule.RequiredKey, out var inventoryItem))
            {
                Debug.LogError("Open");
                
                _door.DoorModule.Open();
                _inventoryController.Remove(inventoryItem);
                _moduleItemsManager.Destroy(inventoryItem.Item);

                OpenDoor();
            }
            else
            {
                Debug.LogError("Cant open");
            }
        }

        private void OpenDoor()
        {
            _animator.SetBool(_opened, true);

            var collider = _animator.GetComponentInChildren<Collider>();
            collider.enabled = false;
        }
    }
}