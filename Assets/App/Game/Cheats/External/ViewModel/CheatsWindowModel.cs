using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.Cheats.External.Services;
using App.Game.Cheats.External.View;
using App.Game.Modules.ContainerItem.Runtime.Config;
using App.Game.Modules.Sprite.Runtime;
using UnityEngine;

namespace App.Game.Cheats.External.ViewModel
{
    public class CheatsWindowModel
    {
        private readonly IAssetManager _assetManager;
        private readonly ICanvasController _canvasController;
        private readonly IReadOnlyList<IModuleItemConfig> _items;
        private readonly Func<IModuleItemConfig, bool> _addItem;

        private CheatsWindow _window;

        public CheatsWindowModel(IAssetManager assetManager, ICanvasController canvasController,
            IReadOnlyList<IModuleItemConfig> items, Func<IModuleItemConfig, bool> addItem)
        {
            _assetManager = assetManager;
            _canvasController = canvasController;
            _items = items;
            _addItem = addItem;
        }

        public void Open()
        {
            if (_window == null && !CreateWindow())
            {
                HLogger.LogError("Failed to create cheats window.");
                return;
            }

            _window.transform.SetAsLastSibling();
            _window.SetActive(true);
        }

        public void Close()
        {
            if (_window != null)
                _window.SetActive(false);
        }

        public bool IsOpen() => _window != null && _window.IsActive();

        private bool CreateWindow()
        {
            var creator = new CheatsWindowCreator(_assetManager, _canvasController);
            var window = creator.Create();
            if (!window.HasValue)
                return false;

            _window = window.Value;
            foreach (var config in _items)
            {
                if (!config.TryGetModule<ContainerItemModuleConfig>(out var size))
                    continue;
                var view = UnityEngine.Object.Instantiate(_window.CheatsSlotViewPrefab, _window.SlotsContent);
                view.name = "CheatItem_" + config.Id;
                view.SetLabel($"{config.Id}\n{size.Width} × {size.Height}");
                view.SetSprite(config.TryGetModule<SpriteModuleConfig>(out var icon) ? icon.Sprite : null);
                view.SetButtonClickCallback(() =>
                {
                    if (!_addItem(config))
                        Debug.LogWarning($"Could not add '{config.Id}' to the player's container.");
                });
                view.SetActive(true);
            }
            return true;
        }
    }
}
