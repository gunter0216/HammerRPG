using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCreator.Runtime.Chest;
using App.Game.FollowIcon.External;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class ChestController
    {
        private const string _iconChest = "FollowIcon_Chest";
        private const string _iconEmptyChest = "FollowIcon_EmptyChest";

        private readonly IAssetManager _assetManager;
        private readonly Transform _root;
        private readonly DungeonCreator.Runtime.Chest.Chest _chest;
        private readonly IContainerWindowController _containerWindow;
        private readonly IFollowIconController _followIconController;
        private readonly IModuleItemsManager _moduleItemsManager;

        public ChestController(
            IAssetManager assetManager,
            Transform root,
            Chest chest,
            IContainerWindowController containerWindow,
            IFollowIconController followIconController,
            IModuleItemsManager moduleItemsManager)
        {
            _assetManager = assetManager;
            _root = root;
            _chest = chest;
            _containerWindow = containerWindow;
            _followIconController = followIconController;
            _moduleItemsManager = moduleItemsManager;
        }

        public void Initialize()
        {
            var prefab = GetTilePrefab("chest");
            if (prefab == null)
            {
                return;
            }
            
            var localPosition = _chest.LocalPosition;
            var worldPosition = _chest.Room.LocalToWorld(localPosition);
            var positionX = worldPosition.X + 0.5f;
            var positionZ = worldPosition.Y + 0.5f;

            var model = Object.Instantiate(prefab, _root.transform);
            model.transform.position = new Vector3(positionX, 1, positionZ);
            
            // chestView.SetClickListener(OnButtonClick);
            // chestView.SetEnterListener(OnButtonEnter);
            // chestView.SetExitListener(OnButtonExit);
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