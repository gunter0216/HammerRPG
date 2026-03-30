using System;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.DungeonCreator.Runtime.Door;
using App.Game.DungeonCreator.Runtime.Rooms;
using App.Generation.DungeonCreator.Runtime.Chest;
using UnityEngine;

namespace App.Game.DungeonCore.External.Controllers
{
    public class ChestController
    {
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly Transform _root;
        private readonly Chest _chest;


        public ChestController(IItemSpriteLoader spriteLoader, Transform root, Chest chest)
        {
            _spriteLoader = spriteLoader;
            _root = root;
            _chest = chest;
        }

        public void Initialize()
        {
            var moduleItem = _chest.ModuleItem;
            var configModule = moduleItem.GetConfigModule<ChestModuleConfig>();
            if (!configModule.HasValue)
            {
                HLogger.LogError($"Config not found.");
                return;
            }
                
            string iconKey = String.Empty;
            var isClosed = _chest.Data.State == ChestStateConstants.Closed;
            if (isClosed)
            {
                iconKey = configModule.Value.CloseIconKey;
            }
            else if (_chest.Data.State == ChestStateConstants.Open)
            {
                iconKey = configModule.Value.OpenIconKey;
            }
            else if (_chest.Data.State == ChestStateConstants.Empty)
            {
                iconKey = configModule.Value.EmptyIconKey;
            }
            
            var sprite = _spriteLoader.Load(iconKey);
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }
                
            var localPosition = _chest.Data.Position;
            var position = _chest.Room.LocalToWorld(localPosition);
            
            var tileView = new GameObject($"Chest {localPosition.X} {localPosition.Y}");
            tileView.transform.position = new Vector3(position.X + 0.5f, position.Y + 0.5f, 1);
            tileView.transform.parent = _root;
            
            var spriteRenderer = tileView.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.Value;
            spriteRenderer.drawMode = SpriteDrawMode.Simple;
            spriteRenderer.sortingOrder = 3;

            var collider = tileView.AddComponent<BoxCollider2D>();
        }
    }
}