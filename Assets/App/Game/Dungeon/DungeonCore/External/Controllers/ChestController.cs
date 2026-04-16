using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.FollowIcon.External;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class ChestController
    {
        private const string _iconChest = "FollowIcon_Chest";
        private const string _iconEmptyChest = "FollowIcon_EmptyChest";

        private readonly IItemSpriteLoader _spriteLoader;
        private readonly Transform _root;
        private readonly DungeonCreator.Runtime.Chest.Chest _chest;
        private readonly IContainerWindowController _containerWindow;
        private readonly IFollowIconController _followIconController;

        public ChestController(
            IItemSpriteLoader spriteLoader, 
            Transform root, 
            DungeonCreator.Runtime.Chest.Chest chest, 
            IContainerWindowController containerWindow, 
            IFollowIconController followIconController)
        {
            _spriteLoader = spriteLoader;
            _root = root;
            _chest = chest;
            _containerWindow = containerWindow;
            _followIconController = followIconController;
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
            chestView.SetClickListener(OnButtonClick);
            chestView.SetEnterListener(OnButtonEnter);
            chestView.SetExitListener(OnButtonExit);
        }

        private void OnButtonEnter()
        {
            var icon = _chest.ChestModule.IsUsed ? _iconEmptyChest : _iconChest;
            _followIconController.Show(this, icon);
        }

        private void OnButtonExit()
        {
            _followIconController.Hide(this);
        }

        private void OnButtonClick()
        {
            var module = _chest.ChestModule;
            if (module.IsClosed)
            {
                HLogger.LogError("Chest is closed.");
                return;
            }
            
            _containerWindow.OpenWindow(_chest.ContainerModule.Container);
            _chest.ChestModule.SetUsedState();
        }
    }
}