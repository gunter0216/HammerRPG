using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Dungeon.DungeonCreator.Runtime.Door;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class DoorController
    {
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly Transform _root;
        private readonly Door _door;

        public Door Door => _door;

        public DoorController(IItemSpriteLoader spriteLoader, Transform root, Door door)
        {
            _spriteLoader = spriteLoader;
            _root = root;
            _door = door;
        }

        public void Initialize()
        {
            var moduleItem = _door.ModuleItem;
            var configModule = moduleItem.GetConfigModule<DoorModuleConfig>();
            if (!configModule.HasValue)
            {
                HLogger.LogError($"Config not found.");
                return;
            }
                
            var isClosed = _door.Data.IsClosed;
            var iconKey = isClosed ? configModule.Value.CloseIconKey : configModule.Value.OpenIconKey;
            var sprite = _spriteLoader.Load(iconKey);
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }
                
            var localPosition = _door.Data.Position;
            var position = _door.Room.LocalToWorld(localPosition);
                
            var tileView = new GameObject($"Door {localPosition.X} {localPosition.Y}");
            tileView.transform.position = new Vector3(position.X + 0.5f, position.Y + 0.5f, 1);
            tileView.transform.parent = _root;
            
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