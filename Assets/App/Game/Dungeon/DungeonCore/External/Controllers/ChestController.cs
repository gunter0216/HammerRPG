using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class ChestController
    {
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly Transform _root;
        private readonly DungeonCreator.Runtime.Chest.Chest _chest;
        private readonly IContainerWindowController _containerWindow;

        public ChestController(
            IItemSpriteLoader spriteLoader, 
            Transform root, 
            DungeonCreator.Runtime.Chest.Chest chest, 
            IContainerWindowController containerWindow)
        {
            _spriteLoader = spriteLoader;
            _root = root;
            _chest = chest;
            _containerWindow = containerWindow;
        }

        public void Initialize()
        {
            var chestModule = _chest.ChestModule;
                
            var iconKey = chestModule.IconKey;
            var sprite = _spriteLoader.Load(iconKey);
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }
                
            var localPosition = _chest.LocalPosition;
            var position = _chest.Room.LocalToWorld(localPosition);
            
            var tileView = new GameObject($"Chest {localPosition.X} {localPosition.Y}");
            tileView.transform.position = new Vector3(position.X + 0.5f, position.Y + 0.5f, 1);
            tileView.transform.parent = _root;
            
            var spriteRenderer = tileView.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.Value;
            spriteRenderer.drawMode = SpriteDrawMode.Simple;
            spriteRenderer.sortingOrder = 3;
            
            var collider = tileView.AddComponent<BoxCollider2D>();
            
            var chestView = tileView.AddComponent<SpriteInteractionHandler>();
            chestView.AddClickListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _containerWindow.OpenWindow(_chest.ContainerModule.Container);
        }
    }
}