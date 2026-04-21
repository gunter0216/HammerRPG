using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Containers.ContainerWindow.Runtime;
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
            IFollowIconController followIconController)
        {
            _service = service;
            _dungeon = dungeon;
            _spriteLoader = spriteLoader;
            _containerWindow = containerWindow;
            _inventoryController = inventoryController;
            _moduleItemsManager = moduleItemsManager;
            _followIconController = followIconController;
        }

        public void Initialize()
        {
            _root = new GameObject($"Room {_service.Room.Data.UID.ToString()}");
            _root.transform.parent = _dungeon.transform;
            CreateFloors();
            CreateWalls();
            CreateDoors();
            CreateChest();
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
                    _spriteLoader,
                    chestRoot, 
                    chest,
                    _containerWindow,
                    _followIconController);
                chestController.Initialize();
                _chest.Add(chestController);
            }
        }

        private void CreateFloors()
        {
            var sprite = _spriteLoader.LoadItemSprite("floor");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }

            var room = _service.Room;

            foreach (var floor in room.Data.Floors)
            {
                var worldPosition = room.LocalToWorld(floor.Position);
                var positionX = worldPosition.X + floor.Width * 0.5f;
                var positionY = worldPosition.Y + floor.Height * 0.5f;
                CreateFloor(sprite.Value, positionX, positionY, floor.Width, floor.Height);
            }
        }

        private void CreateFloor(Sprite sprite, float positionX, float positionY, float width, float height)
        {
            var floor = new GameObject("Floor");
            floor.transform.position = new Vector3(positionX, positionY, 1);
            floor.transform.parent = _root.transform;
            var spriteRenderer = floor.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new UnityEngine.Vector2(width, height);
            spriteRenderer.sortingOrder = 0;
        }

        private void CreateWalls()
        {
            var room = _service.Room;
            var tiles = _service.Room.Tiles;
            var wallsRoot = new GameObject("Walls").transform;
            wallsRoot.parent = _root.transform;
            foreach (var tile in tiles)
            {
                var sprite = _spriteLoader.LoadItemSprite(tile.ModuleItem);
                if (!sprite.HasValue)
                {
                    continue;
                }
                
                var localPosition = tile.Data.Position;
                var position = room.LocalToWorld(localPosition);
                
                var tileView = new GameObject($"Tile {localPosition.X} {localPosition.Y}");
                tileView.transform.position = new Vector3(position.X + 0.5f, position.Y + 0.5f, 1);
                tileView.transform.parent = wallsRoot;
                
                var spriteRenderer = tileView.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite.Value;
                spriteRenderer.drawMode = SpriteDrawMode.Simple;
                spriteRenderer.size = new UnityEngine.Vector2(1, 1);
                spriteRenderer.sortingOrder = 1;

                tileView.AddComponent<BoxCollider2D>();
            }
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
                    _spriteLoader,
                    doorsRoot, 
                    door,
                    _inventoryController,
                    _moduleItemsManager);
                controller.Initialize();
                _doors.Add(controller);
            }
        }
    }
}