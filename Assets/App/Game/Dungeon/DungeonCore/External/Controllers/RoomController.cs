using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.AI.Runtime;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCore.External.View.Spawn;
using App.Game.Dungeon.DungeonCore.Runtime.Services;
using App.Game.FollowIcon.External;
using App.Game.Inventory.External;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class RoomController
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IContainerWindowController _containerWindow;
        private readonly RoomService _service;
        private readonly GameObject _dungeon;
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly InventoryController _inventoryController;
        private readonly IFollowIconController _followIconController;
        private readonly IAssetManager _assetManager;
        private readonly DungeonService _dungeonService;
        private readonly IAIController _aiController;

        private GameObject _root;
        private List<DoorController> _doors;
        private List<ChestController> _chests;
        private EnemySpawnController _enemySpawnController;
        private Transform _roomView;

        public RoomController(RoomService service,
            GameObject dungeon,
            IItemSpriteLoader spriteLoader,
            IContainerWindowController containerWindow,
            InventoryController inventoryController,
            IModuleItemsManager moduleItemsManager,
            IFollowIconController followIconController,
            IAssetManager assetManager,
            DungeonService dungeonService, 
            IAIController aiController)
        {
            _service = service;
            _dungeon = dungeon;
            _spriteLoader = spriteLoader;
            _containerWindow = containerWindow;
            _inventoryController = inventoryController;
            _moduleItemsManager = moduleItemsManager;
            _followIconController = followIconController;
            _assetManager = assetManager;
            _dungeonService = dungeonService;
            _aiController = aiController;
        }

        public void Initialize()
        {
            _root = new GameObject($"Room {_service.Room.Data.UID.ToString()}");
            _root.transform.SetParent(_dungeon.transform, false);
            
            _doors = new List<DoorController>();
            _chests = new List<ChestController>();
            _enemySpawnController = new EnemySpawnController(
                _moduleItemsManager, 
                _assetManager,
                _aiController);
            
            if (!CreateRoom())
                HLogger.LogError($"Failed to create room {_service.Room.Data.UID}.");
        }

        private bool CreateRoom()
        {
            var room = _service.Room;
            var assetReference = room.Variant.AssetReference;
            if (assetReference == null || !assetReference.RuntimeKeyIsValid())
                return false;

            var viewResult = _assetManager.InstantiateSync<Transform>(assetReference, _root.transform);
            if (!viewResult.HasValue)
                return false;

            _roomView = viewResult.Value;
            RoomPlacement.Apply(_roomView, room.Position, room.Config.PrefabOrigin, room.Data.Rotation);
            
            foreach (var transform in _roomView.GetComponentsInChildren<Transform>(includeInactive: false))
            {
                if (transform.GetComponent<IInteractiveView>() == null)
                {
                    continue;
                }

                if (transform.TryGetComponent<DoorInteractiveView>(out var doorView))
                {
                    CreateDoor(doorView);
                }
                
                if (transform.TryGetComponent<ChestInteractiveView>(out var chestView))
                {
                    CreateChest(chestView);
                }
            }

            return true;
        }

        private void CreateDoor(DoorInteractiveView doorView)
        {
            var controller = new DoorController(
                _assetManager,
                _moduleItemsManager,
                _inventoryController,
                doorView,
                _dungeonService,
                _service);
            controller.Initialize();
            _doors.Add(controller);
        }

        private void CreateChest(ChestInteractiveView chestView)
        {
            var controller = new ChestController(
                _containerWindow,
                _followIconController,
                _moduleItemsManager,
                chestView,
                _dungeonService,
                _service);
            controller.Initialize();
            _chests.Add(controller);
        }

        public void SpawnEnemies()
        {
            if (_roomView == null)
                return;

            _enemySpawnController.Initialize(_roomView);
            _enemySpawnController.SpawnEnemies();
        }
    }
}
