using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.Runtime.Services;
using App.Game.FollowIcon.External;
using App.Game.Inventory.External;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
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
        
        private GameObject _root;
        private List<DoorController> _doors;
        private List<ChestController> _chest;

        public RoomController(
            RoomService service,
            GameObject dungeon,
            IItemSpriteLoader spriteLoader,
            IContainerWindowController containerWindow,
            InventoryController inventoryController,
            IModuleItemsManager moduleItemsManager,
            IFollowIconController followIconController, 
            IAssetManager assetManager)
        {
            _service = service;
            _dungeon = dungeon;
            _spriteLoader = spriteLoader;
            _containerWindow = containerWindow;
            _inventoryController = inventoryController;
            _moduleItemsManager = moduleItemsManager;
            _followIconController = followIconController;
            _assetManager = assetManager;
        }

        public void Initialize()
        {
            _root = new GameObject($"Room {_service.Room.Data.UID.ToString()}");
            _root.transform.parent = _dungeon.transform;
            CreateRoom();
            // CreateFloors();
            // CreateWalls();
            // CreateDoors();
            // CreateChest();
        }

        private void CreateRoom()
        {
            var room = _service.Room;
            var assetKey = room.Variant.AssetKey;
            var viewResult = _assetManager.InstantiateSync<Transform>(assetKey, _root.transform);
            var view = viewResult.Value;
            view.transform.position = new Vector3(room.Position.X, 0, room.Position.Y);
            view.transform.rotation = Quaternion.Euler(0, room.Data.Rotation, 0);
            view.transform.parent = _root.transform;
        }

        private void CreateChest()
        {
            _chest = new List<ChestController>();
            var chestRoot = new GameObject("Chests").transform;
            chestRoot.parent = _root.transform;
            
            var room = _service.Room;
            var chests = room.Chests;
            foreach (var chest in chests)
            {
                var chestController = new ChestController(
                    _assetManager,
                    chestRoot, 
                    chest,
                    _containerWindow,
                    _followIconController,
                    _moduleItemsManager);
                chestController.Initialize();
                _chest.Add(chestController);
            }
        }

        private void CreateFloors()
        {
            var prefab = GetTilePrefab("floor");
            if (prefab == null)
            {
                return;
            }
            
            var root = new GameObject("Floors").transform;
            root.parent = _root.transform;
        }

        private void CreateWalls()
        {
            var prefab = GetTilePrefab("wall");
            if (prefab == null)
            {
                return;
            }
            
            var room = _service.Room;
            var tiles = _service.Room.Tiles;
            
            var root = new GameObject("Walls").transform;
            root.parent = _root.transform;
            
            foreach (var tile in tiles)
            {
                var model = Object.Instantiate(prefab, root.transform);
                
                var localPosition = tile.Data.Position;
                var worldPosition = room.LocalToWorld(localPosition);
                var positionX = worldPosition.X + 0.5f;
                var positionZ = worldPosition.Y + 0.5f;
                
                model.transform.position = new Vector3(positionX, 1, positionZ);
                model.transform.localScale = new Vector3(1, 1, 1);
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

        private void CreateDoors()
        {
            var doors = _service.Room.Doors;
            var doorsRoot = new GameObject("Doors").transform;
            doorsRoot.parent = _root.transform;
            _doors = new List<DoorController>(doors.Count);
            foreach (var door in doors)
            {
                var controller = new DoorController(
                    _assetManager,
                    doorsRoot, 
                    door,
                    _moduleItemsManager,
                    _inventoryController);
                controller.Initialize();
                _doors.Add(controller);
            }
        }
    }
}