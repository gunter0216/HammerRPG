using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.DungeonCore.External.Services;
using UnityEngine;

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
            _root = new GameObject(); 
            CreateFloor();
        }
        
        public void CreateFloor()
        {
            var sprite = _spriteLoader.LoadItemSprite("floor");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }

            var room = _service.Room;
        
            var floor = new GameObject();
            var position = room.GetCenter();
            floor.transform.position = new Vector3(position.X, position.Y, 1);
            floor.transform.parent = _root.transform;
            var spriteRenderer = floor.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.Value;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new UnityEngine.Vector2(
                room.Width, 
                room.Height);
        }
    }
}