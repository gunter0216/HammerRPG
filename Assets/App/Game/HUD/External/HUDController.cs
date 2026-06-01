using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Game.CharacterWindow.External;
using App.Game.GameMenu.External.Fabric;
using App.Game.GameMenu.External.View;
using App.Game.Inventory.External;
using App.Game.Inventory.Runtime;
using App.Game.Modules.Health.Runtime;
using App.Game.Player.External;
using UniRx;
using UnityEngine;

namespace App.Game.GameMenu.External
{
    public class HUDController : IInitSystem, IDisposable
    {
        private readonly ICanvasController _canvasController;
        private readonly IAssetManager _assetManager;
        private readonly PlayerController _playerController;
        private readonly HealthModuleSystem _healthModuleSystem;
        private readonly CharacterWindowController _characterWindowController;
        private readonly IInventoryController _inventoryController;
        
        private HUDView _view;
        private IModuleItem _player;
        private HealthModule _healthModule;

        private CompositeDisposable _disposable;

        public HUDController(
            ICanvasController canvasController, 
            IAssetManager assetManager, 
            PlayerController playerController, 
            HealthModuleSystem healthModuleSystem, 
            CharacterWindowController characterWindowController, 
            IInventoryController inventoryController)
        {
            _canvasController = canvasController;
            _assetManager = assetManager;
            _playerController = playerController;
            _healthModuleSystem = healthModuleSystem;
            _characterWindowController = characterWindowController;
            _inventoryController = inventoryController;
        }

        public void Init()
        {
            _disposable = new CompositeDisposable();
            CreateView();

            _player = _playerController.Player;
            if (!_player.TryGetModule(out _healthModule))
            {
                HLogger.LogError($"Health module not found.");
                return;
            }
            
            _healthModule.OnHealthChanged += OnHealthChanged;
            _disposable.Add(Disposable.Create(() =>
            {
                _healthModule.OnHealthChanged -= OnHealthChanged;
            }));
            
            UpdateHealth();
            _view.SetInventoryButtonClickCallback(OnInventoryClick);
            _view.SetCharacterButtonClickCallback(OnCharacterClick);
            _view.SetQuestsButtonClickCallback(OnQuestsClick);
            _view.SetSettingsButtonClickCallback(OnSettingsClick);
        }

        private void OnSettingsClick()
        {
            
        }

        private void OnQuestsClick()
        {
            
        }

        private void OnCharacterClick()
        {
            _characterWindowController.Open();
        }

        private void OnInventoryClick()
        {
            _inventoryController.OpenWindow();
        }

        private void OnHealthChanged()
        {
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            var current = _healthModule.Data.Health;
            var max = _healthModule.Config.MaxHealth;
            _view.SetHealth(Mathf.RoundToInt(current), Mathf.RoundToInt(max));
        }

        private void CreateView()
        {
            var creator = new HUDCreator(_assetManager, _canvasController);
            var view = creator.Create();
            _view = view.Value;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}