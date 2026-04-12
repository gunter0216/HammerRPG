using System;
using App.Common.AssetSystem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Canvases.External;
using App.Game.CharacterWindow.External.Controllers;
using App.Game.Player.External;

namespace App.Game.CharacterWindow.External
{
    public class CharacterWindowController : BaseWindowController<View.CharacterWindow>, IWindowController, IDisposable
    {
        private const string _windowAssetKey = "CharacterWindow";
        
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly PlayerController _playerController;

        private MainInfoController _infoController;
        private StatsInfoController _statsInfoController;
        
        public CharacterWindowController(
            IWindowManager windowManager,
            IAssetManager assetManager,
            PopupCanvas canvas,
            IItemSpriteLoader spriteLoader, 
            PlayerController playerController) : base(windowManager, assetManager, canvas)
        {
            _spriteLoader = spriteLoader;
            _playerController = playerController;
        }

        protected override void OnInitWindow()
        {
            base.OnInitWindow();
            
            _window.SetCloseButtonClickCallback(OnCloseButtonClick);

            _infoController = new MainInfoController(_window, _playerController);
            _statsInfoController = new StatsInfoController(_window);
            _infoController.Init();
            _statsInfoController.Init();
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            
            _infoController.UpdateInfo();
            _statsInfoController.UpdateInfo();
        }

        private void OnCloseButtonClick()
        {
            Close();
        }

        public override WindowNames GetName()
        {
            return WindowNames.Character;
        }

        protected override string GetWindowAssetKey()
        {
            return _windowAssetKey;
        }

        public void Dispose()
        {
        }
    }
}