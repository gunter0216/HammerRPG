using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using UnityEngine;

namespace App.Common.Windows.Runtime
{
    public abstract class BaseWindowController<T> : IWindowController where T : MonoBehaviour
    {
        protected T _window;
        
        protected readonly IWindowManager _windowManager;
        protected readonly IAssetManager _assetManager;
        protected readonly ICanvasController _canvasController;

        protected BaseWindowController(
            IWindowManager windowManager,
            IAssetManager assetManager, 
            ICanvasController canvasController)
        {
            _windowManager = windowManager;
            _assetManager = assetManager;
            _canvasController = canvasController;
        }

        public void Open()
        {
            if (_window == null)
            {
                if (!CreateWindow())
                {
                    HLogger.LogError($"Failed to create {nameof(T)} window.");
                    return;
                }
            }

            _windowManager.Open(this);
        }

        public void Close()
        {
            _windowManager.Close(this);
        }
        
        public bool IsOpen()
        {
            return _window != null && _window.gameObject.activeSelf;
        }
        
        public bool CreateWindow()
        {
            var window = CreateWindowInstance();
            if (!window.HasValue)
            {
                return false;
            }

            _window = window.Value;
            SetActive(false);
            InitWindow();
            
            return true;
        }

        private Optional<T> CreateWindowInstance()
        {
            var window = _assetManager.InstantiateSync<T>(
                new StringKeyEvaluator(GetWindowAssetKey()),
                _canvasController.GetWindowCanvas().GetContent());
            return window;
        }

        private void InitWindow()
        {
            _windowManager.Registry(this, CreateWindowConfig());

            OnInitWindow();
        }

        protected virtual WindowConfig CreateWindowConfig()
        {
            return new WindowConfig(onOpened: OnOpened);
        }

        protected virtual void OnInitWindow() {}
        protected virtual void OnOpened() {}
        protected abstract string GetWindowAssetKey();

        public void SetActive(bool status)
        {
            _window.gameObject.SetActive(status);
        }

        public abstract WindowNames GetName();
    }
}