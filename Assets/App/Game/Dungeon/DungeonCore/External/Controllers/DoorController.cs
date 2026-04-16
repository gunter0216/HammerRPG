using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCreator.Runtime.Doors;
using App.Game.Inventory.External;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class DoorController
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly InventoryController _inventoryController;
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly Transform _root;
        private readonly Door _door;
        
        private SpriteInteractionHandler _spriteInteractionHandler;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;

        public Door Door => _door;

        public DoorController(
            IItemSpriteLoader spriteLoader, 
            Transform root, 
            Door door, 
            InventoryController inventoryController, 
            IModuleItemsManager moduleItemsManager)
        {
            _spriteLoader = spriteLoader;
            _root = root;
            _door = door;
            _inventoryController = inventoryController;
            _moduleItemsManager = moduleItemsManager;
        }

        public void Initialize()
        {
            var doorModule = _door.DoorModule;
            var sprite = _spriteLoader.Load(doorModule.IconKey);
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }
                
            var localPosition = _door.LocalPosition;
            var position = _door.Room.LocalToWorld(localPosition);
                
            var tileView = new GameObject($"Door {localPosition.X} {localPosition.Y}");
            tileView.transform.position = new Vector3(position.X + 0.5f, position.Y + 0.5f, 1);
            tileView.transform.parent = _root;
            
            _spriteRenderer = tileView.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite = sprite.Value;
            _spriteRenderer.drawMode = SpriteDrawMode.Simple;
            _spriteRenderer.size = new UnityEngine.Vector2(1, 1);
            _spriteRenderer.sortingOrder = 3;
            
            _collider = tileView.AddComponent<BoxCollider2D>();
            _collider.enabled = doorModule.IsClosed;
            
            _spriteInteractionHandler = tileView.AddComponent<SpriteInteractionHandler>();
            _spriteInteractionHandler.SetClickListener(OnButtonClick);
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
                
                var sprite = _spriteLoader.Load(doorModule.IconKey);
                if (!sprite.HasValue)
                {
                    HLogger.LogError("Cant get tile sprite");
                    return;
                }

                _spriteRenderer.sprite = sprite.Value;
                _collider.enabled = false;
            }
            else
            {
                Debug.LogError("Cant open");
            }
        }
    }
}