using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
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

        private GameObject _root;
        private List<DoorController> _doors;
        private List<ChestController> _chests;
        private EnemySpawnController _enemySpawnController;
        private Transform _roomView;

        public RoomController(
            RoomService service,
            GameObject dungeon,
            IItemSpriteLoader spriteLoader,
            IContainerWindowController containerWindow,
            InventoryController inventoryController,
            IModuleItemsManager moduleItemsManager,
            IFollowIconController followIconController,
            IAssetManager assetManager, 
            DungeonService dungeonService)
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
        }

        public void Initialize()
        {
            _root = new GameObject($"Room {_service.Room.Data.UID.ToString()}");
            _root.transform.parent = _dungeon.transform;
            
            _doors = new List<DoorController>();
            _chests = new List<ChestController>();
            _enemySpawnController = new EnemySpawnController(_moduleItemsManager, _assetManager);
            
            CreateRoom();
        }

        private void CreateRoom()
        {
            var room = _service.Room;
            var assetKey = room.Variant.AssetKey;
            var viewResult = _assetManager.InstantiateSync<Transform>(assetKey, _root.transform);
            _roomView = viewResult.Value;
            _roomView.transform.position = new Vector3(room.Position.X, 0, room.Position.Y);
            _roomView.transform.rotation = Quaternion.Euler(0, room.Data.Rotation, 0);
            _roomView.transform.parent = _root.transform;
            
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
            _enemySpawnController.Initialize(_roomView);
            _enemySpawnController.SpawnEnemies();
        }
    }
}