using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.StatusBar.External.Controller;
using App.Game.StatusBar.External.View;
using App.Game.StatusBar.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Game.StatusBar.External
{
    public class StatusBarController : IInitSystem, IStatusBarController, IUpdateSystem, IDisposable
    {
        private const string _assetKey = "StatusBarView";
        
        private readonly ICanvasController _canvasController;
        private readonly IAssetManager _assetManager;
        
        private StatusBarView _prefab;
        private ListPool<StatusBarViewController> _pool;
        private Dictionary<ModuleItemView, StatusBarViewController> _controllers;

        public StatusBarController(ICanvasController canvasController, IAssetManager assetManager)
        {
            _canvasController = canvasController;
            _assetManager = assetManager;
        }

        public void Init()
        {
            if (!_assetManager.TryLoadSync<GameObject>(_assetKey, out var prefab))
            {
                HLogger.LogError("StatusBarView cant load.");
                return;
            }

            _prefab = prefab.GetComponent<StatusBarView>();

            _pool = new ListPool<StatusBarViewController>(Create);
            _controllers = new Dictionary<ModuleItemView, StatusBarViewController>();
        }

        private Optional<StatusBarViewController> Create()
        {
            var asset = Object.Instantiate(_prefab, _canvasController.GetStatusBarCanvas().GetContent());
            asset.SetActive(false);
            var controller = new StatusBarViewController(asset);
            
            return Optional<StatusBarViewController>.Success(controller);
        }

        public void OnUpdate()
        {
            if (_controllers == null)
            {
                return;
            }
            
            foreach (var controller in _controllers)
            {
                controller.Value.OnUpdate();
            }
        }

        public void Show(ModuleItemView view)
        {
            var controller = _pool.Get().Value;
            _controllers.Add(view, controller);
            controller.Activate(view);
        }

        public void Hide(ModuleItemView view)
        {
            if (!_controllers.TryGetValue(view, out var controller))
            {
                HLogger.LogError("Not found controller.");
                return;
            }

            _controllers.Remove(view);
            controller.Deactivate();
        }

        public void Dispose()
        {
            _assetManager.UnloadAsset(_assetKey);
            _pool?.Dispose();
        }
    }
}