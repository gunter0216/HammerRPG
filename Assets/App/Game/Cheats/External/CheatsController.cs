using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Cheats.External.ViewModel;
using App.Game.Modules.Container.Runtime;
using App.Game.Modules.ContainerItem.Runtime.Config;
using App.Game.Player.External;

namespace App.Game.Cheats.External
{
    public class CheatsController : IInitSystem
    {
        private readonly ICanvasController _canvasController;
        private readonly IAssetManager _assetManager;
        private readonly IModuleItemsManager _itemsManager;
        private readonly PlayerController _playerController;

        private CheatsWindowModel _cheatsWindowModel;

        public CheatsController(ICanvasController canvasController, IAssetManager assetManager,
            IModuleItemsManager itemsManager, PlayerController playerController)
        {
            _canvasController = canvasController;
            _assetManager = assetManager;
            _itemsManager = itemsManager;
            _playerController = playerController;
        }

        public void Init()
        {
            var configs = _itemsManager.GetConfigs("Default");
            if (!configs.HasValue)
            {
                HLogger.LogError("Could not load the cheats item catalog.");
                return;
            }

            var items = new List<IModuleItemConfig>();
            foreach (var config in configs.Value)
                if (config.HasModule<ContainerItemModuleConfig>())
                    items.Add(config);
            items.Sort((left, right) => StringComparer.Ordinal.Compare(left.Id, right.Id));
            _cheatsWindowModel = new CheatsWindowModel(_assetManager, _canvasController, items, TryAddItem);
        }

        public bool TryAddItem(IModuleItemConfig config)
        {
            if (config == null || !config.HasModule<ContainerItemModuleConfig>() ||
                _playerController.Player == null ||
                !_playerController.Player.TryGetModule<ContainerModule>(out var container))
                return false;

            var item = _itemsManager.Create(config.Id);
            if (!item.HasValue)
                return false;

            if (container.AddItem(item.Value))
                return true;

            if (!_itemsManager.Destroy(item.Value))
                HLogger.LogError($"Could not clean up cheat item '{config.Id}'.");
            return false;
        }

        public bool IsOpen() => _cheatsWindowModel != null && _cheatsWindowModel.IsOpen();
        public void CloseWindow() => _cheatsWindowModel?.Close();
        public void OpenWindow() => _cheatsWindowModel?.Open();
    }
}
