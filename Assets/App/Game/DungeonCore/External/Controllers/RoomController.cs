using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.DungeonCore.External.Services;
using App.Game.DungeonCreator.Runtime.Door;
using UnityEngine;
using Vector2 = App.Common.Algorithms.Runtime.Vector2;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.DungeonCore.External.Controllers
{
    public class RoomController
    {
        private readonly RoomService _service;
        private readonly IItemSpriteLoader _spriteLoader;
        
        private GameObject _root;

        public RoomController(RoomService service, IItemSpriteLoader spriteLoader)
        {
            _service = service;
            _spriteLoader = spriteLoader;
        }

        public void Initialize()
        {
            _root = new GameObject($"Room {_service.Room.Data.UID.ToString()}"); 
            CreateFloor();
            CreateWalls();
            CreateDoors();
        }

        private void CreateFloor()
        {
            var sprite = _spriteLoader.LoadItemSprite("floor");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }

            var room = _service.Room;
        
            var position = room.GetCenter();

            var floor = new GameObject("Floor");
            floor.transform.position = new Vector3(position.X, position.Y, 1);
            floor.transform.parent = _root.transform;
            var spriteRenderer = floor.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.Value;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new UnityEngine.Vector2(room.Width, room.Height);
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
                    HLogger.LogError("Cant get tile sprite");
                    return;
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
            var room = _service.Room;
            var doors = _service.Room.Doors;
            var doorsRoot = new GameObject("Doors").transform;
            doorsRoot.parent = _root.transform;
            foreach (var door in doors)
            {
                var moduleItem = door.ModuleItem;
                var configModule = moduleItem.GetConfigModule<DoorModuleConfig>();
                if (!configModule.HasValue)
                {
                    HLogger.LogError($"Config not found.");
                    continue;
                }
                
                var isClosed = door.Data.IsClosed;
                var iconKey = isClosed ? configModule.Value.CloseIconKey : configModule.Value.OpenIconKey;
                var sprite = _spriteLoader.Load(iconKey);
                if (!sprite.HasValue)
                {
                    HLogger.LogError("Cant get tile sprite");
                    continue;
                }
                
                var localPosition = door.Data.Position;
                var position = room.LocalToWorld(localPosition);
                
                var tileView = new GameObject($"Door {localPosition.X} {localPosition.Y}");
                tileView.transform.position = new Vector3(position.X + 0.5f, position.Y + 0.5f, 1);
                tileView.transform.parent = doorsRoot;
                
                var spriteRenderer = tileView.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite.Value;
                spriteRenderer.drawMode = SpriteDrawMode.Simple;
                spriteRenderer.size = new UnityEngine.Vector2(1, 1);
                spriteRenderer.sortingOrder = 3;

                var collider = tileView.AddComponent<BoxCollider2D>();
                collider.enabled = isClosed;
            }
        }
    }
}